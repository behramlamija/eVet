import 'package:flutter/material.dart';

class Paginacija extends StatelessWidget {
  const Paginacija({super.key, required this.stranica, required this.ukupno, required this.velicinaStranice, required this.onPromjena});

  final int stranica;
  final int ukupno;
  final int velicinaStranice;
  final ValueChanged<int> onPromjena;

  int get _brojStranica => ukupno == 0 ? 1 : (ukupno / velicinaStranice).ceil();

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.end,
      children: [
        Text('Ukupno: $ukupno'),
        const SizedBox(width: 16),
        IconButton(
          tooltip: 'Prethodna stranica',
          onPressed: stranica > 0 ? () => onPromjena(stranica - 1) : null,
          icon: const Icon(Icons.chevron_left),
        ),
        Text('Stranica ${stranica + 1} od $_brojStranica'),
        IconButton(
          tooltip: 'Sljedeća stranica',
          onPressed: stranica + 1 < _brojStranica ? () => onPromjena(stranica + 1) : null,
          icon: const Icon(Icons.chevron_right),
        ),
      ],
    );
  }
}
