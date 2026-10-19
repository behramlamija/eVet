import 'dart:async';

import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'promocija_forma.dart';

/// Promocije (kuponi) za usluge: pretraga, filter važećih, dodavanje, uređivanje i brisanje.
class PromocijeTab extends StatefulWidget {
  const PromocijeTab({super.key});

  @override
  State<PromocijeTab> createState() => _PromocijeTabState();
}

class _PromocijeTabState extends State<PromocijeTab> {
  static const _velicinaStranice = 10;

  final _pretraga = TextEditingController();
  Timer? _debounce;
  int _stranica = 0;
  bool _samoVazece = false;
  late Future<PagedResult<Promocije>> _promocije;

  @override
  void initState() {
    super.initState();
    _ucitaj();
  }

  @override
  void dispose() {
    _debounce?.cancel();
    _pretraga.dispose();
    super.dispose();
  }

  void _ucitaj() => _promocije = context.read<PromocijeProvider>().get(
        page: _stranica,
        pageSize: _velicinaStranice,
        filter: {'FTS': _pretraga.text.trim(), 'SamoVazece': _samoVazece ? true : null},
      );

  void _osvjezi({bool naPocetak = false}) => setState(() {
        if (naPocetak) _stranica = 0;
        _ucitaj();
      });

  Future<void> _forma([Promocije? promocija]) async {
    if (await otvoriFormuPromocije(context, promocija: promocija)) _osvjezi(naPocetak: promocija == null);
  }

  Future<void> _obrisi(Promocije p) async {
    final potvrda = await potvrdiAkciju(context, naslov: 'Brisanje promocije', poruka: 'Obrisati promociju "${p.naziv}" (kod ${p.kod})?', potvrda: 'Obriši', opasna: true);
    if (!potvrda || !mounted) return;
    if (await izvrsiAkciju(context, () => context.read<PromocijeProvider>().delete(p.promocijaId), uspjeh: 'Promocija "${p.naziv}" je obrisana.')) _osvjezi();
  }

  bool _vazi(Promocije p) {
    final sada = DateTime.now();
    return p.jeAktivna && !p.datumPocetka.isAfter(sada) && !p.datumKraja.isBefore(sada);
  }

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(24),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Row(
            children: [
              Expanded(
                child: TextField(
                  controller: _pretraga,
                  decoration: const InputDecoration(hintText: 'Pretraga po nazivu, kodu ili usluzi', prefixIcon: Icon(Icons.search), isDense: true),
                  onChanged: (_) {
                    _debounce?.cancel();
                    _debounce = Timer(const Duration(milliseconds: 400), () => _osvjezi(naPocetak: true));
                  },
                ),
              ),
              const SizedBox(width: 12),
              SizedBox(
                width: 200,
                child: CheckboxListTile(
                  title: const Text('Samo važeće'),
                  value: _samoVazece,
                  onChanged: (v) {
                    _samoVazece = v ?? false;
                    _osvjezi(naPocetak: true);
                  },
                ),
              ),
              FilledButton.icon(onPressed: () => _forma(), icon: const Icon(Icons.add), label: const Text('Dodaj promociju')),
            ],
          ),
          const SizedBox(height: 12),
          Expanded(
            child: FutureBuilder<PagedResult<Promocije>>(
              future: _promocije,
              builder: (context, snapshot) {
                if (snapshot.hasError) return PorukaGreske(snapshot.error.toString(), onPokusajPonovo: _osvjezi);
                if (!snapshot.hasData) return const Center(child: CircularProgressIndicator());
                final rezultat = snapshot.data!;
                return Column(
                  children: [
                    Expanded(
                      child: Card(
                        child: rezultat.resultList.isEmpty
                            ? const Center(child: Text('Nema promocija za zadanu pretragu.'))
                            : ListView.separated(
                                itemCount: rezultat.resultList.length,
                                separatorBuilder: (_, _) => const Divider(height: 1),
                                itemBuilder: (_, i) {
                                  final p = rezultat.resultList[i];
                                  final vazi = _vazi(p);
                                  return ListTile(
                                    leading: CircleAvatar(
                                      backgroundColor: vazi ? EvetBoje.crvena : Colors.grey.shade300,
                                      child: Text('${p.popust.round()}%', style: TextStyle(color: vazi ? Colors.white : Colors.black54, fontSize: 12, fontWeight: FontWeight.bold)),
                                    ),
                                    title: Text('${p.naziv} • ${p.kod}', style: const TextStyle(fontWeight: FontWeight.w600)),
                                    subtitle: Text('${p.uslugaNaziv} (${formatNovac(p.uslugaCijena)}) • ${formatDatum(p.datumPocetka)} - ${formatDatum(p.datumKraja)}'),
                                    trailing: Row(
                                      mainAxisSize: MainAxisSize.min,
                                      children: [
                                        Chip(label: Text(vazi ? 'Važi' : (p.jeAktivna ? 'Van perioda' : 'Neaktivna'))),
                                        IconButton(tooltip: 'Uredi promociju', onPressed: () => _forma(p), icon: const Icon(Icons.edit_outlined)),
                                        IconButton(tooltip: 'Obriši promociju', onPressed: () => _obrisi(p), icon: const Icon(Icons.delete_outline)),
                                      ],
                                    ),
                                  );
                                },
                              ),
                      ),
                    ),
                    Paginacija(
                      stranica: _stranica,
                      ukupno: rezultat.count,
                      velicinaStranice: _velicinaStranice,
                      onPromjena: (s) => setState(() {
                        _stranica = s;
                        _ucitaj();
                      }),
                    ),
                  ],
                );
              },
            ),
          ),
        ],
      ),
    );
  }
}
