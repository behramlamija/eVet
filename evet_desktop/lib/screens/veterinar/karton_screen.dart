import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'pacijent_forma.dart';
import 'terapija_forma.dart';
import 'termin_detalji_dijalog.dart';
import 'terapije_tabela.dart';

/// Karton pacijenta (master-detail): podaci o ljubimcu, terapije i historija termina.
class KartonScreen extends StatefulWidget {
  const KartonScreen({super.key, required this.ljubimacId});

  final int ljubimacId;

  @override
  State<KartonScreen> createState() => _KartonScreenState();
}

class _KartonScreenState extends State<KartonScreen> {
  late Future<Ljubimci> _pacijent;
  late Future<PagedResult<Terapije>> _terapije;
  late Future<PagedResult<Termini>> _termini;
  int _verzijaSlike = 0;

  @override
  void initState() {
    super.initState();
    _ucitaj();
  }

  void _ucitaj() {
    final filter = {'LjubimacId': widget.ljubimacId};
    _pacijent = context.read<LjubimciProvider>().getById(widget.ljubimacId);
    _terapije = context.read<TerapijeProvider>().get(filter: filter, pageSize: BaseProvider.maxPageSize);
    _termini = context.read<TerminiProvider>().get(filter: filter, pageSize: BaseProvider.maxPageSize);
  }

  Future<void> _uredi(Ljubimci p) async {
    if (await otvoriFormuPacijenta(context, pacijent: p)) setState(_ucitaj);
  }

  Future<void> _promijeniSliku(Ljubimci p) async {
    try {
      final slika = await odaberiSliku();
      if (slika == null || !mounted) return;
      final ok = await izvrsiAkciju(
        context,
        () => context.read<SlikeProvider>().postavi(TipSlike.ljubimac, p.ljubimacId, bytes: slika.bytes, nazivDatoteke: slika.naziv, contentType: slika.contentType),
        uspjeh: 'Slika pacijenta ${p.ime} je promijenjena.',
      );
      if (ok) {
        setState(() {
          _verzijaSlike++;
          _ucitaj();
        });
      }
    } on ApiException catch (e) {
      if (mounted) prikaziGresku(context, e.poruka);
    }
  }

  Future<void> _dodajTerapiju(Ljubimci p) async {
    if (await otvoriFormuTerapije(context, ljubimacId: p.ljubimacId, ljubimacIme: p.ime)) setState(_ucitaj);
  }

  Future<void> _obrisi(Ljubimci p) async {
    final potvrda = await potvrdiAkciju(
      context,
      naslov: 'Brisanje pacijenta',
      poruka: 'Obrisati pacijenta ${p.ime}? Pacijent s aktivnim terminima ne može biti obrisan.',
      potvrda: 'Obriši',
      opasna: true,
    );
    if (!potvrda || !mounted) return;
    if (await izvrsiAkciju(context, () => context.read<LjubimciProvider>().delete(p.ljubimacId), uspjeh: 'Pacijent ${p.ime} je obrisan.') && mounted) {
      Navigator.pop(context);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Karton pacijenta')),
      body: FutureBuilder<Ljubimci>(
        future: _pacijent,
        builder: (context, snapshot) {
          if (snapshot.hasError) {
            return Padding(padding: const EdgeInsets.all(32), child: PorukaGreske(snapshot.error.toString(), onPokusajPonovo: () => setState(_ucitaj)));
          }
          if (!snapshot.hasData) return const Center(child: CircularProgressIndicator());
          final p = snapshot.data!;

          return ListView(
            padding: const EdgeInsets.all(32),
            children: [
              Card(
                child: Padding(
                  padding: const EdgeInsets.all(20),
                  child: Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Column(
                        children: [
                          EvetSlika(slike: context.read<SlikeProvider>(), tip: TipSlike.ljubimac, id: p.ljubimacId, imaSliku: p.imaSliku, velicina: 110, verzija: _verzijaSlike),
                          TextButton.icon(onPressed: () => _promijeniSliku(p), icon: const Icon(Icons.photo_camera_outlined), label: const Text('Promijeni sliku')),
                        ],
                      ),
                      const SizedBox(width: 24),
                      Expanded(
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(p.ime, style: Theme.of(context).textTheme.headlineMedium?.copyWith(fontWeight: FontWeight.bold)),
                            Text('${p.korisnikIme} ${p.korisnikPrezime}', style: const TextStyle(fontSize: 16, color: EvetBoje.tekstSivi)),
                            const SizedBox(height: 12),
                            Wrap(
                              spacing: 24,
                              runSpacing: 8,
                              children: [
                                _Podatak(Icons.pets, '${p.vrstaLjubimcaNaziv}, ${p.rasaNaziv}'),
                                _Podatak(Icons.wc, nazivSpola(p.spol)),
                                _Podatak(Icons.cake_outlined, p.datumRodjenja == null ? 'Datum rođenja nepoznat' : formatDatum(p.datumRodjenja)),
                                _Podatak(Icons.monitor_weight_outlined, p.tezina == null ? 'Težina nije unesena' : '${p.tezina} kg'),
                              ],
                            ),
                            if (p.napomena != null) ...[const SizedBox(height: 12), Text('Napomena: ${p.napomena}')],
                          ],
                        ),
                      ),
                      Column(
                        crossAxisAlignment: CrossAxisAlignment.end,
                        children: [
                          OutlinedButton.icon(onPressed: () => _uredi(p), icon: const Icon(Icons.edit_outlined), label: const Text('Uredi podatke')),
                          const SizedBox(height: 8),
                          TextButton.icon(
                            onPressed: () => _obrisi(p),
                            icon: const Icon(Icons.delete_outline),
                            label: const Text('Obriši pacijenta'),
                            style: TextButton.styleFrom(foregroundColor: Theme.of(context).colorScheme.error),
                          ),
                        ],
                      ),
                    ],
                  ),
                ),
              ),
              const SizedBox(height: 24),
              Row(
                children: [
                  Text('Terapije', style: Theme.of(context).textTheme.titleLarge?.copyWith(fontWeight: FontWeight.bold)),
                  const Spacer(),
                  FilledButton.icon(onPressed: () => _dodajTerapiju(p), icon: const Icon(Icons.add), label: const Text('Dodaj novu terapiju')),
                ],
              ),
              const SizedBox(height: 8),
              FutureBuilder<PagedResult<Terapije>>(
                future: _terapije,
                builder: (context, s) {
                  if (s.hasError) return PorukaGreske(s.error.toString());
                  if (!s.hasData) return const LinearProgressIndicator();
                  return TerapijeTabela(terapije: s.data!.resultList, prikaziPacijenta: false, onPromjena: () => setState(_ucitaj));
                },
              ),
              const SizedBox(height: 24),
              Text('Historija termina', style: Theme.of(context).textTheme.titleLarge?.copyWith(fontWeight: FontWeight.bold)),
              const SizedBox(height: 8),
              FutureBuilder<PagedResult<Termini>>(
                future: _termini,
                builder: (context, s) {
                  if (s.hasError) return PorukaGreske(s.error.toString());
                  if (!s.hasData) return const LinearProgressIndicator();
                  final termini = s.data!.resultList;
                  if (termini.isEmpty) return const Card(child: ListTile(title: Text('Pacijent nema termina.')));
                  return Card(
                    child: Column(
                      children: [
                        for (final t in termini)
                          ListTile(
                            onTap: () => prikaziDetaljeTermina(context, t),
                            title: Text(t.stavke.map((x) => x.uslugaNaziv).join(', ')),
                            subtitle: Text('${formatDatum(t.datumTermina)} u ${formatVrijeme(t.vrijemePocetka)} • ${t.veterinarIme} ${t.veterinarPrezime}'),
                            trailing: StatusTerminaChip(t.status),
                          ),
                      ],
                    ),
                  );
                },
              ),
            ],
          );
        },
      ),
    );
  }
}

class _Podatak extends StatelessWidget {
  const _Podatak(this.ikona, this.tekst);

  final IconData ikona;
  final String tekst;

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisSize: MainAxisSize.min,
      children: [Icon(ikona, size: 18, color: EvetBoje.tamnoplava), const SizedBox(width: 6), Text(tekst)],
    );
  }
}
