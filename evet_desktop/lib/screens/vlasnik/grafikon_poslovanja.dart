import 'package:evet_shared/evet_shared.dart';
import 'package:fl_chart/fl_chart.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Grafički prikaz poslovanja kroz vrijeme (mockup sa slike 2): termini i operacije kao linije, prihod kao stupci.
class GrafikonPoslovanja extends StatefulWidget {
  const GrafikonPoslovanja({super.key});

  @override
  State<GrafikonPoslovanja> createState() => _GrafikonPoslovanjaState();
}

class _GrafikonPoslovanjaState extends State<GrafikonPoslovanja> {
  static const _bojaTermina = Color(0xFF7DD3E0);
  static const _bojaOperacija = Color(0xFF2A9DB5);
  static const _mjeseci = ['Jan', 'Feb', 'Mar', 'Apr', 'Maj', 'Jun', 'Jul', 'Aug', 'Sep', 'Okt', 'Nov', 'Dec'];

  GrupisanjeStatistike _grupisanje = GrupisanjeStatistike.godina;
  int _godina = DateTime.now().year;
  late Future<List<StatistikaPeriod>> _podaci;

  @override
  void initState() {
    super.initState();
    _ucitaj();
  }

  void _ucitaj() => _podaci = context.read<StatistikaProvider>().poPeriodima(
        _grupisanje,
        godina: _grupisanje == GrupisanjeStatistike.mjesec ? _godina : null,
      );

  String _oznaka(StatistikaPeriod p) => p.mjesec == null ? '${p.godina}' : _mjeseci[p.mjesec! - 1];

  Widget _osa(double vrijednost, TitleMeta meta, List<StatistikaPeriod> periodi) {
    final i = vrijednost.toInt();
    if (i < 0 || i >= periodi.length || vrijednost != i.toDouble()) return const SizedBox.shrink();
    return SideTitleWidget(meta: meta, child: Text(_oznaka(periodi[i]), style: const TextStyle(fontSize: 12)));
  }

  Widget _legenda(Color boja, String naziv) => Row(mainAxisSize: MainAxisSize.min, children: [
        Container(width: 12, height: 12, decoration: BoxDecoration(color: boja, shape: BoxShape.circle)),
        const SizedBox(width: 6),
        Text(naziv),
      ]);

  @override
  Widget build(BuildContext context) {
    return Card(
      child: Padding(
        padding: const EdgeInsets.all(20),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Text('Poslovanje kroz vrijeme', style: Theme.of(context).textTheme.titleLarge),
                const Spacer(),
                SegmentedButton<GrupisanjeStatistike>(
                  segments: const [
                    ButtonSegment(value: GrupisanjeStatistike.godina, label: Text('Po godinama')),
                    ButtonSegment(value: GrupisanjeStatistike.mjesec, label: Text('Po mjesecima')),
                  ],
                  selected: {_grupisanje},
                  onSelectionChanged: (s) => setState(() {
                    _grupisanje = s.first;
                    _ucitaj();
                  }),
                ),
                if (_grupisanje == GrupisanjeStatistike.mjesec) ...[
                  const SizedBox(width: 12),
                  DropdownButton<int>(
                    value: _godina,
                    items: [for (var g = DateTime.now().year + 1; g >= DateTime.now().year - 5; g--) DropdownMenuItem(value: g, child: Text('$g'))],
                    onChanged: (g) => setState(() {
                      _godina = g ?? _godina;
                      _ucitaj();
                    }),
                  ),
                ],
              ],
            ),
            const SizedBox(height: 16),
            FutureBuilder<List<StatistikaPeriod>>(
              future: _podaci,
              builder: (context, snapshot) {
                if (snapshot.hasError) return PorukaGreske(snapshot.error.toString(), onPokusajPonovo: () => setState(_ucitaj));
                if (!snapshot.hasData) return const SizedBox(height: 300, child: Center(child: CircularProgressIndicator()));
                final periodi = snapshot.data!;
                if (periodi.isEmpty) return const SizedBox(height: 120, child: Center(child: Text('Nema podataka o terminima za odabrani period.')));

                LineChartBarData linija(Color boja, double Function(StatistikaPeriod) vrijednost) => LineChartBarData(
                      spots: [for (final (i, p) in periodi.indexed) FlSpot(i.toDouble(), vrijednost(p))],
                      color: boja,
                      barWidth: 3,
                      dotData: const FlDotData(show: true),
                    );

                final osaX = AxisTitles(sideTitles: SideTitles(showTitles: true, interval: 1, getTitlesWidget: (v, m) => _osa(v, m, periodi)));
                const bezOsa = AxisTitles();

                return Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Wrap(spacing: 20, children: [_legenda(_bojaTermina, 'Termini'), _legenda(_bojaOperacija, 'Operacije')]),
                    const SizedBox(height: 12),
                    SizedBox(
                      height: 260,
                      child: LineChart(LineChartData(
                        minY: 0,
                        lineBarsData: [linija(_bojaTermina, (p) => p.brojTermina.toDouble()), linija(_bojaOperacija, (p) => p.brojOperacija.toDouble())],
                        titlesData: FlTitlesData(bottomTitles: osaX, topTitles: bezOsa, rightTitles: bezOsa),
                        borderData: FlBorderData(show: false),
                      )),
                    ),
                    const SizedBox(height: 24),
                    _legenda(EvetBoje.tamnoplava, 'Prihod (KM, završeni termini)'),
                    const SizedBox(height: 12),
                    SizedBox(
                      height: 220,
                      child: BarChart(BarChartData(
                        minY: 0,
                        barGroups: [
                          for (final (i, p) in periodi.indexed)
                            BarChartGroupData(x: i, barRods: [BarChartRodData(toY: p.prihod, color: EvetBoje.tamnoplava, width: 22, borderRadius: BorderRadius.circular(4))]),
                        ],
                        barTouchData: BarTouchData(
                          touchTooltipData: BarTouchTooltipData(getTooltipItem: (g, _, rod, _) => BarTooltipItem(formatNovac(rod.toY), const TextStyle(color: Colors.white))),
                        ),
                        titlesData: FlTitlesData(
                          bottomTitles: osaX,
                          topTitles: bezOsa,
                          rightTitles: bezOsa,
                          leftTitles: const AxisTitles(sideTitles: SideTitles(showTitles: true, reservedSize: 56)),
                        ),
                        borderData: FlBorderData(show: false),
                      )),
                    ),
                  ],
                );
              },
            ),
          ],
        ),
      ),
    );
  }
}
