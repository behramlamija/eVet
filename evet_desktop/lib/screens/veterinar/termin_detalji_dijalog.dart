import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';

/// Detalji termina (master-detail): podaci o terminu i stavke s cijenama.
Future<void> prikaziDetaljeTermina(BuildContext context, Termini t) {
  return showDialog<void>(
    context: context,
    builder: (_) => FormaDijalog(
      naslov: 'Termin ${t.sifra}',
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Align(alignment: Alignment.centerLeft, child: StatusTerminaChip(t.status)),
          const SizedBox(height: 12),
          _Red(Icons.calendar_today_outlined, 'Datum i vrijeme', '${formatDatum(t.datumTermina)}, ${formatVrijeme(t.vrijemePocetka)} - ${formatVrijeme(t.vrijemeKraja)}'),
          _Red(Icons.pets, 'Pacijent', t.ljubimacIme),
          _Red(Icons.person_outline, 'Vlasnik', '${t.korisnikIme} ${t.korisnikPrezime}'),
          _Red(Icons.medical_services_outlined, 'Veterinar', '${t.veterinarIme} ${t.veterinarPrezime}'),
          _Red(Icons.payments_outlined, 'Plaćanje', '${t.nacinPlacanjaNaziv}${t.jePlacen ? ' (plaćeno; pri otkazivanju se novac automatski vraća)' : t.jeRefundiran ? ' (novac vraćen)' : ''}'),
          if (t.jePrevoz) _Red(Icons.local_shipping_outlined, 'Prevoz', '${t.adresaPrevoza ?? ''}, ${t.gradPrevozaNaziv ?? ''}'),
          if (t.razlogPromjeneStatusa != null)
            _Red(Icons.info_outline, 'Razlog (${t.statusPromijenioIme ?? ''} ${t.statusPromijenioPrezime ?? ''})', t.razlogPromjeneStatusa!),
          const Divider(height: 32),
          Text('Usluge', style: Theme.of(context).textTheme.titleMedium),
          for (final s in t.stavke)
            ListTile(dense: true, contentPadding: EdgeInsets.zero, title: Text(s.uslugaNaziv), trailing: Text(formatNovac(s.cijena))),
          if (t.iznosPopusta > 0)
            ListTile(dense: true, contentPadding: EdgeInsets.zero, title: const Text('Popust (kupon)'), trailing: Text('-${formatNovac(t.iznosPopusta)}')),
          if (t.cijenaPrevoza > 0)
            ListTile(dense: true, contentPadding: EdgeInsets.zero, title: const Text('Prevoz'), trailing: Text(formatNovac(t.cijenaPrevoza))),
          ListTile(
            dense: true,
            contentPadding: EdgeInsets.zero,
            title: const Text('Ukupno', style: TextStyle(fontWeight: FontWeight.bold)),
            trailing: Text(formatNovac(t.ukupnaCijena), style: const TextStyle(fontWeight: FontWeight.bold)),
          ),
        ],
      ),
    ),
  );
}

class _Red extends StatelessWidget {
  const _Red(this.ikona, this.naziv, this.vrijednost);

  final IconData ikona;
  final String naziv;
  final String vrijednost;

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 6),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Icon(ikona, size: 20, color: EvetBoje.tamnoplava),
          const SizedBox(width: 12),
          SizedBox(width: 160, child: Text(naziv, style: const TextStyle(color: EvetBoje.tekstSivi))),
          Expanded(child: Text(vrijednost)),
        ],
      ),
    );
  }
}
