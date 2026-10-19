import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

class ProfilScreen extends StatelessWidget {
  const ProfilScreen({super.key});

  Future<void> _odjava(BuildContext context) async {
    final obavijesti = context.read<ObavijestiProvider>();
    final auth = context.read<AuthProvider>();
    obavijesti.zaustaviOsvjezavanje();
    await auth.logout();
  }

  @override
  Widget build(BuildContext context) {
    final korisnik = context.watch<AuthProvider>().korisnik!;

    return ListView(
      padding: const EdgeInsets.all(16),
      children: [
        Center(
          child: EvetSlika(
            slike: context.read<SlikeProvider>(),
            tip: TipSlike.korisnik,
            id: korisnik.korisnikId,
            imaSliku: korisnik.imaSliku,
            velicina: 96,
            zamjenskaIkona: Icons.person,
          ),
        ),
        const SizedBox(height: 12),
        Center(child: Text('${korisnik.ime} ${korisnik.prezime}', style: Theme.of(context).textTheme.titleLarge)),
        const SizedBox(height: 24),
        Card(
          child: Column(
            children: [
              ListTile(leading: const Icon(Icons.alternate_email), title: const Text('Korisničko ime'), subtitle: Text(korisnik.korisnickoIme)),
              ListTile(leading: const Icon(Icons.email_outlined), title: const Text('Email'), subtitle: Text(korisnik.email)),
              ListTile(leading: const Icon(Icons.phone_outlined), title: const Text('Telefon'), subtitle: Text(korisnik.telefon ?? '-')),
            ],
          ),
        ),
        const SizedBox(height: 24),
        OutlinedButton.icon(onPressed: () => _odjava(context), icon: const Icon(Icons.logout), label: const Text('Odjava')),
      ],
    );
  }
}
