import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'obaveze_panel.dart';
import 'termin_detalji_dijalog.dart';

/// Zakazani termini veterinara po danima, s akcijama nad statusom (mockup sa slike 5 u prijavi).
class TerminiScreen extends StatefulWidget {
  const TerminiScreen({super.key});

  @override
  State<TerminiScreen> createState() => _TerminiScreenState();
}

class _TerminiScreenState extends State<TerminiScreen> {
  DateTime _datum = DateUtils.dateOnly(DateTime.now());
  StatusTermina? _status;
  late Future<(List<Termini>, int)> _podaci;

  @override
  void initState() {
    super.initState();
    _ucitaj();
  }

  void _ucitaj() {
    final termini = context.read<TerminiProvider>();
    _podaci = Future.wait([
      termini.get(filter: {'DatumOd': _datum, 'DatumDo': _datum, 'Status': _status}, pageSize: BaseProvider.maxPageSize),
      termini.get(filter: {'Status': StatusTermina.naCekanju}, pageSize: 1),
    ]).then((r) {
      final lista = [...r[0].resultList]..sort((a, b) => a.vrijemePocetka.compareTo(b.vrijemePocetka));
      return (lista, r[1].count);
    });
  }

  void _promijeniDatum(DateTime datum) => setState(() {
        _datum = DateUtils.dateOnly(datum);
        _ucitaj();
      });

  Future<void> _odaberiDatum() async {
    final odabran = await showDatePicker(
      context: context,
      initialDate: _datum,
      firstDate: DateTime(2020),
      lastDate: DateTime.now().add(const Duration(days: 365)),
    );
    if (odabran != null) _promijeniDatum(odabran);
  }

  Future<void> _akcija(Future<void> Function() akcija, String uspjeh) async {
    if (await izvrsiAkciju(context, akcija, uspjeh: uspjeh)) setState(_ucitaj);
  }

  Future<void> _potvrdi(Termini t) async {
    final ok = await potvrdiAkciju(context, naslov: 'Potvrda termina', poruka: 'Potvrditi termin ${t.sifra} za ljubimca ${t.ljubimacIme}?');
    if (ok && mounted) await _akcija(() => context.read<TerminiProvider>().potvrdi(t.terminId), 'Termin ${t.sifra} je potvrđen.');
  }

  Future<void> _zavrsi(Termini t) async {
    final ok = await potvrdiAkciju(context, naslov: 'Završavanje termina', poruka: 'Označiti termin ${t.sifra} kao završen?');
    if (ok && mounted) await _akcija(() => context.read<TerminiProvider>().zavrsi(t.terminId), 'Termin ${t.sifra} je označen kao završen.');
  }

  Future<void> _odbij(Termini t) async {
    final razlog = await unesiRazlog(context, naslov: 'Odbijanje termina ${t.sifra}', potvrda: 'Odbij termin');
    if (razlog != null && mounted) {
      await _akcija(() => context.read<TerminiProvider>().odbij(t.terminId, razlog), 'Termin ${t.sifra} je odbijen. Klijent je obaviješten.');
    }
  }

  Future<void> _otkazi(Termini t) async {
    final razlog = await unesiRazlog(context, naslov: 'Otkazivanje termina ${t.sifra}', potvrda: 'Otkaži termin');
    if (razlog != null && mounted) {
      await _akcija(() => context.read<TerminiProvider>().otkazi(t.terminId, razlog), 'Termin ${t.sifra} je otkazan. Klijent je obaviješten.');
    }
  }

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(32),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text('ZAKAZANI TERMINI', style: Theme.of(context).textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold)),
                const SizedBox(height: 8),
                Row(
                  children: [
                    IconButton(tooltip: 'Prethodni dan', onPressed: () => _promijeniDatum(_datum.subtract(const Duration(days: 1))), icon: const Icon(Icons.chevron_left)),
                    TextButton.icon(onPressed: _odaberiDatum, icon: const Icon(Icons.calendar_today_outlined, size: 18), label: Text(formatDatum(_datum))),
                    IconButton(tooltip: 'Sljedeći dan', onPressed: () => _promijeniDatum(_datum.add(const Duration(days: 1))), icon: const Icon(Icons.chevron_right)),
                    TextButton(onPressed: () => _promijeniDatum(DateTime.now()), child: const Text('Danas')),
                    const Spacer(),
                    SizedBox(
                      width: 200,
                      child: DropdownButtonFormField<StatusTermina?>(
                        initialValue: _status,
                        decoration: const InputDecoration(labelText: 'Status', isDense: true),
                        items: [
                          const DropdownMenuItem(value: null, child: Text('Svi statusi')),
                          for (final s in StatusTermina.values) DropdownMenuItem(value: s, child: Text(nazivStatusa(s))),
                        ],
                        onChanged: (s) => setState(() {
                          _status = s;
                          _ucitaj();
                        }),
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 12),
                Expanded(
                  child: FutureBuilder<(List<Termini>, int)>(
                    future: _podaci,
                    builder: (context, snapshot) {
                      if (snapshot.hasError) {
                        return PorukaGreske(snapshot.error.toString(), onPokusajPonovo: () => setState(_ucitaj));
                      }
                      if (!snapshot.hasData) {
                        return const Center(child: CircularProgressIndicator());
                      }
                      final (termini, _) = snapshot.data!;
                      return Card(
                        child: termini.isEmpty
                            ? const Center(child: Text('Nema termina za odabrani dan i status.'))
                            : ListView.separated(
                                itemCount: termini.length,
                                separatorBuilder: (_, _) => const Divider(height: 1),
                                itemBuilder: (_, i) => _TerminRed(
                                  termin: termini[i],
                                  onDetalji: () => prikaziDetaljeTermina(context, termini[i]),
                                  onPotvrdi: () => _potvrdi(termini[i]),
                                  onOdbij: () => _odbij(termini[i]),
                                  onZavrsi: () => _zavrsi(termini[i]),
                                  onOtkazi: () => _otkazi(termini[i]),
                                ),
                              ),
                      );
                    },
                  ),
                ),
              ],
            ),
          ),
          const SizedBox(width: 24),
          SizedBox(
            width: 320,
            child: Column(
              children: [
                FutureBuilder<(List<Termini>, int)>(
                  future: _podaci,
                  builder: (context, snapshot) {
                    final (termini, brojNaCekanju) = snapshot.data ?? (const <Termini>[], 0);
                    return Card(
                      child: Column(
                        children: [
                          ListTile(
                            leading: const Icon(Icons.event_available_outlined),
                            title: Text(DateUtils.isSameDay(_datum, DateTime.now()) ? 'Danas' : formatDatum(_datum)),
                            subtitle: Text('${termini.length} termina'),
                          ),
                          const Divider(height: 1),
                          ListTile(
                            leading: const Icon(Icons.mark_chat_unread_outlined),
                            title: Text('$brojNaCekanju neodgovorenih zahtjeva'),
                            trailing: brojNaCekanju > 0
                                ? TextButton(
                                    onPressed: () => setState(() {
                                      _status = StatusTermina.naCekanju;
                                      _ucitaj();
                                    }),
                                    child: const Text('Prikaži'),
                                  )
                                : null,
                          ),
                        ],
                      ),
                    );
                  },
                ),
                const SizedBox(height: 16),
                Expanded(child: ObavezePanel(datum: _datum)),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

class _TerminRed extends StatelessWidget {
  const _TerminRed({
    required this.termin,
    required this.onDetalji,
    required this.onPotvrdi,
    required this.onOdbij,
    required this.onZavrsi,
    required this.onOtkazi,
  });

  final Termini termin;
  final VoidCallback onDetalji;
  final VoidCallback onPotvrdi;
  final VoidCallback onOdbij;
  final VoidCallback onZavrsi;
  final VoidCallback onOtkazi;

  bool get _zapocet {
    final dijelovi = termin.vrijemePocetka.split(':').map(int.parse).toList();
    final pocetak = DateTime(termin.datumTermina.year, termin.datumTermina.month, termin.datumTermina.day, dijelovi[0], dijelovi[1]);
    return !pocetak.isAfter(DateTime.now());
  }

  @override
  Widget build(BuildContext context) {
    return ListTile(
      onTap: onDetalji,
      contentPadding: const EdgeInsets.symmetric(horizontal: 20, vertical: 8),
      leading: Text(formatVrijeme(termin.vrijemePocetka), style: const TextStyle(fontSize: 18, fontWeight: FontWeight.w600)),
      title: Text(termin.stavke.map((s) => s.uslugaNaziv).join(', ')),
      subtitle: Text('${termin.ljubimacIme} • ${termin.korisnikIme} ${termin.korisnikPrezime}'),
      trailing: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          StatusTerminaChip(termin.status),
          const SizedBox(width: 8),
          if (termin.status == StatusTermina.naCekanju) ...[
            IconButton(tooltip: 'Potvrdi termin', onPressed: onPotvrdi, icon: const Icon(Icons.check_circle_outline, color: Colors.green)),
            IconButton(tooltip: 'Odbij termin', onPressed: onOdbij, icon: const Icon(Icons.cancel_outlined, color: Colors.red)),
          ],
          if (termin.status == StatusTermina.potvrdjen) ...[
            Tooltip(
              message: _zapocet ? 'Označi kao završen' : 'Termin je moguće završiti tek nakon njegovog početka.',
              child: IconButton(onPressed: _zapocet ? onZavrsi : null, icon: const Icon(Icons.task_alt)),
            ),
            IconButton(tooltip: 'Otkaži termin', onPressed: onOtkazi, icon: const Icon(Icons.event_busy_outlined, color: Colors.red)),
          ],
        ],
      ),
    );
  }
}
