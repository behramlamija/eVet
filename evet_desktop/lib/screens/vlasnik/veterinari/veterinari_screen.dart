import 'dart:async';

import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'veterinar_forma.dart';
import 'veterinar_profil_dijalog.dart';

/// Pregled uposlenika s brojem termina i recenzija (mockup sa slike 4 u prijavi).
class VeterinariScreen extends StatefulWidget {
  const VeterinariScreen({super.key});

  @override
  State<VeterinariScreen> createState() => _VeterinariScreenState();
}

class _VeterinariScreenState extends State<VeterinariScreen> {
  static const _velicinaStranice = 10;

  final _pretraga = TextEditingController();
  Timer? _debounce;
  int _stranica = 0;
  late Future<PagedResult<StatistikaVeterinara>> _veterinari;

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

  void _ucitaj() => _veterinari = context.read<StatistikaProvider>().veterinari(fts: _pretraga.text.trim(), page: _stranica, pageSize: _velicinaStranice);

  void _osvjezi({bool naPocetak = false}) => setState(() {
        if (naPocetak) _stranica = 0;
        _ucitaj();
      });

  Future<void> _dodaj() async {
    if (await otvoriFormuVeterinara(context)) _osvjezi(naPocetak: true);
  }

  Future<void> _profil(StatistikaVeterinara v) async {
    await prikaziProfilVeterinara(context, v.korisnikId);
    if (mounted) _osvjezi();
  }

  Future<void> _ukloni(StatistikaVeterinara v) async {
    final potvrda = await potvrdiAkciju(
      context,
      naslov: 'Uklanjanje veterinara',
      poruka: 'Ukloniti veterinara ${v.ime} ${v.prezime}? Veterinar s aktivnim terminima ne može biti uklonjen; tada ga možete deaktivirati.',
      potvrda: 'Ukloni',
      opasna: true,
    );
    if (!potvrda || !mounted) return;
    if (await izvrsiAkciju(context, () => context.read<KorisniciProvider>().delete(v.korisnikId), uspjeh: 'Veterinar ${v.ime} ${v.prezime} je uklonjen.')) {
      _osvjezi();
    }
  }

  String _recenzije(StatistikaVeterinara v) {
    final ukupno = v.pozitivneRecenzije + v.negativneRecenzije;
    if (ukupno == 0) return 'Nema recenzija';
    return '${(v.pozitivneRecenzije * 100 / ukupno).round()}% pozitivnih ($ukupno)';
  }

  @override
  Widget build(BuildContext context) {
    final slike = context.read<SlikeProvider>();

    return ListView(
      padding: const EdgeInsets.all(32),
      children: [
        Row(
          children: [
            Text('VETERINARI', style: Theme.of(context).textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold)),
            const Spacer(),
            FilledButton.icon(onPressed: _dodaj, icon: const Icon(Icons.person_add_alt), label: const Text('Dodajte veterinara')),
          ],
        ),
        const SizedBox(height: 12),
        TextField(
          controller: _pretraga,
          decoration: const InputDecoration(hintText: 'Pretražite po imenu ili prezimenu', prefixIcon: Icon(Icons.search)),
          onChanged: (_) {
            _debounce?.cancel();
            _debounce = Timer(const Duration(milliseconds: 400), () => _osvjezi(naPocetak: true));
          },
        ),
        const SizedBox(height: 12),
        FutureBuilder<PagedResult<StatistikaVeterinara>>(
          future: _veterinari,
          builder: (context, snapshot) {
            if (snapshot.hasError) return PorukaGreske(snapshot.error.toString(), onPokusajPonovo: _osvjezi);
            if (!snapshot.hasData) return const Center(child: CircularProgressIndicator());
            final rezultat = snapshot.data!;
            if (rezultat.resultList.isEmpty) return const Card(child: ListTile(title: Text('Nema veterinara za zadanu pretragu.')));

            return Column(
              children: [
                Card(
                  child: SizedBox(
                    width: double.infinity,
                    child: DataTable(
                      showCheckboxColumn: false,
                      columns: const [
                        DataColumn(label: Text('Redni broj')),
                        DataColumn(label: Text('Veterinar')),
                        DataColumn(label: Text('Specijalnost')),
                        DataColumn(label: Text('Termini'), numeric: true),
                        DataColumn(label: Text('Recenzije')),
                        DataColumn(label: Text('Status')),
                        DataColumn(label: Text('')),
                      ],
                      rows: [
                        for (final (i, v) in rezultat.resultList.indexed)
                          DataRow(
                            onSelectChanged: (_) => _profil(v),
                            cells: [
                              DataCell(Text('${_stranica * _velicinaStranice + i + 1}')),
                              DataCell(Row(children: [
                                EvetSlika(slike: slike, tip: TipSlike.korisnik, id: v.korisnikId, imaSliku: v.imaSliku, velicina: 36, zamjenskaIkona: Icons.person),
                                const SizedBox(width: 12),
                                Text('${v.ime} ${v.prezime}'),
                              ])),
                              DataCell(Text(v.specijalnostNaziv ?? '-')),
                              DataCell(Text('${v.brojTermina}')),
                              DataCell(Text(_recenzije(v))),
                              DataCell(Text(v.jeAktivan ? 'Aktivan' : 'Neaktivan', style: TextStyle(color: v.jeAktivan ? Colors.green.shade700 : EvetBoje.tekstSivi))),
                              DataCell(Row(
                                mainAxisSize: MainAxisSize.min,
                                children: [
                                  IconButton(tooltip: 'Profil veterinara', onPressed: () => _profil(v), icon: const Icon(Icons.badge_outlined)),
                                  IconButton(tooltip: 'Ukloni veterinara', onPressed: () => _ukloni(v), icon: const Icon(Icons.person_remove_outlined)),
                                ],
                              )),
                            ],
                          ),
                      ],
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
      ],
    );
  }
}
