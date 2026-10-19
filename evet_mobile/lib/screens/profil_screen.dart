import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'ljubimci_screen.dart';
import 'profil_uredi_screen.dart';
import 'promocije_screen.dart';

class ProfilScreen extends StatefulWidget {
  const ProfilScreen({super.key});

  @override
  State<ProfilScreen> createState() => _ProfilScreenState();
}

class _ProfilScreenState extends State<ProfilScreen> {
  int _verzijaSlike = 0;

  Future<void> _odjava() async {
    final potvrda = await potvrdiAkciju(context, naslov: 'Odjava', poruka: 'Želite li se odjaviti?', potvrda: 'Odjavi se');
    if (!potvrda || !mounted) return;
    final obavijesti = context.read<ObavijestiProvider>();
    final auth = context.read<AuthProvider>();
    obavijesti.zaustaviOsvjezavanje();
    await auth.logout();
  }

  Future<void> _otvori(Widget ekran) async {
    await Navigator.of(context).push(MaterialPageRoute(builder: (_) => ekran));
    if (mounted) setState(() => _verzijaSlike++);
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
            verzija: _verzijaSlike,
          ),
        ),
        const SizedBox(height: 12),
        Center(child: Text('${korisnik.ime} ${korisnik.prezime}', style: Theme.of(context).textTheme.titleLarge)),
        Center(child: Text(korisnik.email, style: const TextStyle(color: EvetBoje.tekstSivi))),
        const SizedBox(height: 24),
        Card(
          child: Column(
            children: [
              ListTile(
                leading: const Icon(Icons.edit_outlined),
                title: const Text('Uredi profil i lozinku'),
                trailing: const Icon(Icons.chevron_right),
                onTap: () => _otvori(const ProfilUrediScreen()),
              ),
              const Divider(height: 1),
              ListTile(
                leading: const Icon(Icons.pets),
                title: const Text('Moji ljubimci'),
                trailing: const Icon(Icons.chevron_right),
                onTap: () => _otvori(const LjubimciScreen()),
              ),
              const Divider(height: 1),
              ListTile(
                leading: const Icon(Icons.local_offer_outlined),
                title: const Text('Promocije i kuponi'),
                trailing: const Icon(Icons.chevron_right),
                onTap: () => _otvori(const PromocijeScreen()),
              ),
            ],
          ),
        ),
        const SizedBox(height: 24),
        OutlinedButton.icon(onPressed: _odjava, icon: const Icon(Icons.logout), label: const Text('Odjava')),
      ],
    );
  }
}
