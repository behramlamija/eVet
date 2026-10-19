import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Pregled recenzija klijenata (palac gore/dolje za uslugu i veterinara), odgovori i uklanjanje neprimjerenih.
class RecenzijeScreen extends StatefulWidget {
  const RecenzijeScreen({super.key});

  @override
  State<RecenzijeScreen> createState() => _RecenzijeScreenState();
}

class _RecenzijeScreenState extends State<RecenzijeScreen> {
  static const _velicinaStranice = 10;

  int _stranica = 0;
  int? _veterinarId;
  bool? _pozitivna;
  late Future<PagedResult<Recenzije>> _recenzije;
  late final Future<PagedResult<KorisnikOsnovno>> _veterinari;

  @override
  void initState() {
    super.initState();
    _veterinari = context.read<VeterinariProvider>().get(pageSize: BaseProvider.maxPageSize);
    _ucitaj();
  }

  void _ucitaj() => _recenzije = context.read<RecenzijeProvider>().get(
        page: _stranica,
        pageSize: _velicinaStranice,
        filter: {'VeterinarId': _veterinarId, 'VeterinarPozitivan': _pozitivna},
      );

  void _osvjezi({bool naPocetak = false}) => setState(() {
        if (naPocetak) _stranica = 0;
        _ucitaj();
      });

  Future<void> _odgovori(Recenzije r) async {
    final odgovor = await unesiRazlog(context, naslov: 'Odgovor na recenziju klijenta ${r.korisnikIme} ${r.korisnikPrezime}', potvrda: 'Pošalji odgovor', labela: 'Odgovor');
    if (odgovor == null || !mounted) return;
    if (await izvrsiAkciju(context, () => context.read<RecenzijeProvider>().odgovori(r.recenzijaId, odgovor), uspjeh: 'Odgovor je poslan. Klijent je obaviješten.')) {
      _osvjezi();
    }
  }

  Future<void> _obrisi(Recenzije r) async {
    final potvrda = await potvrdiAkciju(context, naslov: 'Brisanje recenzije', poruka: 'Obrisati recenziju klijenta ${r.korisnikIme} ${r.korisnikPrezime} za termin ${r.terminSifra}?', potvrda: 'Obriši', opasna: true);
    if (!potvrda || !mounted) return;
    if (await izvrsiAkciju(context, () => context.read<RecenzijeProvider>().delete(r.recenzijaId), uspjeh: 'Recenzija je obrisana.')) _osvjezi();
  }

  Widget _palac(String naziv, bool pozitivno) {
    return Row(
      mainAxisSize: MainAxisSize.min,
      children: [
        Icon(pozitivno ? Icons.thumb_up_alt : Icons.thumb_down_alt, size: 18, color: pozitivno ? Colors.green : Colors.red),
        const SizedBox(width: 4),
        Text(naziv),
      ],
    );
  }

  @override
  Widget build(BuildContext context) {
    return ListView(
      padding: const EdgeInsets.all(32),
      children: [
        Text('RECENZIJE', style: Theme.of(context).textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold)),
        const SizedBox(height: 12),
        Row(
          children: [
            SizedBox(
              width: 280,
              child: FutureBuilder<PagedResult<KorisnikOsnovno>>(
                future: _veterinari,
                builder: (context, s) => DropdownButtonFormField<int?>(
                  initialValue: _veterinarId,
                  decoration: const InputDecoration(labelText: 'Veterinar', isDense: true),
                  items: [
                    const DropdownMenuItem(value: null, child: Text('Svi veterinari')),
                    for (final v in s.data?.resultList ?? const <KorisnikOsnovno>[]) DropdownMenuItem(value: v.korisnikId, child: Text('${v.ime} ${v.prezime}')),
                  ],
                  onChanged: (v) {
                    _veterinarId = v;
                    _osvjezi(naPocetak: true);
                  },
                ),
              ),
            ),
            const SizedBox(width: 12),
            SizedBox(
              width: 240,
              child: DropdownButtonFormField<bool?>(
                initialValue: _pozitivna,
                decoration: const InputDecoration(labelText: 'Ocjena veterinara', isDense: true),
                items: const [
                  DropdownMenuItem(value: null, child: Text('Sve ocjene')),
                  DropdownMenuItem(value: true, child: Text('Pozitivne')),
                  DropdownMenuItem(value: false, child: Text('Negativne')),
                ],
                onChanged: (v) {
                  _pozitivna = v;
                  _osvjezi(naPocetak: true);
                },
              ),
            ),
          ],
        ),
        const SizedBox(height: 12),
        FutureBuilder<PagedResult<Recenzije>>(
          future: _recenzije,
          builder: (context, snapshot) {
            if (snapshot.hasError) return PorukaGreske(snapshot.error.toString(), onPokusajPonovo: _osvjezi);
            if (!snapshot.hasData) return const Center(child: CircularProgressIndicator());
            final rezultat = snapshot.data!;
            if (rezultat.resultList.isEmpty) return const Card(child: ListTile(title: Text('Nema recenzija za odabrane filtere.')));
            return Column(
              children: [
                for (final r in rezultat.resultList)
                  Card(
                    child: Padding(
                      padding: const EdgeInsets.all(16),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Row(
                            children: [
                              Expanded(
                                child: Text(
                                  '${r.korisnikIme} ${r.korisnikPrezime} → Dr. ${r.veterinarIme} ${r.veterinarPrezime}',
                                  style: const TextStyle(fontWeight: FontWeight.w600),
                                ),
                              ),
                              Text('${formatDatum(r.datumDodavanja)} • termin ${r.terminSifra}', style: const TextStyle(color: EvetBoje.tekstSivi)),
                            ],
                          ),
                          const SizedBox(height: 8),
                          Wrap(spacing: 24, children: [_palac('Usluga', r.uslugaPozitivna), _palac('Veterinar', r.veterinarPozitivan)]),
                          if (r.komentar != null) ...[const SizedBox(height: 8), Text('"${r.komentar}"')],
                          for (final o in r.odgovori)
                            Container(
                              margin: const EdgeInsets.only(top: 8, left: 24),
                              padding: const EdgeInsets.all(10),
                              decoration: BoxDecoration(color: EvetBoje.pozadina, borderRadius: BorderRadius.circular(8)),
                              child: Text('${o.korisnikIme} ${o.korisnikPrezime} (${formatDatum(o.datumDodavanja)}): ${o.komentar}'),
                            ),
                          Align(
                            alignment: Alignment.centerRight,
                            child: Row(
                              mainAxisSize: MainAxisSize.min,
                              children: [
                                TextButton.icon(onPressed: () => _odgovori(r), icon: const Icon(Icons.reply), label: const Text('Odgovori')),
                                TextButton.icon(
                                  onPressed: () => _obrisi(r),
                                  icon: const Icon(Icons.delete_outline),
                                  label: const Text('Obriši'),
                                  style: TextButton.styleFrom(foregroundColor: Theme.of(context).colorScheme.error),
                                ),
                              ],
                            ),
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
