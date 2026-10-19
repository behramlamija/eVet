import 'dart:async';

import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'usluga_forma.dart';

/// Katalog usluga: pretraga, filter po vrsti, dodavanje, uređivanje i brisanje.
class UslugeTab extends StatefulWidget {
  const UslugeTab({super.key});

  @override
  State<UslugeTab> createState() => _UslugeTabState();
}

class _UslugeTabState extends State<UslugeTab> {
  static const _velicinaStranice = 10;

  final _pretraga = TextEditingController();
  Timer? _debounce;
  int _stranica = 0;
  int? _vrstaId;
  int _verzijaSlika = 0;
  late Future<PagedResult<Usluge>> _usluge;
  late final Future<PagedResult<VrstaUsluge>> _vrste;

  @override
  void initState() {
    super.initState();
    _vrste = context.read<VrsteUslugaProvider>().get(pageSize: BaseProvider.maxPageSize);
    _ucitaj();
  }

  @override
  void dispose() {
    _debounce?.cancel();
    _pretraga.dispose();
    super.dispose();
  }

  void _ucitaj() => _usluge = context.read<UslugeProvider>().get(
        page: _stranica,
        pageSize: _velicinaStranice,
        filter: {'FTS': _pretraga.text.trim(), 'VrstaId': _vrstaId},
      );

  void _osvjezi({bool naPocetak = false}) => setState(() {
        if (naPocetak) _stranica = 0;
        _verzijaSlika++;
        _ucitaj();
      });

  Future<void> _forma([Usluge? usluga]) async {
    if (await otvoriFormuUsluge(context, usluga: usluga)) _osvjezi(naPocetak: usluga == null);
  }

  Future<void> _obrisi(Usluge u) async {
    final potvrda = await potvrdiAkciju(context, naslov: 'Brisanje usluge', poruka: 'Obrisati uslugu "${u.naziv}"?', potvrda: 'Obriši', opasna: true);
    if (!potvrda || !mounted) return;
    if (await izvrsiAkciju(context, () => context.read<UslugeProvider>().delete(u.uslugaId), uspjeh: 'Usluga "${u.naziv}" je obrisana.')) _osvjezi();
  }

  @override
  Widget build(BuildContext context) {
    final slike = context.read<SlikeProvider>();

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
                  decoration: const InputDecoration(hintText: 'Pretraga po nazivu ili opisu', prefixIcon: Icon(Icons.search), isDense: true),
                  onChanged: (_) {
                    _debounce?.cancel();
                    _debounce = Timer(const Duration(milliseconds: 400), () => _osvjezi(naPocetak: true));
                  },
                ),
              ),
              const SizedBox(width: 12),
              SizedBox(
                width: 220,
                child: FutureBuilder<PagedResult<VrstaUsluge>>(
                  future: _vrste,
                  builder: (context, s) => DropdownButtonFormField<int?>(
                    initialValue: _vrstaId,
                    decoration: const InputDecoration(labelText: 'Vrsta usluge', isDense: true),
                    items: [
                      const DropdownMenuItem(value: null, child: Text('Sve vrste')),
                      for (final v in s.data?.resultList ?? const <VrstaUsluge>[]) DropdownMenuItem(value: v.vrstaId, child: Text(v.naziv)),
                    ],
                    onChanged: (v) {
                      _vrstaId = v;
                      _osvjezi(naPocetak: true);
                    },
                  ),
                ),
              ),
              const SizedBox(width: 12),
              FilledButton.icon(onPressed: () => _forma(), icon: const Icon(Icons.add), label: const Text('Dodaj uslugu')),
            ],
          ),
          const SizedBox(height: 12),
          Expanded(
            child: FutureBuilder<PagedResult<Usluge>>(
              future: _usluge,
              builder: (context, snapshot) {
                if (snapshot.hasError) return PorukaGreske(snapshot.error.toString(), onPokusajPonovo: _osvjezi);
                if (!snapshot.hasData) return const Center(child: CircularProgressIndicator());
                final rezultat = snapshot.data!;
                return Column(
                  children: [
                    Expanded(
                      child: Card(
                        child: rezultat.resultList.isEmpty
                            ? const Center(child: Text('Nema usluga za zadanu pretragu.'))
                            : ListView.separated(
                                itemCount: rezultat.resultList.length,
                                separatorBuilder: (_, _) => const Divider(height: 1),
                                itemBuilder: (_, i) {
                                  final u = rezultat.resultList[i];
                                  return ListTile(
                                    leading: EvetSlika(slike: slike, tip: TipSlike.usluga, id: u.uslugaId, imaSliku: u.imaSliku, zamjenskaIkona: Icons.medical_services_outlined, verzija: _verzijaSlika),
                                    title: Text(u.naziv, style: const TextStyle(fontWeight: FontWeight.w600)),
                                    subtitle: Text('${u.vrstaNaziv} • ${u.trajanje} min'),
                                    trailing: Row(
                                      mainAxisSize: MainAxisSize.min,
                                      children: [
                                        Text(formatNovac(u.cijena), style: const TextStyle(fontWeight: FontWeight.bold)),
                                        const SizedBox(width: 16),
                                        IconButton(tooltip: 'Uredi uslugu', onPressed: () => _forma(u), icon: const Icon(Icons.edit_outlined)),
                                        IconButton(tooltip: 'Obriši uslugu', onPressed: () => _obrisi(u), icon: const Icon(Icons.delete_outline)),
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
