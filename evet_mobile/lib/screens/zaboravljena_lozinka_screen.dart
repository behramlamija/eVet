import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Reset zaboravljene lozinke u dva koraka: slanje koda na e-mail, zatim unos koda i nove lozinke.
class ZaboravljenaLozinkaScreen extends StatefulWidget {
  const ZaboravljenaLozinkaScreen({super.key});

  @override
  State<ZaboravljenaLozinkaScreen> createState() => _ZaboravljenaLozinkaScreenState();
}

class _ZaboravljenaLozinkaScreenState extends State<ZaboravljenaLozinkaScreen> {
  final _emailKey = GlobalKey<FormState>();
  final _resetKey = GlobalKey<FormState>();
  final _email = TextEditingController();
  final _kod = TextEditingController();
  final _lozinka = TextEditingController();
  final _potvrda = TextEditingController();
  bool _kodPoslan = false;
  bool _salje = false;
  ApiException? _greska;

  @override
  void dispose() {
    for (final c in [_email, _kod, _lozinka, _potvrda]) {
      c.dispose();
    }
    super.dispose();
  }

  Future<void> _posaljiKod() async {
    setState(() => _greska = null);
    if (!_emailKey.currentState!.validate()) return;
    setState(() => _salje = true);
    try {
      await context.read<AuthProvider>().zatraziResetLozinke(_email.text.trim());
      if (!mounted) return;
      setState(() => _kodPoslan = true);
      prikaziUspjeh(context, 'Ako je e-mail registrovan, na njega je poslan kod za promjenu lozinke.');
    } on ApiException catch (e) {
      setState(() => _greska = e);
    } finally {
      if (mounted) setState(() => _salje = false);
    }
  }

  Future<void> _promijeniLozinku() async {
    setState(() => _greska = null);
    if (!_resetKey.currentState!.validate()) return;
    setState(() => _salje = true);
    try {
      await context.read<AuthProvider>().resetujLozinku(ResetLozinkeRequest(
            email: _email.text.trim(),
            kod: _kod.text.trim(),
            novaLozinka: _lozinka.text,
            novaLozinkaPotvrda: _potvrda.text,
          ));
      if (!mounted) return;
      prikaziUspjeh(context, 'Lozinka je promijenjena. Prijavite se novom lozinkom.');
      Navigator.pop(context, _email.text.trim());
    } on ApiException catch (e) {
      setState(() => _greska = e);
    } finally {
      if (mounted) setState(() => _salje = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    const razmak = SizedBox(height: 16);

    return Scaffold(
      appBar: AppBar(title: const Text('Zaboravljena lozinka')),
      body: SafeArea(
        child: ListView(
          padding: const EdgeInsets.all(24),
          children: [
            Form(
              key: _emailKey,
              autovalidateMode: AutovalidateMode.onUserInteraction,
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  const Text('Unesite e-mail adresu korisničkog računa. Poslat ćemo vam kod za promjenu lozinke.'),
                  razmak,
                  TextFormField(
                    controller: _email,
                    enabled: !_kodPoslan,
                    keyboardType: TextInputType.emailAddress,
                    decoration: InputDecoration(labelText: 'Email', errorText: _greska?.greskaZaPolje('Email')),
                    validator: Validatori.email,
                  ),
                  razmak,
                  if (!_kodPoslan) FilledButton(onPressed: _salje ? null : _posaljiKod, child: const Text('Pošalji kod')),
                ],
              ),
            ),
            if (_kodPoslan)
              Form(
                key: _resetKey,
                autovalidateMode: AutovalidateMode.onUserInteraction,
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: [
                    TextFormField(
                      controller: _kod,
                      keyboardType: TextInputType.number,
                      maxLength: 6,
                      decoration: InputDecoration(labelText: 'Kod iz e-maila', errorText: _greska?.greskaZaPolje('Kod')),
                      validator: (v) => RegExp(r'^\d{6}$').hasMatch(v?.trim() ?? '') ? null : 'Kod se sastoji od tačno 6 cifara, npr. 123456.',
                    ),
                    razmak,
                    TextFormField(controller: _lozinka, obscureText: true, decoration: const InputDecoration(labelText: 'Nova lozinka'), validator: Validatori.lozinka),
                    razmak,
                    TextFormField(
                      controller: _potvrda,
                      obscureText: true,
                      decoration: const InputDecoration(labelText: 'Potvrda nove lozinke'),
                      validator: Validatori.potvrdaLozinke(() => _lozinka.text),
                    ),
                    razmak,
                    FilledButton(onPressed: _salje ? null : _promijeniLozinku, child: const Text('Promijeni lozinku')),
                    TextButton(onPressed: _salje ? null : _posaljiKod, child: const Text('Pošalji novi kod')),
                  ],
                ),
              ),
            if (_greska case final g? when g.greske.isEmpty) ...[razmak, PorukaGreske(g.poruka)],
          ],
        ),
      ),
    );
  }
}
