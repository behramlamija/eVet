import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../providers/korpa_provider.dart';
import '../widgets/preporuke_sekcija.dart';

/// Detalji usluge s dodavanjem u korpu za rezervaciju.
class UslugaDetaljiScreen extends StatelessWidget {
  const UslugaDetaljiScreen({super.key, required this.usluga});

  final Usluge usluga;

  @override
  Widget build(BuildContext context) {
    final korpa = context.watch<KorpaProvider>();
    final uKorpi = korpa.sadrzi(usluga.uslugaId);
    final slike = context.read<SlikeProvider>();

    return Scaffold(
      appBar: AppBar(title: Text(usluga.naziv)),
      body: ListView(
        padding: const EdgeInsets.all(16),
        children: [
          ClipRRect(
            borderRadius: BorderRadius.circular(16),
            child: AspectRatio(
              aspectRatio: 16 / 9,
              child: usluga.imaSliku
                  ? Image.network(
                      slike.url(TipSlike.usluga, usluga.uslugaId),
                      headers: slike.headers,
                      fit: BoxFit.cover,
                      errorBuilder: (_, _, _) => const _ZamjenskaSlika(),
                    )
                  : const _ZamjenskaSlika(),
            ),
          ),
          const SizedBox(height: 16),
          Text(usluga.naziv, style: Theme.of(context).textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold)),
          const SizedBox(height: 8),
          Wrap(
            spacing: 8,
            children: [
              Chip(avatar: const Icon(Icons.category_outlined, size: 18), label: Text(usluga.vrstaNaziv)),
              Chip(avatar: const Icon(Icons.schedule, size: 18), label: Text('${usluga.trajanje} min')),
            ],
          ),
          const SizedBox(height: 12),
          Text(usluga.opis ?? 'Opis usluge nije unesen.', style: const TextStyle(fontSize: 16)),
          const SizedBox(height: 24),
          Row(
            children: [
              Text(formatNovac(usluga.cijena), style: Theme.of(context).textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold, color: EvetBoje.tamnoplava)),
              const Spacer(),
              FilledButton.icon(
                onPressed: uKorpi
                    ? null
                    : () {
                        context.read<KorpaProvider>().dodaj(usluga);
                        prikaziUspjeh(context, 'Usluga "${usluga.naziv}" je dodana u korpu.');
                      },
                icon: Icon(uKorpi ? Icons.check : Icons.add_shopping_cart),
                label: Text(uKorpi ? 'U korpi' : 'Dodaj u korpu'),
              ),
            ],
          ),
          const SizedBox(height: 32),
          PreporukeSekcija(key: ValueKey(usluga.uslugaId), uslugeIds: [usluga.uslugaId]),
        ],
      ),
    );
  }
}

class _ZamjenskaSlika extends StatelessWidget {
  const _ZamjenskaSlika();

  @override
  Widget build(BuildContext context) {
    return Container(
      color: Theme.of(context).colorScheme.surfaceContainerHighest,
      child: const Icon(Icons.medical_services_outlined, size: 64, color: EvetBoje.tamnoplava),
    );
  }
}
