import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Uređivanje vlastitog profila i promjena lozinke (uz potvrdu stare lozinke).
class ProfilScreen extends StatefulWidget {
  const ProfilScreen({super.key});

  @override
  State<ProfilScreen> createState() => _ProfilScreenState();
}

class _ProfilScreenState extends State<ProfilScreen> {
  final _podaciKey = GlobalKey<FormState>();
  final _lozinkaKey = GlobalKey<FormState>();
  late final Korisnici _korisnik = context.read<AuthProvider>().korisnik!;
  late final _ime = TextEditingController(text: _korisnik.ime);
  late final _prezime = TextEditingController(text: _korisnik.prezime);
  late final _email = TextEditingController(text: _korisnik.email);
  late final _telefon = TextEditingController(text: _korisnik.telefon);
  final _staraLozinka = TextEditingController();
  final _novaLozinka = TextEditingController();
  final _potvrda = TextEditingController();
  bool _snimaPodatke = false;
  bool _snimaLozinku = false;
  ApiException? _greskaPodataka;
  String? _greskaLozinke;
  int _verzijaSlike = 0;

  @override
  void dispose() {
    for (final c in [_ime, _prezime, _email, _telefon, _staraLozinka, _novaLozinka, _potvrda]) {
      c.dispose();
    }
    super.dispose();
  }

  Future<void> _sacuvajPodatke() async {
    setState(() => _greskaPodataka = null);
    if (!_podaciKey.currentState!.validate()) return;

    setState(() => _snimaPodatke = true);
    try {
      final azuriran = await context.read<ProfilProvider>().update(ProfilUpdateRequest(
            ime: _ime.text.trim(),
            prezime: _prezime.text.trim(),
            email: _email.text.trim(),
            telefon: _telefon.text.trim().isEmpty ? null : _telefon.text.trim(),
          ));
      if (!mounted) return;
      context.read<AuthProvider>().azurirajKorisnika(azuriran);
      prikaziUspjeh(context, 'Podaci profila su sačuvani.');
    } on ApiException catch (e) {
      setState(() => _greskaPodataka = e);
    } finally {
      if (mounted) setState(() => _snimaPodatke = false);
    }
  }

  Future<void> _promijeniLozinku() async {
    setState(() => _greskaLozinke = null);
    if (!_lozinkaKey.currentState!.validate()) return;

    setState(() => _snimaLozinku = true);
    try {
      await context.read<ProfilProvider>().promijeniLozinku(PromjenaLozinkeRequest(
            staraLozinka: _staraLozinka.text,
            novaLozinka: _novaLozinka.text,
            novaLozinkaPotvrda: _potvrda.text,
          ));
      if (!mounted) return;
      _lozinkaKey.currentState!.reset();
      for (final c in [_staraLozinka, _novaLozinka, _potvrda]) {
        c.clear();
      }
      prikaziUspjeh(context, 'Lozinka je uspješno promijenjena.');
    } on ApiException catch (e) {
      setState(() => _greskaLozinke = e.poruka);
    } finally {
      if (mounted) setState(() => _snimaLozinku = false);
    }
  }

  Future<void> _promijeniSliku() async {
    try {
      final slika = await odaberiSliku();
      if (slika == null || !mounted) return;
      final slike = context.read<SlikeProvider>();
      final ok = await izvrsiAkciju(
        context,
        () => slike.postavi(TipSlike.korisnik, _korisnik.korisnikId, bytes: slika.bytes, nazivDatoteke: slika.naziv, contentType: slika.contentType),
        uspjeh: 'Slika profila je promijenjena.',
      );
      if (ok && mounted) {
        final auth = context.read<AuthProvider>();
        auth.azurirajKorisnika(await context.read<ProfilProvider>().get());
        setState(() => _verzijaSlike++);
      }
    } on ApiException catch (e) {
      if (mounted) prikaziGresku(context, e.poruka);
    }
  }

  @override
  Widget build(BuildContext context) {
    final korisnik = context.watch<AuthProvider>().korisnik!;

    return ListView(
      padding: const EdgeInsets.all(32),
      children: [
        Text('MOJ PROFIL', style: Theme.of(context).textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold)),
        const SizedBox(height: 16),
        Wrap(
          spacing: 24,
          runSpacing: 24,
          crossAxisAlignment: WrapCrossAlignment.start,
          children: [
            SizedBox(
              width: 480,
              child: Card(
                child: Padding(
                  padding: const EdgeInsets.all(24),
                  child: Form(
                    key: _podaciKey,
                    autovalidateMode: AutovalidateMode.onUserInteraction,
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.stretch,
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
                        Center(child: TextButton.icon(onPressed: _promijeniSliku, icon: const Icon(Icons.photo_camera_outlined), label: const Text('Promijeni sliku'))),
                        Text('Korisničko ime: ${korisnik.korisnickoIme}', textAlign: TextAlign.center, style: const TextStyle(color: EvetBoje.tekstSivi)),
                        const SizedBox(height: 16),
                        TextFormField(controller: _ime, decoration: InputDecoration(labelText: 'Ime', errorText: _greskaPodataka?.greskaZaPolje('Ime')), validator: Validatori.tekst('Ime', maksimalnaDuzina: 100)),
                        const SizedBox(height: 16),
                        TextFormField(controller: _prezime, decoration: InputDecoration(labelText: 'Prezime', errorText: _greskaPodataka?.greskaZaPolje('Prezime')), validator: Validatori.tekst('Prezime', maksimalnaDuzina: 100)),
                        const SizedBox(height: 16),
                        TextFormField(controller: _email, decoration: InputDecoration(labelText: 'Email', errorText: _greskaPodataka?.greskaZaPolje('Email')), validator: Validatori.email),
                        const SizedBox(height: 16),
                        TextFormField(controller: _telefon, decoration: InputDecoration(labelText: 'Telefon (opcionalno)', errorText: _greskaPodataka?.greskaZaPolje('Telefon')), validator: Validatori.telefonOpcionalno),
                        if (_greskaPodataka case final g? when g.greske.isEmpty) ...[const SizedBox(height: 16), PorukaGreske(g.poruka)],
                        const SizedBox(height: 24),
                        FilledButton(onPressed: _snimaPodatke ? null : _sacuvajPodatke, child: const Text('Sačuvaj podatke')),
                      ],
                    ),
                  ),
                ),
              ),
            ),
            SizedBox(
              width: 400,
              child: Card(
                child: Padding(
                  padding: const EdgeInsets.all(24),
                  child: Form(
                    key: _lozinkaKey,
                    autovalidateMode: AutovalidateMode.onUserInteraction,
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.stretch,
                      children: [
                        Text('Promjena lozinke', style: Theme.of(context).textTheme.titleMedium),
                        const SizedBox(height: 16),
                        TextFormField(controller: _staraLozinka, obscureText: true, decoration: const InputDecoration(labelText: 'Trenutna lozinka'), validator: (v) => (v == null || v.isEmpty) ? 'Unesite trenutnu lozinku.' : null),
                        const SizedBox(height: 16),
                        TextFormField(controller: _novaLozinka, obscureText: true, decoration: const InputDecoration(labelText: 'Nova lozinka'), validator: Validatori.lozinka),
                        const SizedBox(height: 16),
                        TextFormField(controller: _potvrda, obscureText: true, decoration: const InputDecoration(labelText: 'Potvrda nove lozinke'), validator: Validatori.potvrdaLozinke(() => _novaLozinka.text)),
                        if (_greskaLozinke != null) ...[const SizedBox(height: 16), PorukaGreske(_greskaLozinke!)],
                        const SizedBox(height: 24),
                        FilledButton(onPressed: _snimaLozinku ? null : _promijeniLozinku, child: const Text('Promijeni lozinku')),
                      ],
                    ),
                  ),
                ),
              ),
            ),
          ],
        ),
      ],
    );
  }
}
