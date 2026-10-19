import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'recenzija_screen.dart';

/// Detalji termina klijenta (master-detail) s otkazivanjem i ostavljanjem recenzije.
class TerminDetaljiScreen extends StatefulWidget {
  const TerminDetaljiScreen({super.key, required this.terminId});

  final int terminId;

  @override
  State<TerminDetaljiScreen> createState() => _TerminDetaljiScreenState();
}

class _TerminDetaljiScreenState extends State<TerminDetaljiScreen> {
  late Future<(Termini, TerminiKonfiguracija)> _podaci;
  bool _promijenjen = false;

  @override
  void initState() {
    super.initState();
    _ucitaj();
  }

  void _ucitaj() {
    final termini = context.read<TerminiProvider>();
    _podaci = (termini.getById(widget.terminId), termini.konfiguracija()).wait;
  }

  DateTime _pocetak(Termini t) {
    final d = t.vrijemePocetka.split(':').map(int.parse).toList();
    return DateTime(t.datumTermina.year, t.datumTermina.month, t.datumTermina.day, d[0], d[1]);
  }

  Future<void> _otkazi(Termini t) async {
    final razlog = await unesiRazlog(context, naslov: 'Otkazivanje termina ${t.sifra}', potvrda: 'Otkaži termin');
    if (razlog == null || !mounted) return;
    final ok = await izvrsiAkciju(context, () => context.read<TerminiProvider>().otkazi(t.terminId, razlog), uspjeh: 'Termin ${t.sifra} je otkazan.');
    if (ok) {
      setState(() {
        _promijenjen = true;
        _ucitaj();
      });
    }
  }

  Future<void> _recenzija(Termini t) async {
    final ostavljena = await Navigator.of(context).push<bool>(MaterialPageRoute(builder: (_) => RecenzijaScreen(termin: t)));
    if (ostavljena == true) {
      setState(() {
        _promijenjen = true;
        _ucitaj();
      });
    }
  }

  Widget _red(IconData ikona, String naziv, String vrijednost) => ListTile(
        dense: true,
        contentPadding: EdgeInsets.zero,
        leading: Icon(ikona, color: EvetBoje.tamnoplava),
        title: Text(naziv, style: const TextStyle(color: EvetBoje.tekstSivi)),
        subtitle: Text(vrijednost, style: const TextStyle(fontSize: 15, color: Colors.black87)),
      );

  @override
  Widget build(BuildContext context) {
    return PopScope(
      canPop: false,
      onPopInvokedWithResult: (popped, _) {
        if (!popped) Navigator.pop(context, _promijenjen);
      },
      child: Scaffold(
        appBar: AppBar(title: const Text('Detalji termina')),
        body: FutureBuilder<(Termini, TerminiKonfiguracija)>(
          future: _podaci,
          builder: (context, snapshot) {
            if (snapshot.hasError) {
              return Padding(padding: const EdgeInsets.all(16), child: PorukaGreske(snapshot.error.toString(), onPokusajPonovo: () => setState(_ucitaj)));
            }
            if (!snapshot.hasData) return const Center(child: CircularProgressIndicator());
            final (t, k) = snapshot.data!;

            final aktivan = t.status == StatusTermina.naCekanju || t.status == StatusTermina.potvrdjen;
            final dovoljnoRano = _pocetak(t).difference(DateTime.now()) >= Duration(hours: k.minimalnoSatiZaOtkazivanje);

            return ListView(
              padding: const EdgeInsets.all(16),
              children: [
                Row(
                  children: [
                    Expanded(child: Text('Termin ${t.sifra}', style: Theme.of(context).textTheme.titleLarge?.copyWith(fontWeight: FontWeight.bold))),
                    StatusTerminaChip(t.status),
                  ],
                ),
                const SizedBox(height: 8),
                _red(Icons.calendar_today_outlined, 'Datum i vrijeme', '${formatDatum(t.datumTermina)}, ${formatVrijeme(t.vrijemePocetka)} - ${formatVrijeme(t.vrijemeKraja)}'),
                _red(Icons.pets, 'Ljubimac', t.ljubimacIme),
                _red(Icons.medical_services_outlined, 'Veterinar', 'Dr. ${t.veterinarIme} ${t.veterinarPrezime}'),
                _red(Icons.payments_outlined, 'Način plaćanja', t.nacinPlacanjaNaziv),
                if (t.jePrevoz) _red(Icons.local_shipping_outlined, 'Prevoz', '${t.adresaPrevoza ?? ''}, ${t.gradPrevozaNaziv ?? ''}'),
                if (t.razlogPromjeneStatusa != null) _red(Icons.info_outline, 'Napomena uz status', t.razlogPromjeneStatusa!),
                const Divider(height: 24),
                Text('Usluge', style: Theme.of(context).textTheme.titleMedium?.copyWith(fontWeight: FontWeight.bold)),
                for (final s in t.stavke) ListTile(dense: true, contentPadding: EdgeInsets.zero, title: Text(s.uslugaNaziv), trailing: Text(formatNovac(s.cijena))),
                if (t.iznosPopusta > 0) ListTile(dense: true, contentPadding: EdgeInsets.zero, title: const Text('Popust'), trailing: Text('-${formatNovac(t.iznosPopusta)}')),
                if (t.cijenaPrevoza > 0) ListTile(dense: true, contentPadding: EdgeInsets.zero, title: const Text('Prevoz'), trailing: Text(formatNovac(t.cijenaPrevoza))),
                ListTile(
                  dense: true,
                  contentPadding: EdgeInsets.zero,
                  title: const Text('Ukupno', style: TextStyle(fontWeight: FontWeight.bold, fontSize: 16)),
                  trailing: Text(formatNovac(t.ukupnaCijena), style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 16)),
                ),
                const SizedBox(height: 16),
                if (aktivan)
                  Tooltip(
                    message: dovoljnoRano ? '' : 'Termin je moguće otkazati najkasnije ${k.minimalnoSatiZaOtkazivanje} sati prije početka.',
                    child: OutlinedButton.icon(
                      onPressed: dovoljnoRano ? () => _otkazi(t) : null,
                      icon: const Icon(Icons.event_busy_outlined),
                      label: const Text('Otkaži termin'),
                      style: OutlinedButton.styleFrom(foregroundColor: Theme.of(context).colorScheme.error),
                    ),
                  ),
                if (aktivan && !dovoljnoRano)
                  Padding(
                    padding: const EdgeInsets.only(top: 8),
                    child: Text(
                      'Otkazivanje je moguće najkasnije ${k.minimalnoSatiZaOtkazivanje} sati prije početka. Za hitna otkazivanja kontaktirajte stanicu.',
                      style: const TextStyle(color: EvetBoje.tekstSivi),
                    ),
                  ),
                if (t.status == StatusTermina.zavrsen)
                  FilledButton.icon(
                    onPressed: t.imaRecenziju ? null : () => _recenzija(t),
                    icon: const Icon(Icons.thumb_up_alt_outlined),
                    label: Text(t.imaRecenziju ? 'Recenzija je ostavljena' : 'Ocijenite uslugu i veterinara'),
                  ),
              ],
            );
          },
        ),
      ),
    );
  }
}
