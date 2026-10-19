import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../providers/korpa_provider.dart';
import '../screens/usluga_detalji_screen.dart';

/// Sekcija "Korisnici također rezervišu": usluge koje se najčešće rezervišu zajedno s [uslugeIds],
/// uz objašnjenje preporuke i dodavanje u korpu jednim klikom.
class PreporukeSekcija extends StatefulWidget {
  const PreporukeSekcija({super.key, required this.uslugeIds, this.sakrijAkoPrazno = false});

  final List<int> uslugeIds;

  /// U korpi se sekcija ne prikazuje kad nema preporuka; na detaljima usluge se prikazuje poruka.
  final bool sakrijAkoPrazno;

  @override
  State<PreporukeSekcija> createState() => _PreporukeSekcijaState();
}

class _PreporukeSekcijaState extends State<PreporukeSekcija> {
  late Future<List<PreporukeUsluga>> _preporuke;

  @override
  void initState() {
    super.initState();
    _ucitaj();
  }

  @override
  void didUpdateWidget(PreporukeSekcija oldWidget) {
    super.didUpdateWidget(oldWidget);
    if (!_isteUsluge(oldWidget.uslugeIds, widget.uslugeIds)) _ucitaj();
  }

  bool _isteUsluge(List<int> a, List<int> b) => a.length == b.length && a.toSet().containsAll(b);

  void _ucitaj() => _preporuke = context.read<PreporukeProvider>().zaUsluge(widget.uslugeIds);

  @override
  Widget build(BuildContext context) {
    final korpa = context.watch<KorpaProvider>();
    final slike = context.read<SlikeProvider>();

    return FutureBuilder<List<PreporukeUsluga>>(
      future: _preporuke,
      builder: (context, snapshot) {
        final Widget sadrzaj;
        if (snapshot.hasError) {
          sadrzaj = PorukaGreske(snapshot.error.toString());
        } else if (!snapshot.hasData) {
          sadrzaj = const Padding(padding: EdgeInsets.all(16), child: Center(child: CircularProgressIndicator()));
        } else if (snapshot.data!.isEmpty) {
          if (widget.sakrijAkoPrazno) return const SizedBox.shrink();
          sadrzaj = const Text('Još nema dovoljno rezervacija da bismo preporučili usluge koje se rezervišu uz ovu.');
        } else {
          sadrzaj = Column(
            children: [
              for (final p in snapshot.data!)
                Card(
                  child: Padding(
                    padding: const EdgeInsets.symmetric(vertical: 4),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        ListTile(
                          leading: EvetSlika(slike: slike, tip: TipSlike.usluga, id: p.usluga.uslugaId, imaSliku: p.usluga.imaSliku, zamjenskaIkona: Icons.medical_services_outlined),
                          title: Text(p.usluga.naziv),
                          subtitle: Text(formatNovac(p.usluga.cijena), style: const TextStyle(fontWeight: FontWeight.bold)),
                          onTap: () => Navigator.of(context).push(MaterialPageRoute(builder: (_) => UslugaDetaljiScreen(usluga: p.usluga))),
                          trailing: korpa.sadrzi(p.usluga.uslugaId)
                              ? const Icon(Icons.check, color: EvetBoje.tamnoplava)
                              : IconButton(
                                  tooltip: 'Dodaj u korpu',
                                  icon: const Icon(Icons.add_shopping_cart),
                                  onPressed: () {
                                    korpa.dodaj(p.usluga);
                                    prikaziUspjeh(context, 'Usluga "${p.usluga.naziv}" je dodana u korpu.');
                                  },
                                ),
                        ),
                        Padding(
                          padding: const EdgeInsets.fromLTRB(16, 0, 16, 8),
                          child: Row(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              const Icon(Icons.lightbulb_outline, size: 18, color: EvetBoje.tamnoplava),
                              const SizedBox(width: 8),
                              Expanded(child: Text(p.objasnjenje, style: Theme.of(context).textTheme.bodySmall)),
                            ],
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
            ],
          );
        }

        return Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            Text('Korisnici također rezervišu', style: Theme.of(context).textTheme.titleMedium?.copyWith(fontWeight: FontWeight.bold)),
            const SizedBox(height: 8),
            sadrzaj,
          ],
        );
      },
    );
  }
}
