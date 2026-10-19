import 'package:flutter/material.dart';

import '../theme/evet_theme.dart';

class EvetLogo extends StatelessWidget {
  const EvetLogo({super.key, this.velicina = 28});

  final double velicina;

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisSize: MainAxisSize.min,
      children: [
        Icon(Icons.pets, color: EvetBoje.crvena, size: velicina * 1.2),
        const SizedBox(width: 8),
        Text(
          'PetCity',
          style: TextStyle(fontSize: velicina, fontWeight: FontWeight.bold, color: EvetBoje.tamnoplava),
        ),
      ],
    );
  }
}
