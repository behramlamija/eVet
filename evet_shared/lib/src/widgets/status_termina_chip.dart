import 'package:flutter/material.dart';

import '../models/models.dart';
import '../utils/nazivi.dart';

class StatusTerminaChip extends StatelessWidget {
  const StatusTerminaChip(this.status, {super.key});

  final StatusTermina status;

  @override
  Widget build(BuildContext context) {
    final boja = switch (status) {
      StatusTermina.naCekanju => Colors.orange,
      StatusTermina.potvrdjen => Colors.blue,
      StatusTermina.zavrsen => Colors.green,
      StatusTermina.otkazan || StatusTermina.odbijen => Colors.red,
    };
    return Chip(
      label: Text(nazivStatusa(status), style: TextStyle(color: boja.shade800)),
      backgroundColor: boja.shade50,
      side: BorderSide(color: boja.shade200),
      visualDensity: VisualDensity.compact,
    );
  }
}
