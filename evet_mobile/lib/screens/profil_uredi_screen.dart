import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Izmjena ličnih podataka, slike profila i lozinke (uz potvrdu trenutne lozinke).
class ProfilUrediScreen extends StatefulWidget {
  const ProfilUrediScreen({super.key});

  @override
  State<ProfilUrediScreen> createState() => _ProfilUrediScreenState();
}

class _ProfilUrediScreenState extends State<ProfilUrediScreen> {
  final _podaciKey = GlobalKey<FormState>();
  final _lozinkaKey = GlobalKey<FormState>();
  late final Korisnici _k = context.read<AuthProvider>().korisnik!;
  late final _ime = TextEditingController(text: _k.ime);
  late final _prezime = TextEditingController(text: _k.prezime);
  late final _email = TextEditingController(text: _k.email);
  late final _telefon = TextEditingController(text: _k.telefon);
  final _stara = TextEditingController();
  final _nova = TextEditingController();
  final _potvrda = TextEditingController();
  bool _snima = false;
  ApiException? _greskaPodataka;
  String? _greskaLozinke;
  int _verzijaSlike = 0;

  @override
  void dispose() {
    for (final c in [_ime, _prezime, _email, _telefon, _stara, _nova, _potvrda]) {
      c.dispose();
    }
    super.dispose();
  }

  Future<void> _sacuvajPodatke() async {
    setState(() => _greskaPodataka = null);
    if (!_podaciKey.currentState!.validate()) return;
    setState(() => _snima = true);
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
      if (mounted) setState(() => _snima = false);
    }
  }

  Future<void> _promijeniLozinku() async {
    setState(() => _greskaLozinke = null);
    if (!_lozinkaKey.currentState!.validate()) return;
    setState(() => _snima = true);
    try {
      await context.read<ProfilProvider>().promijeniLozinku(PromjenaLozinkeRequest(staraLozinka: _stara.text, novaLozinka: _nova.text, novaLozinkaPotvrda: _potvrda.text));
      if (!mounted) return;
      _lozinkaKey.currentState!.reset();
      for (final c in [_stara, _nova, _potvrda]) {
        c.clear();
      }
      prikaziUspjeh(context, 'Lozinka je uspješno promijenjena.');
    } on ApiException catch (e) {
      setState(() => _greskaLozinke = e.poruka);
    } finally {
      if (mounted) setState(() => _snima = false);
    }
  }

  Future<void> _promijeniSliku() async {
    try {
      final slika = await odaberiSliku();
      if (slika == null || !mounted) return;
      final ok = await izvrsiAkciju(
        context,
        () => context.read<SlikeProvider>().postavi(TipSlike.korisnik, _k.korisnikId, bytes: slika.bytes, nazivDatoteke: slika.naziv, contentType: slika.contentType),
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
    const razmak = SizedBox(height: 16);

    return Scaffold(
      appBar: AppBar(title: const Text('Uredi profil')),
      body: ListView(
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
          Center(child: TextButton.icon(onPressed: _promijeniSliku, icon: const Icon(Icons.photo_camera_outlined), label: const Text('Promijeni sliku'))),
          Form(
            key: _podaciKey,
            autovalidateMode: AutovalidateMode.onUserInteraction,
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: [
                TextFormField(controller: _ime, decoration: InputDecoration(labelText: 'Ime', errorText: _greskaPodataka?.greskaZaPolje('Ime')), validator: Validatori.tekst('Ime', maksimalnaDuzina: 100)),
                razmak,
                TextFormField(controller: _prezime, decoration: InputDecoration(labelText: 'Prezime', errorText: _greskaPodataka?.greskaZaPolje('Prezime')), validator: Validatori.tekst('Prezime', maksimalnaDuzina: 100)),
                razmak,
                TextFormField(controller: _email, keyboardType: TextInputType.emailAddress, decoration: InputDecoration(labelText: 'Email', errorText: _greskaPodataka?.greskaZaPolje('Email')), validator: Validatori.email),
                razmak,
                TextFormField(controller: _telefon, keyboardType: TextInputType.phone, decoration: InputDecoration(labelText: 'Telefon (opcionalno)', errorText: _greskaPodataka?.greskaZaPolje('Telefon')), validator: Validatori.telefonOpcionalno),
                if (_greskaPodataka case final g? when g.greske.isEmpty) ...[razmak, PorukaGreske(g.poruka)],
                razmak,
                FilledButton(onPressed: _snima ? null : _sacuvajPodatke, child: const Text('Sačuvaj podatke')),
              ],
            ),
          ),
          const Divider(height: 48),
          Form(
            key: _lozinkaKey,
            autovalidateMode: AutovalidateMode.onUserInteraction,
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: [
                Text('Promjena lozinke', style: Theme.of(context).textTheme.titleMedium?.copyWith(fontWeight: FontWeight.bold)),
                razmak,
                TextFormField(controller: _stara, obscureText: true, decoration: const InputDecoration(labelText: 'Trenutna lozinka'), validator: (v) => (v == null || v.isEmpty) ? 'Unesite trenutnu lozinku.' : null),
                razmak,
                TextFormField(controller: _nova, obscureText: true, decoration: const InputDecoration(labelText: 'Nova lozinka'), validator: Validatori.lozinka),
                razmak,
                TextFormField(controller: _potvrda, obscureText: true, decoration: const InputDecoration(labelText: 'Potvrda nove lozinke'), validator: Validatori.potvrdaLozinke(() => _nova.text)),
                if (_greskaLozinke != null) ...[razmak, PorukaGreske(_greskaLozinke!)],
                razmak,
                OutlinedButton(onPressed: _snima ? null : _promijeniLozinku, child: const Text('Promijeni lozinku')),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
