import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Historija termina klijenta; najnoviji termini su na vrhu.
class TerminiScreen extends StatefulWidget {
  const TerminiScreen({super.key});

  @override
  State<TerminiScreen> createState() => _TerminiScreenState();
}

class _TerminiScreenState extends State<TerminiScreen> {
  late Future<PagedResult<Termini>> _termini;

  @override
  void initState() {
    super.initState();
    _ucitaj();
  }

  void _ucitaj() => _termini = context.read<TerminiProvider>().get();

  @override
  Widget build(BuildContext context) {
    return RefreshIndicator(
      onRefresh: () async {
        setState(_ucitaj);
        await _termini;
      },
      child: FutureBuilder<PagedResult<Termini>>(
        future: _termini,
        builder: (context, snapshot) {
          if (snapshot.hasError) {
            return ListView(padding: const EdgeInsets.all(16), children: [PorukaGreske(snapshot.error.toString(), onPokusajPonovo: () => setState(_ucitaj))]);
          }
          if (!snapshot.hasData) {
            return const Center(child: CircularProgressIndicator());
          }
          final termini = snapshot.data!.resultList;
          return ListView(
            padding: const EdgeInsets.all(16),
            children: [
              Text('Moji termini', style: Theme.of(context).textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold)),
              const SizedBox(height: 12),
              if (termini.isEmpty) const Center(child: Padding(padding: EdgeInsets.all(24), child: Text('Još nemate zakazanih termina.'))),
              for (final t in termini)
                Card(
                  child: ListTile(
                    title: Text(t.stavke.map((s) => s.uslugaNaziv).join(', ')),
                    subtitle: Text('${formatDatum(t.datumTermina)} u ${formatVrijeme(t.vrijemePocetka)} • ${t.ljubimacIme}\n${formatNovac(t.ukupnaCijena)}'),
                    isThreeLine: true,
                    trailing: Chip(label: Text(nazivStatusa(t.status))),
                  ),
                ),
            ],
          );
        },
      ),
    );
  }
}
