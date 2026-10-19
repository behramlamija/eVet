import 'package:flutter/material.dart';

import '../models/models.dart';
import '../providers/slike_provider.dart';

/// Slika entiteta sa zaštićenog endpointa; ako slike nema, prikazuje se ikona.
class EvetSlika extends StatelessWidget {
  const EvetSlika({
    super.key,
    required this.slike,
    required this.tip,
    required this.id,
    required this.imaSliku,
    this.velicina = 48,
    this.zamjenskaIkona = Icons.pets,
    this.verzija,
  });

  final SlikeProvider slike;
  final TipSlike tip;
  final int id;
  final bool imaSliku;
  final double velicina;
  final IconData zamjenskaIkona;

  /// Mijenja se nakon uploada nove slike, da se ne prikaže stara slika iz keša.
  final int? verzija;

  @override
  Widget build(BuildContext context) {
    final zamjena = CircleAvatar(
      radius: velicina / 2,
      backgroundColor: Theme.of(context).colorScheme.surfaceContainerHighest,
      child: Icon(zamjenskaIkona, size: velicina / 2),
    );

    if (!imaSliku) {
      return zamjena;
    }

    return ClipOval(
      child: Image.network(
        slike.url(tip, id, verzija: verzija),
        headers: slike.headers,
        width: velicina,
        height: velicina,
        fit: BoxFit.cover,
        errorBuilder: (_, _, _) => zamjena,
      ),
    );
  }
}
