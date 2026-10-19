import 'package:flutter/material.dart';

/// Poruka greške unutar forme ili ekrana (ne kao dijalog), uz opcionalno dugme za ponovni pokušaj.
class PorukaGreske extends StatelessWidget {
  const PorukaGreske(this.poruka, {super.key, this.onPokusajPonovo});

  final String poruka;
  final VoidCallback? onPokusajPonovo;

  @override
  Widget build(BuildContext context) {
    final boja = Theme.of(context).colorScheme.error;
    return Container(
      padding: const EdgeInsets.all(12),
      decoration: BoxDecoration(
        color: boja.withValues(alpha: 0.08),
        borderRadius: BorderRadius.circular(8),
        border: Border.all(color: boja.withValues(alpha: 0.4)),
      ),
      child: Row(
        children: [
          Icon(Icons.error_outline, color: boja),
          const SizedBox(width: 8),
          Expanded(child: Text(poruka, style: TextStyle(color: boja))),
          if (onPokusajPonovo != null) TextButton(onPressed: onPokusajPonovo, child: const Text('Pokušaj ponovo')),
        ],
      ),
    );
  }
}
