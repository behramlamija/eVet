import 'dart:async';

import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'karton_screen.dart';
import 'pacijent_forma.dart';

enum _Starost {
  sve('Sve', null, null),
  doGodine('Do 1 godine', 0, 0),
  mladi('1-3 godine', 1, 3),
  odrasli('4-7 godina', 4, 7),
  stariji('8 i više godina', 8, null);

  const _Starost(this.naziv, this.od, this.do_);

  final String naziv;
  final int? od;
  final int? do_;
}

/// Pregled, pretraga i filtriranje pacijenata (mockup sa slike 6 u prijavi).
class PacijentiScreen extends StatefulWidget {
  const PacijentiScreen({super.key});

  @override
  State<PacijentiScreen> createState() => _PacijentiScreenState();
}

class _PacijentiScreenState extends State<PacijentiScreen> {
  static const _velicinaStranice = 10;

  final _pretraga = TextEditingController();
  Timer? _debounce;
  int _stranica = 0;
  int? _vrstaId;
  bool _samoVakcinisani = false;
  _Starost _starost = _Starost.sve;
  Spol? _spol;
  late Future<PagedResult<Ljubimci>> _pacijenti;
  late final Future<PagedResult<VrstaLjubimca>> _vrste;

  @override
  void initState() {
    super.initState();
    _vrste = context.read<VrsteLjubimacaProvider>().get(pageSize: BaseProvider.maxPageSize);
    _ucitaj();
  }

  @override
  void dispose() {
    _debounce?.cancel();
    _pretraga.dispose();
    super.dispose();
  }

  void _ucitaj() {
    _pacijenti = context.read<LjubimciProvider>().get(
      page: _stranica,
      pageSize: _velicinaStranice,
      filter: {
        'FTS': _pretraga.text.trim(),
        'VrstaLjubimcaId': _vrstaId,
        'JeVakcinisan': _samoVakcinisani ? true : null,
        'StarostOd': _starost.od,
        'StarostDo': _starost.do_,
        'Spol': _spol,
      },
    );
  }

  void _filtriraj(VoidCallback promjena) => setState(() {
        promjena();
        _stranica = 0;
        _ucitaj();
      });

  Future<void> _dodaj() async {
    final dodan = await otvoriFormuPacijenta(context);
    if (dodan) _filtriraj(() {});
  }

  Future<void> _otvoriKarton(Ljubimci pacijent) async {
    await Navigator.of(context).push(MaterialPageRoute(builder: (_) => KartonScreen(ljubimacId: pacijent.ljubimacId)));
    if (mounted) setState(_ucitaj);
  }

  @override
  Widget build(BuildContext context) {
    final slike = context.read<SlikeProvider>();

    return Padding(
      padding: const EdgeInsets.all(32),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: [
                Text('PACIJENTI', style: Theme.of(context).textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold)),
                const SizedBox(height: 12),
                TextField(
                  controller: _pretraga,
                  decoration: const InputDecoration(hintText: 'Pretražite po imenu ljubimca ili vlasnika', prefixIcon: Icon(Icons.search)),
                  onChanged: (_) {
                    _debounce?.cancel();
                    _debounce = Timer(const Duration(milliseconds: 400), () => _filtriraj(() {}));
                  },
                ),
                const SizedBox(height: 12),
                Expanded(
                  child: FutureBuilder<PagedResult<Ljubimci>>(
                    future: _pacijenti,
                    builder: (context, snapshot) {
                      if (snapshot.hasError) return PorukaGreske(snapshot.error.toString(), onPokusajPonovo: () => setState(_ucitaj));
                      if (!snapshot.hasData) return const Center(child: CircularProgressIndicator());
                      final rezultat = snapshot.data!;
                      return Column(
                        children: [
                          Expanded(
                            child: Card(
                              child: rezultat.resultList.isEmpty
                                  ? const Center(child: Text('Nema pacijenata za zadanu pretragu.'))
                                  : ListView.separated(
                                      itemCount: rezultat.resultList.length,
                                      separatorBuilder: (_, _) => const Divider(height: 1),
                                      itemBuilder: (_, i) {
                                        final p = rezultat.resultList[i];
                                        return ListTile(
                                          onTap: () => _otvoriKarton(p),
                                          leading: EvetSlika(slike: slike, tip: TipSlike.ljubimac, id: p.ljubimacId, imaSliku: p.imaSliku),
                                          title: Text(p.ime, style: const TextStyle(fontWeight: FontWeight.w600)),
                                          subtitle: Text('${p.vrstaLjubimcaNaziv} • ${p.rasaNaziv} • ${nazivSpola(p.spol)}'),
                                          trailing: Text('${p.korisnikIme} ${p.korisnikPrezime}'),
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
          ),
          const SizedBox(width: 24),
          SizedBox(
            width: 280,
            child: Card(
              child: Padding(
                padding: const EdgeInsets.all(16),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: [
                    const Row(children: [Icon(Icons.tune, color: EvetBoje.tamnoplava), SizedBox(width: 8), Text('Filter', style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold))]),
                    const SizedBox(height: 16),
                    FutureBuilder<PagedResult<VrstaLjubimca>>(
                      future: _vrste,
                      builder: (context, snapshot) => DropdownButtonFormField<int?>(
                        initialValue: _vrstaId,
                        decoration: const InputDecoration(labelText: 'Vrsta'),
                        items: [
                          const DropdownMenuItem(value: null, child: Text('Sve vrste')),
                          for (final v in snapshot.data?.resultList ?? const <VrstaLjubimca>[]) DropdownMenuItem(value: v.vrstaLjubimcaId, child: Text(v.naziv)),
                        ],
                        onChanged: (v) => _filtriraj(() => _vrstaId = v),
                      ),
                    ),
                    const SizedBox(height: 8),
                    CheckboxListTile(
                      contentPadding: EdgeInsets.zero,
                      title: const Text('Vakcinisani u zadnjih godinu dana'),
                      value: _samoVakcinisani,
                      onChanged: (v) => _filtriraj(() => _samoVakcinisani = v ?? false),
                    ),
                    const SizedBox(height: 8),
                    DropdownButtonFormField<_Starost>(
                      initialValue: _starost,
                      decoration: const InputDecoration(labelText: 'Starost'),
                      items: [for (final s in _Starost.values) DropdownMenuItem(value: s, child: Text(s.naziv))],
                      onChanged: (s) => _filtriraj(() => _starost = s ?? _Starost.sve),
                    ),
                    const SizedBox(height: 16),
                    const Text('Spol'),
                    RadioGroup<Spol?>(
                      groupValue: _spol,
                      onChanged: (s) => _filtriraj(() => _spol = s),
                      child: const Column(
                        children: [
                          RadioListTile<Spol?>(contentPadding: EdgeInsets.zero, value: null, title: Text('Svi')),
                          RadioListTile<Spol?>(contentPadding: EdgeInsets.zero, value: Spol.muski, title: Text('Muški')),
                          RadioListTile<Spol?>(contentPadding: EdgeInsets.zero, value: Spol.zenski, title: Text('Ženski')),
                        ],
                      ),
                    ),
                    const Spacer(),
                    FilledButton.icon(onPressed: _dodaj, icon: const Icon(Icons.add), label: const Text('Dodajte pacijenta')),
                  ],
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}
