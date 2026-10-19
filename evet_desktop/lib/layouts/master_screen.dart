import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../screens/profil_screen.dart';
import '../screens/veterinar/pacijenti_screen.dart';
import '../screens/veterinar/terapije_screen.dart';
import '../screens/veterinar/termini_screen.dart';
import '../screens/vlasnik/izvjestaji_screen.dart';
import '../screens/vlasnik/recenzije_screen.dart';
import '../screens/vlasnik/sifrarnici/sifrarnici_screen.dart';
import '../screens/vlasnik/statistika_screen.dart';
import '../screens/vlasnik/usluge/usluge_screen.dart';
import '../screens/vlasnik/veterinari/veterinari_screen.dart';
import '../widgets/obavijesti_dijalog.dart';

class _StavkaMenija {
  const _StavkaMenija(this.naziv, this.ikona, this.ekran);

  final String naziv;
  final IconData ikona;
  final Widget ekran;
}

/// Okvir desktop aplikacije: tamnoplavi meni lijevo (prema mockupima), zaglavlje i sadržaj.
class MasterScreen extends StatefulWidget {
  const MasterScreen({super.key});

  @override
  State<MasterScreen> createState() => _MasterScreenState();
}

class _MasterScreenState extends State<MasterScreen> {
  int _odabrano = 0;

  @override
  void initState() {
    super.initState();
    context.read<ObavijestiProvider>().pokreniOsvjezavanje();
  }

  List<_StavkaMenija> _meni(AuthProvider auth) {
    if (auth.imaUlogu(Uloge.vlasnik)) {
      return const [
        _StavkaMenija('Statistika', Icons.bar_chart, StatistikaScreen()),
        _StavkaMenija('Veterinari', Icons.groups_outlined, VeterinariScreen()),
        _StavkaMenija('Usluge i promocije', Icons.medical_services_outlined, UslugeScreen()),
        _StavkaMenija('Recenzije', Icons.reviews_outlined, RecenzijeScreen()),
        _StavkaMenija('Izvještaji', Icons.picture_as_pdf_outlined, IzvjestajiScreen()),
        _StavkaMenija('Šifrarnici', Icons.list_alt_outlined, SifrarniciScreen()),
        _StavkaMenija('Postavke', Icons.settings_outlined, ProfilScreen()),
      ];
    }

    return const [
      _StavkaMenija('Termini', Icons.calendar_month_outlined, TerminiScreen()),
      _StavkaMenija('Pacijenti', Icons.person_outline, PacijentiScreen()),
      _StavkaMenija('Terapije', Icons.add_box_outlined, TerapijeScreen()),
      _StavkaMenija('Postavke', Icons.settings_outlined, ProfilScreen()),
    ];
  }

  Future<void> _odjava() async {
    final obavijesti = context.read<ObavijestiProvider>();
    final auth = context.read<AuthProvider>();
    obavijesti.zaustaviOsvjezavanje();
    await auth.logout();
  }

  @override
  Widget build(BuildContext context) {
    final auth = context.watch<AuthProvider>();
    final korisnik = auth.korisnik!;
    final meni = _meni(auth);
    final odabrano = _odabrano < meni.length ? _odabrano : 0;

    return Scaffold(
      body: Row(
        children: [
          Container(
            width: 230,
            color: EvetBoje.tamnoplava,
            padding: const EdgeInsets.symmetric(vertical: 24),
            child: Column(
              children: [
                for (var i = 0; i < meni.length; i++)
                  ListTile(
                    selected: i == odabrano,
                    selectedTileColor: Colors.white.withValues(alpha: 0.12),
                    leading: Icon(meni[i].ikona, color: Colors.white),
                    title: Text(meni[i].naziv, style: const TextStyle(color: Colors.white, fontSize: 16)),
                    onTap: () => setState(() => _odabrano = i),
                  ),
              ],
            ),
          ),
          Expanded(
            child: Column(
              children: [
                Padding(
                  padding: const EdgeInsets.fromLTRB(32, 20, 24, 12),
                  child: Row(
                    children: [
                      const EvetLogo(),
                      const Spacer(),
                      Consumer<ObavijestiProvider>(
                        builder: (_, obavijesti, _) => IconButton(
                          tooltip: 'Obavijesti',
                          onPressed: () => prikaziObavijesti(context),
                          icon: Badge(
                            isLabelVisible: obavijesti.brojNeprocitanih > 0,
                            label: Text('${obavijesti.brojNeprocitanih}'),
                            child: const Icon(Icons.notifications_outlined, color: EvetBoje.tamnoplava),
                          ),
                        ),
                      ),
                      const SizedBox(width: 24),
                      PopupMenuButton<void>(
                        tooltip: 'Korisnički račun',
                        child: Row(
                          children: [
                            Text('${korisnik.ime} ${korisnik.prezime}', style: const TextStyle(fontSize: 16)),
                            const SizedBox(width: 8),
                            const Icon(Icons.person_outline),
                          ],
                        ),
                        itemBuilder: (_) => [
                          PopupMenuItem(onTap: _odjava, child: const ListTile(leading: Icon(Icons.logout), title: Text('Odjava'))),
                        ],
                      ),
                    ],
                  ),
                ),
                const Divider(height: 1),
                Expanded(child: meni[odabrano].ekran),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
