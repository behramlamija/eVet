import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Pregled važećih promocija s aktiviranjem i lista vlastitih (aktiviranih) kupona.
class PromocijeScreen extends StatefulWidget {
  const PromocijeScreen({super.key});

  @override
  State<PromocijeScreen> createState() => _PromocijeScreenState();
}

class _PromocijeScreenState extends State<PromocijeScreen> {
  late Future<(PagedResult<Promocije>, PagedResult<AktiviranePromocije>)> _podaci;

  @override
  void initState() {
    super.initState();
    _ucitaj();
  }

  void _ucitaj() {
    final promocije = context.read<PromocijeProvider>();
    _podaci = (promocije.get(pageSize: BaseProvider.maxPageSize), promocije.moje(pageSize: BaseProvider.maxPageSize)).wait;
  }

  Future<void> _aktiviraj(Promocije p) async {
    final ok = await izvrsiAkciju(
      context,
      () => context.read<PromocijeProvider>().aktiviraj(p.promocijaId),
      uspjeh: 'Promocija "${p.naziv}" je aktivirana. Kod ${p.kod} možete iskoristiti u korpi.',
    );
    if (ok) setState(_ucitaj);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Promocije')),
      body: FutureBuilder(
        future: _podaci,
        builder: (context, snapshot) {
          if (snapshot.hasError) {
            return Padding(padding: const EdgeInsets.all(16), child: PorukaGreske(snapshot.error.toString(), onPokusajPonovo: () => setState(_ucitaj)));
          }
          if (!snapshot.hasData) return const Center(child: CircularProgressIndicator());
          final (promocije, moje) = snapshot.data!;
          final aktivirane = {for (final m in moje.resultList) m.promocijaId};

          return ListView(
            padding: const EdgeInsets.all(16),
            children: [
              Text('Aktuelne ponude', style: Theme.of(context).textTheme.titleLarge?.copyWith(fontWeight: FontWeight.bold)),
              const SizedBox(height: 8),
              if (promocije.resultList.isEmpty) const Text('Trenutno nema aktivnih promocija.'),
              for (final p in promocije.resultList)
                Card(
                  child: ListTile(
                    leading: CircleAvatar(backgroundColor: EvetBoje.crvena, child: Text('-${p.popust.round()}%', style: const TextStyle(color: Colors.white, fontSize: 11, fontWeight: FontWeight.bold))),
                    title: Text(p.naziv, style: const TextStyle(fontWeight: FontWeight.w600)),
                    subtitle: Text('${p.uslugaNaziv} • važi do ${formatDatum(p.datumKraja)}${p.opis == null ? '' : '\n${p.opis}'}'),
                    isThreeLine: p.opis != null,
                    trailing: aktivirane.contains(p.promocijaId)
                        ? const Tooltip(message: 'Promocija je već aktivirana', child: Icon(Icons.check_circle, color: Colors.green))
                        : TextButton(onPressed: () => _aktiviraj(p), child: const Text('Aktiviraj')),
                  ),
                ),
              const SizedBox(height: 24),
              Text('Moji kuponi', style: Theme.of(context).textTheme.titleLarge?.copyWith(fontWeight: FontWeight.bold)),
              const SizedBox(height: 8),
              if (moje.resultList.isEmpty) const Text('Nemate aktiviranih kupona.'),
              for (final k in moje.resultList)
                Card(
                  child: ListTile(
                    leading: Icon(Icons.local_offer, color: k.iskoristena ? Colors.grey : EvetBoje.crvena),
                    title: Text('${k.promocijaKod} • -${k.promocijaPopust.round()}%'),
                    subtitle: Text('${k.uslugaNaziv} • važi do ${formatDatum(k.promocijaDatumKraja)}'),
                    trailing: Text(k.iskoristena ? 'Iskorišten' : 'Dostupan', style: TextStyle(color: k.iskoristena ? Colors.grey : Colors.green.shade700)),
                  ),
                ),
            ],
          );
        },
      ),
    );
  }
}
