import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Lista sistemskih notifikacija prijavljenog korisnika, s označavanjem pročitanih.
Future<void> prikaziObavijesti(BuildContext context) {
  return showDialog<void>(context: context, builder: (_) => const _ObavijestiDijalog());
}

class _ObavijestiDijalog extends StatefulWidget {
  const _ObavijestiDijalog();

  @override
  State<_ObavijestiDijalog> createState() => _ObavijestiDijalogState();
}

class _ObavijestiDijalogState extends State<_ObavijestiDijalog> {
  static const _velicinaStranice = 10;

  int _stranica = 0;
  late Future<PagedResult<Obavijesti>> _obavijesti;

  @override
  void initState() {
    super.initState();
    _ucitaj();
  }

  void _ucitaj() => _obavijesti = context.read<ObavijestiProvider>().get(page: _stranica, pageSize: _velicinaStranice);

  Future<void> _oznaci(Obavijesti o) async {
    if (o.jePogledana) return;
    final provider = context.read<ObavijestiProvider>();
    try {
      await provider.oznaciProcitano(o.obavijestId);
      if (mounted) setState(_ucitaj);
    } on ApiException catch (e) {
      if (mounted) prikaziGresku(context, e.poruka);
    }
  }

  Future<void> _oznaciSve() async {
    final provider = context.read<ObavijestiProvider>();
    if (await izvrsiAkciju(context, provider.oznaciSveProcitano, uspjeh: 'Sve obavijesti su označene kao pročitane.') && mounted) {
      setState(_ucitaj);
    }
  }

  @override
  Widget build(BuildContext context) {
    final brojNeprocitanih = context.watch<ObavijestiProvider>().brojNeprocitanih;

    return FormaDijalog(
      naslov: 'Obavijesti',
      sirina: 620,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Align(
            alignment: Alignment.centerRight,
            child: Tooltip(
              message: brojNeprocitanih == 0 ? 'Nema nepročitanih obavijesti.' : '',
              child: TextButton.icon(
                onPressed: brojNeprocitanih == 0 ? null : _oznaciSve,
                icon: const Icon(Icons.done_all),
                label: const Text('Označi sve pročitano'),
              ),
            ),
          ),
          FutureBuilder<PagedResult<Obavijesti>>(
            future: _obavijesti,
            builder: (context, snapshot) {
              if (snapshot.hasError) return PorukaGreske(snapshot.error.toString(), onPokusajPonovo: () => setState(_ucitaj));
              if (!snapshot.hasData) return const Padding(padding: EdgeInsets.all(24), child: Center(child: CircularProgressIndicator()));
              final rezultat = snapshot.data!;
              if (rezultat.resultList.isEmpty) return const Padding(padding: EdgeInsets.all(24), child: Center(child: Text('Nemate obavijesti.')));
              return Column(
                children: [
                  for (final o in rezultat.resultList)
                    Card(
                      color: o.jePogledana ? null : EvetBoje.pozadina,
                      child: ListTile(
                        onTap: () => _oznaci(o),
                        leading: Icon(o.jePogledana ? Icons.drafts_outlined : Icons.mark_email_unread_outlined, color: o.jePogledana ? null : EvetBoje.crvena),
                        title: Text(o.naslov, style: TextStyle(fontWeight: o.jePogledana ? FontWeight.normal : FontWeight.bold)),
                        subtitle: Text('${o.sadrzaj}\n${formatDatumVrijeme(o.datumObavijesti)}'),
                        isThreeLine: true,
                        trailing: o.jePogledana ? null : const Tooltip(message: 'Kliknite da označite kao pročitano', child: Icon(Icons.circle, size: 10, color: EvetBoje.crvena)),
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
      ),
    );
  }
}
