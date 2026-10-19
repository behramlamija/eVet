import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'terapija_forma.dart';

/// Tabela terapija (mockup sa slike 7). Izmjena i brisanje su dostupni samo veterinaru koji je propisao terapiju.
class TerapijeTabela extends StatelessWidget {
  const TerapijeTabela({super.key, required this.terapije, required this.onPromjena, this.prikaziPacijenta = true});

  final List<Terapije> terapije;
  final VoidCallback onPromjena;
  final bool prikaziPacijenta;

  Future<void> _uredi(BuildContext context, Terapije t) async {
    if (await otvoriFormuTerapije(context, ljubimacId: t.ljubimacId, ljubimacIme: t.ljubimacIme, terapija: t)) onPromjena();
  }

  Future<void> _obrisi(BuildContext context, Terapije t) async {
    final potvrda = await potvrdiAkciju(context, naslov: 'Brisanje terapije', poruka: 'Obrisati terapiju "${t.dijagnoza}" za ${t.ljubimacIme}?', potvrda: 'Obriši', opasna: true);
    if (!potvrda || !context.mounted) return;
    if (await izvrsiAkciju(context, () => context.read<TerapijeProvider>().delete(t.terapijaId), uspjeh: 'Terapija je obrisana.')) onPromjena();
  }

  @override
  Widget build(BuildContext context) {
    if (terapije.isEmpty) {
      return const Card(child: ListTile(title: Text('Nema evidentiranih terapija.')));
    }

    final mojId = context.read<AuthProvider>().korisnik!.korisnikId;
    const nijeAutor = 'Terapiju može mijenjati samo veterinar koji ju je propisao.';

    return Card(
      child: SizedBox(
        width: double.infinity,
        child: DataTable(
          columns: [
            const DataColumn(label: Text('Datum')),
            if (prikaziPacijenta) const DataColumn(label: Text('Pacijent')),
            const DataColumn(label: Text('Terapija')),
            const DataColumn(label: Text('Doza')),
            const DataColumn(label: Text('Trajanje')),
            const DataColumn(label: Text('Veterinar')),
            const DataColumn(label: Text('Status')),
            const DataColumn(label: Text('')),
          ],
          rows: [
            for (final t in terapije)
              DataRow(cells: [
                DataCell(Text(formatDatum(t.datumTerapije))),
                if (prikaziPacijenta) DataCell(Text(t.ljubimacIme)),
                DataCell(Text(t.lijek == null ? t.dijagnoza : '${t.dijagnoza}\n(${t.lijek})')),
                DataCell(Text(t.doza ?? '-')),
                DataCell(Text(t.trajanjeDana == null ? '-' : '${t.trajanjeDana} dana')),
                DataCell(Text('Dr. ${t.veterinarPrezime}')),
                DataCell(Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Icon(t.jeZavrsena ? Icons.check_circle : Icons.schedule, size: 18, color: t.jeZavrsena ? Colors.green : Colors.orange),
                    const SizedBox(width: 4),
                    Text(t.jeZavrsena ? 'Završeno' : 'U toku'),
                  ],
                )),
                DataCell(Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    IconButton(
                      tooltip: t.veterinarId == mojId ? 'Izmijeni terapiju' : nijeAutor,
                      onPressed: t.veterinarId == mojId ? () => _uredi(context, t) : null,
                      icon: const Icon(Icons.edit_outlined),
                    ),
                    IconButton(
                      tooltip: t.veterinarId == mojId ? 'Obriši terapiju' : nijeAutor,
                      onPressed: t.veterinarId == mojId ? () => _obrisi(context, t) : null,
                      icon: const Icon(Icons.delete_outline),
                    ),
                  ],
                )),
              ]),
          ],
        ),
      ),
    );
  }
}
