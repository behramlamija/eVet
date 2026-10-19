import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Sistemske notifikacije; lista se osvježava kad polling javi promjenu broja nepročitanih.
class ObavijestiScreen extends StatefulWidget {
  const ObavijestiScreen({super.key});

  @override
  State<ObavijestiScreen> createState() => _ObavijestiScreenState();
}

class _ObavijestiScreenState extends State<ObavijestiScreen> {
  late Future<PagedResult<Obavijesti>> _obavijesti;
  int? _zadnjiBroj;

  void _ucitaj() {
    _obavijesti = context.read<ObavijestiProvider>().get();
  }

  Future<void> _oznaci(Obavijesti obavijest) async {
    if (obavijest.jePogledana) return;
    await context.read<ObavijestiProvider>().oznaciProcitano(obavijest.obavijestId);
  }

  @override
  Widget build(BuildContext context) {
    final provider = context.watch<ObavijestiProvider>();
    if (_zadnjiBroj != provider.brojNeprocitanih) {
      _zadnjiBroj = provider.brojNeprocitanih;
      _ucitaj();
    }

    return FutureBuilder<PagedResult<Obavijesti>>(
      future: _obavijesti,
      builder: (context, snapshot) {
        if (snapshot.hasError) {
          return Padding(padding: const EdgeInsets.all(16), child: PorukaGreske(snapshot.error.toString(), onPokusajPonovo: () => setState(_ucitaj)));
        }
        if (!snapshot.hasData) {
          return const Center(child: CircularProgressIndicator());
        }
        final obavijesti = snapshot.data!.resultList;
        return ListView(
          padding: const EdgeInsets.all(16),
          children: [
            Row(
              children: [
                Expanded(child: Text('Obavijesti', style: Theme.of(context).textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold))),
                TextButton(
                  onPressed: provider.brojNeprocitanih == 0 ? null : provider.oznaciSveProcitano,
                  child: const Text('Označi sve pročitano'),
                ),
              ],
            ),
            if (obavijesti.isEmpty) const Center(child: Padding(padding: EdgeInsets.all(24), child: Text('Nemate obavijesti.'))),
            for (final o in obavijesti)
              Card(
                child: ListTile(
                  leading: Icon(o.jePogledana ? Icons.drafts_outlined : Icons.mark_email_unread_outlined, color: o.jePogledana ? null : EvetBoje.crvena),
                  title: Text(o.naslov, style: TextStyle(fontWeight: o.jePogledana ? FontWeight.normal : FontWeight.bold)),
                  subtitle: Text('${o.sadrzaj}\n${formatDatumVrijeme(o.datumObavijesti)}'),
                  isThreeLine: true,
                  onTap: () => _oznaci(o),
                ),
              ),
          ],
        );
      },
    );
  }
}
