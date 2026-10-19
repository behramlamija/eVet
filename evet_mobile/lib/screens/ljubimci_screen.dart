import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'ljubimac_forma_screen.dart';

class LjubimciScreen extends StatefulWidget {
  const LjubimciScreen({super.key});

  @override
  State<LjubimciScreen> createState() => _LjubimciScreenState();
}

class _LjubimciScreenState extends State<LjubimciScreen> {
  late Future<PagedResult<Ljubimci>> _ljubimci;
  int _verzijaSlika = 0;

  @override
  void initState() {
    super.initState();
    _ucitaj();
  }

  void _ucitaj() => _ljubimci = context.read<LjubimciProvider>().get(pageSize: BaseProvider.maxPageSize);

  Future<void> _otvori([Ljubimci? ljubimac]) async {
    final promjena = await Navigator.of(context).push<bool>(MaterialPageRoute(builder: (_) => LjubimacFormaScreen(ljubimac: ljubimac)));
    if (promjena == true) {
      setState(() {
        _verzijaSlika++;
        _ucitaj();
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    final slike = context.read<SlikeProvider>();

    return Scaffold(
      appBar: AppBar(title: const Text('Moji ljubimci')),
      floatingActionButton: FloatingActionButton.extended(onPressed: () => _otvori(), icon: const Icon(Icons.add), label: const Text('Dodaj ljubimca')),
      body: FutureBuilder<PagedResult<Ljubimci>>(
        future: _ljubimci,
        builder: (context, snapshot) {
          if (snapshot.hasError) {
            return Padding(padding: const EdgeInsets.all(16), child: PorukaGreske(snapshot.error.toString(), onPokusajPonovo: () => setState(_ucitaj)));
          }
          if (!snapshot.hasData) return const Center(child: CircularProgressIndicator());
          final ljubimci = snapshot.data!.resultList;
          if (ljubimci.isEmpty) return const Center(child: Text('Još niste dodali nijednog ljubimca.'));
          return ListView(
            padding: const EdgeInsets.fromLTRB(16, 16, 16, 96),
            children: [
              for (final l in ljubimci)
                Card(
                  child: ListTile(
                    onTap: () => _otvori(l),
                    leading: EvetSlika(slike: slike, tip: TipSlike.ljubimac, id: l.ljubimacId, imaSliku: l.imaSliku, verzija: _verzijaSlika),
                    title: Text(l.ime, style: const TextStyle(fontWeight: FontWeight.w600)),
                    subtitle: Text('${l.vrstaLjubimcaNaziv} • ${l.rasaNaziv} • ${nazivSpola(l.spol)}'),
                    trailing: const Icon(Icons.chevron_right),
                  ),
                ),
            ],
          );
        },
      ),
    );
  }
}
