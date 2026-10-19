import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'grafikon_poslovanja.dart';

enum _Period { danas, mjesec, godina, sve }

/// Statistika poslovanja za vlasnika (mockup sa slike 1 u prijavi).
class StatistikaScreen extends StatefulWidget {
  const StatistikaScreen({super.key});

  @override
  State<StatistikaScreen> createState() => _StatistikaScreenState();
}

class _StatistikaScreenState extends State<StatistikaScreen> {
  _Period _period = _Period.mjesec;
  late Future<StatistikaPoslovanja> _podaci;

  @override
  void initState() {
    super.initState();
    _ucitaj();
  }

  void _ucitaj() {
    final danas = DateTime.now();
    final od = switch (_period) {
      _Period.danas => DateTime(danas.year, danas.month, danas.day),
      _Period.mjesec => DateTime(danas.year, danas.month),
      _Period.godina => DateTime(danas.year),
      _Period.sve => null,
    };
    _podaci = context.read<StatistikaProvider>().poslovanje(datumOd: od, datumDo: od == null ? null : danas);
  }

  @override
  Widget build(BuildContext context) {
    return ListView(
      padding: const EdgeInsets.all(32),
      children: [
        SegmentedButton<_Period>(
          segments: const [
            ButtonSegment(value: _Period.danas, label: Text('Danas')),
            ButtonSegment(value: _Period.mjesec, label: Text('Ovaj mjesec')),
            ButtonSegment(value: _Period.godina, label: Text('Ova godina')),
            ButtonSegment(value: _Period.sve, label: Text('Sve')),
          ],
          selected: {_period},
          onSelectionChanged: (odabir) => setState(() {
            _period = odabir.first;
            _ucitaj();
          }),
        ),
        const SizedBox(height: 24),
        FutureBuilder<StatistikaPoslovanja>(
          future: _podaci,
          builder: (context, snapshot) {
            if (snapshot.hasError) {
              return PorukaGreske(snapshot.error.toString(), onPokusajPonovo: () => setState(_ucitaj));
            }
            if (!snapshot.hasData) {
              return const Center(child: CircularProgressIndicator());
            }

            final s = snapshot.data!;
            return Wrap(
              spacing: 16,
              runSpacing: 16,
              children: [
                _Kartica('Ukupni prihod', formatNovac(s.ukupniPrihod), Icons.payments_outlined, istaknuto: true),
                _Kartica('Broj operacija', '${s.brojOperacija}', Icons.medical_services_outlined, istaknuto: true),
                _Kartica('Broj termina', '${s.brojTermina}', Icons.calendar_month_outlined),
                _Kartica('Broj pruženih usluga', '${s.brojPruzenihUsluga}', Icons.add_box_outlined),
                _Kartica('TopVet', s.topVeterinar ?? '-', Icons.favorite_border),
                _Kartica('Novi klijenti', '${s.brojNovihKlijenata}', Icons.person_add_alt),
                _Kartica('Najčešća usluga', s.najcescaUsluga ?? '-', Icons.content_cut),
                _Kartica('Najčešća rasa', s.najzastupljenijaRasa ?? '-', Icons.pets),
              ],
            );
          },
        ),
        const SizedBox(height: 24),
        const GrafikonPoslovanja(),
      ],
    );
  }
}

class _Kartica extends StatelessWidget {
  const _Kartica(this.naslov, this.vrijednost, this.ikona, {this.istaknuto = false});

  final String naslov;
  final String vrijednost;
  final IconData ikona;
  final bool istaknuto;

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: 260,
      child: Card(
        child: Padding(
          padding: const EdgeInsets.all(20),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                children: [
                  Icon(ikona, color: istaknuto ? EvetBoje.crvena : EvetBoje.tamnoplava),
                  const SizedBox(width: 8),
                  Text(naslov, style: const TextStyle(fontWeight: FontWeight.w600)),
                ],
              ),
              const SizedBox(height: 12),
              Text(
                vrijednost,
                style: TextStyle(fontSize: 26, fontWeight: FontWeight.bold, color: istaknuto ? EvetBoje.crvena : EvetBoje.tamnoplava),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
