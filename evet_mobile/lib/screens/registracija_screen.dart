import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Registracija klijenta. Validacija koristi ista pravila kao backend (Validatori u evet_shared).
class RegistracijaScreen extends StatefulWidget {
  const RegistracijaScreen({super.key});

  @override
  State<RegistracijaScreen> createState() => _RegistracijaScreenState();
}

class _RegistracijaScreenState extends State<RegistracijaScreen> {
  final _formKey = GlobalKey<FormState>();
  final _ime = TextEditingController();
  final _prezime = TextEditingController();
  final _korisnickoIme = TextEditingController();
  final _email = TextEditingController();
  final _telefon = TextEditingController();
  final _lozinka = TextEditingController();
  final _potvrda = TextEditingController();
  bool _ucitava = false;
  ApiException? _greska;

  @override
  void dispose() {
    for (final c in [_ime, _prezime, _korisnickoIme, _email, _telefon, _lozinka, _potvrda]) {
      c.dispose();
    }
    super.dispose();
  }

  Future<void> _registruj() async {
    setState(() => _greska = null);
    if (!_formKey.currentState!.validate()) return;

    setState(() => _ucitava = true);
    try {
      await context.read<AuthProvider>().registracija(RegistracijaRequest(
            ime: _ime.text.trim(),
            prezime: _prezime.text.trim(),
            korisnickoIme: _korisnickoIme.text.trim(),
            email: _email.text.trim(),
            telefon: _telefon.text.trim().isEmpty ? null : _telefon.text.trim(),
            lozinka: _lozinka.text,
            lozinkaPotvrda: _potvrda.text,
          ));
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Račun je uspješno kreiran. Prijavite se svojim podacima.')),
      );
      Navigator.of(context).pop(_korisnickoIme.text.trim());
    } on ApiException catch (e) {
      if (mounted) setState(() => _greska = e);
    } finally {
      if (mounted) setState(() => _ucitava = false);
    }
  }

  Widget _polje(TextEditingController controller, String label, String poljeApi, FormFieldValidator<String> validator,
      {bool lozinka = false, TextInputType? tip}) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 16),
      child: TextFormField(
        controller: controller,
        obscureText: lozinka,
        keyboardType: tip,
        decoration: InputDecoration(labelText: label, errorText: _greska?.greskaZaPolje(poljeApi)),
        validator: validator,
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Registracija')),
      body: SafeArea(
        child: Form(
          key: _formKey,
          child: ListView(
            padding: const EdgeInsets.all(24),
            children: [
              _polje(_ime, 'Ime', 'Ime', Validatori.tekst('Ime', maksimalnaDuzina: 100)),
              _polje(_prezime, 'Prezime', 'Prezime', Validatori.tekst('Prezime', maksimalnaDuzina: 100)),
              _polje(_korisnickoIme, 'Korisničko ime', 'KorisnickoIme', Validatori.korisnickoIme),
              _polje(_email, 'Email', 'Email', Validatori.email, tip: TextInputType.emailAddress),
              _polje(_telefon, 'Telefon (opcionalno)', 'Telefon', Validatori.telefonOpcionalno, tip: TextInputType.phone),
              _polje(_lozinka, 'Lozinka', 'Lozinka', Validatori.lozinka, lozinka: true),
              _polje(_potvrda, 'Potvrda lozinke', 'LozinkaPotvrda', Validatori.potvrdaLozinke(() => _lozinka.text), lozinka: true),
              if (_greska case final greska? when greska.greske.isEmpty) ...[
                PorukaGreske(greska.poruka),
                const SizedBox(height: 16),
              ],
              FilledButton(
                onPressed: _ucitava ? null : _registruj,
                child: _ucitava
                    ? const SizedBox(width: 22, height: 22, child: CircularProgressIndicator(strokeWidth: 2, color: Colors.white))
                    : const Text('Kreiraj račun'),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
