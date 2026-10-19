import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../providers/korpa_provider.dart';
import '../screens/korpa_screen.dart';
import '../screens/obavijesti_screen.dart';
import '../screens/pocetna_screen.dart';
import '../screens/profil_screen.dart';
import '../screens/termini_screen.dart';

/// Glavni okvir mobilne aplikacije s donjom navigacijom.
class MasterScreen extends StatefulWidget {
  const MasterScreen({super.key});

  @override
  State<MasterScreen> createState() => _MasterScreenState();
}

class _MasterScreenState extends State<MasterScreen> {
  int _odabrano = 0;

  static const _ekrani = [PocetnaScreen(), TerminiScreen(), KorpaScreen(), ObavijestiScreen(), ProfilScreen()];

  @override
  void initState() {
    super.initState();
    context.read<ObavijestiProvider>().pokreniOsvjezavanje();
  }

  void _promijeniTab(int i) => setState(() => _odabrano = i);

  @override
  Widget build(BuildContext context) {
    final brojNeprocitanih = context.watch<ObavijestiProvider>().brojNeprocitanih;
    final brojUKorpi = context.watch<KorpaProvider>().brojStavki;

    return Scaffold(
      body: SafeArea(child: IndexedStack(index: _odabrano, children: _ekrani)),
      bottomNavigationBar: NavigationBar(
        selectedIndex: _odabrano,
        onDestinationSelected: _promijeniTab,
        destinations: [
          const NavigationDestination(icon: Icon(Icons.home_outlined), selectedIcon: Icon(Icons.home), label: 'Početna'),
          const NavigationDestination(icon: Icon(Icons.event_note_outlined), selectedIcon: Icon(Icons.event_note), label: 'Termini'),
          NavigationDestination(
            icon: Badge(isLabelVisible: brojUKorpi > 0, label: Text('$brojUKorpi'), child: const Icon(Icons.shopping_cart_outlined)),
            label: 'Korpa',
          ),
          NavigationDestination(
            icon: Badge(
              isLabelVisible: brojNeprocitanih > 0,
              label: Text('$brojNeprocitanih'),
              child: const Icon(Icons.notifications_outlined),
            ),
            label: 'Obavijesti',
          ),
          const NavigationDestination(icon: Icon(Icons.person_outline), selectedIcon: Icon(Icons.person), label: 'Profil'),
        ],
      ),
    );
  }
}
