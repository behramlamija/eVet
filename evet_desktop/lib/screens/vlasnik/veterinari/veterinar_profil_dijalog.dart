import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'veterinar_forma.dart';

/// Profil veterinara (mockup sa slike 3), s uređivanjem i brisanjem.
Future<void> prikaziProfilVeterinara(BuildContext context, int korisnikId) {
  return showDialog<void>(context: context, builder: (_) => _ProfilVeterinara(korisnikId: korisnikId));
}

class _ProfilVeterinara extends StatefulWidget {
  const _ProfilVeterinara({required this.korisnikId});

  final int korisnikId;

  @override
  State<_ProfilVeterinara> createState() => _ProfilVeterinaraState();
}

class _ProfilVeterinaraState extends State<_ProfilVeterinara> {
  late Future<Korisnici> _veterinar;
  int _verzijaSlike = 0;

  @override
  void initState() {
    super.initState();
    _ucitaj();
  }

  void _ucitaj() => _veterinar = context.read<KorisniciProvider>().getById(widget.korisnikId);

  Future<void> _uredi(Korisnici v) async {
    if (await otvoriFormuVeterinara(context, veterinar: v)) {
      setState(() {
        _verzijaSlike++;
        _ucitaj();
      });
    }
  }

  Future<void> _izbrisi(Korisnici v) async {
    final potvrda = await potvrdiAkciju(context, naslov: 'Brisanje veterinara', poruka: 'Izbrisati veterinara ${v.ime} ${v.prezime}?', potvrda: 'Izbriši', opasna: true);
    if (!potvrda || !mounted) return;
    if (await izvrsiAkciju(context, () => context.read<KorisniciProvider>().delete(v.korisnikId), uspjeh: 'Veterinar ${v.ime} ${v.prezime} je izbrisan.') && mounted) {
      Navigator.pop(context);
    }
  }

  @override
  Widget build(BuildContext context) {
    return FormaDijalog(
        naslov: 'Profil veterinara',
        sirina: 760,
        child: FutureBuilder<Korisnici>(
          future: _veterinar,
          builder: (context, snapshot) {
            if (snapshot.hasError) return PorukaGreske(snapshot.error.toString(), onPokusajPonovo: () => setState(_ucitaj));
            if (!snapshot.hasData) return const Center(child: CircularProgressIndicator());
            final v = snapshot.data!;

            final redovi = <(String, String)>[
              ('Datum rođenja', formatDatum(v.datumRodjenja)),
              ('Mjesto rođenja', v.gradRodjenjaNaziv ?? '-'),
              ('Email', v.email),
              ('Broj telefona', v.telefon ?? '-'),
              ('Datum zapošljavanja', formatDatum(v.datumZaposlenja)),
              ('Status', v.jeAktivan ? 'Aktivan' : 'Neaktivan'),
              ('Zvanje', v.zvanje ?? '-'),
              ('Specijalnost', v.specijalnostNaziv ?? '-'),
              ('Godine iskustva', v.godineIskustva?.toString() ?? '-'),
            ];

            return Row(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                SizedBox(
                  width: 220,
                  child: Column(
                    children: [
                      EvetSlika(
                        slike: context.read<SlikeProvider>(),
                        tip: TipSlike.korisnik,
                        id: v.korisnikId,
                        imaSliku: v.imaSliku,
                        velicina: 150,
                        zamjenskaIkona: Icons.person,
                        verzija: _verzijaSlike,
                      ),
                      const SizedBox(height: 12),
                      Text('${v.ime} ${v.prezime}', style: Theme.of(context).textTheme.titleLarge?.copyWith(fontWeight: FontWeight.bold), textAlign: TextAlign.center),
                      const SizedBox(height: 16),
                      FilledButton.icon(onPressed: () => _uredi(v), icon: const Icon(Icons.edit_outlined), label: const Text('Uredi veterinara')),
                      const SizedBox(height: 8),
                      OutlinedButton.icon(
                        onPressed: () => _izbrisi(v),
                        icon: const Icon(Icons.delete_outline),
                        label: const Text('Izbriši veterinara'),
                        style: OutlinedButton.styleFrom(foregroundColor: Theme.of(context).colorScheme.error),
                      ),
                    ],
                  ),
                ),
                const SizedBox(width: 24),
                Expanded(
                  child: Table(
                    border: TableBorder.all(color: Colors.grey.shade300),
                    columnWidths: const {0: FlexColumnWidth(2), 1: FlexColumnWidth(3)},
                    children: [
                      for (final (naziv, vrijednost) in redovi)
                        TableRow(children: [
                          Padding(padding: const EdgeInsets.all(12), child: Text(naziv, style: const TextStyle(fontWeight: FontWeight.w600))),
                          Padding(padding: const EdgeInsets.all(12), child: Text(vrijednost)),
                        ]),
                    ],
                  ),
                ),
              ],
            );
          },
        ),
    );
  }
}
