import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final _formKey = GlobalKey<FormState>();
  final _korisnickoIme = TextEditingController();
  final _lozinka = TextEditingController();
  bool _ucitava = false;
  bool _sakrijLozinku = true;
  String? _greska;

  @override
  void dispose() {
    _korisnickoIme.dispose();
    _lozinka.dispose();
    super.dispose();
  }

  Future<void> _prijava() async {
    if (!_formKey.currentState!.validate()) return;

    setState(() {
      _ucitava = true;
      _greska = null;
    });

    try {
      await context.read<AuthProvider>().login(_korisnickoIme.text.trim(), _lozinka.text);
    } on ApiException catch (e) {
      if (mounted) setState(() => _greska = e.poruka);
    } finally {
      if (mounted) setState(() => _ucitava = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final porukaOdjave = context.watch<AuthProvider>().porukaOdjave;

    return Scaffold(
      body: Center(
        child: ConstrainedBox(
          constraints: const BoxConstraints(maxWidth: 420),
          child: Card(
            child: Padding(
              padding: const EdgeInsets.all(32),
              child: Form(
                key: _formKey,
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: [
                    const Center(child: EvetLogo(velicina: 34)),
                    const SizedBox(height: 8),
                    const Text('Prijava za vlasnika i veterinare', textAlign: TextAlign.center, style: TextStyle(color: EvetBoje.tekstSivi)),
                    const SizedBox(height: 32),
                    TextFormField(
                      controller: _korisnickoIme,
                      decoration: const InputDecoration(labelText: 'Korisničko ime', prefixIcon: Icon(Icons.person_outline)),
                      textInputAction: TextInputAction.next,
                      validator: (v) => (v == null || v.trim().isEmpty) ? 'Unesite korisničko ime.' : null,
                    ),
                    const SizedBox(height: 16),
                    TextFormField(
                      controller: _lozinka,
                      obscureText: _sakrijLozinku,
                      decoration: InputDecoration(
                        labelText: 'Lozinka',
                        prefixIcon: const Icon(Icons.lock_outline),
                        suffixIcon: IconButton(
                          tooltip: _sakrijLozinku ? 'Prikaži lozinku' : 'Sakrij lozinku',
                          icon: Icon(_sakrijLozinku ? Icons.visibility_outlined : Icons.visibility_off_outlined),
                          onPressed: () => setState(() => _sakrijLozinku = !_sakrijLozinku),
                        ),
                      ),
                      onFieldSubmitted: (_) => _ucitava ? null : _prijava(),
                      validator: (v) => (v == null || v.isEmpty) ? 'Unesite lozinku.' : null,
                    ),
                    const SizedBox(height: 16),
                    if (_greska ?? porukaOdjave case final poruka?) ...[
                      PorukaGreske(poruka),
                      const SizedBox(height: 16),
                    ],
                    FilledButton(
                      onPressed: _ucitava ? null : _prijava,
                      child: _ucitava
                          ? const SizedBox(width: 22, height: 22, child: CircularProgressIndicator(strokeWidth: 2, color: Colors.white))
                          : const Text('Prijavi se'),
                    ),
                  ],
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }
}
