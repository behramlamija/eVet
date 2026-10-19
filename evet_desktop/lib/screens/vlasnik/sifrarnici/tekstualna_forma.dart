import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';

class PoljeForme {
  const PoljeForme({required this.kljuc, required this.labela, required this.validator, this.pocetnaVrijednost, this.maksimalnoRedova = 1});

  /// Naziv polja u requestu (npr. 'naziv'); koristi se i za greške s backenda.
  final String kljuc;
  final String labela;
  final FormFieldValidator<String> validator;
  final String? pocetnaVrijednost;
  final int maksimalnoRedova;
}

/// Forma s tekstualnim poljima za jednostavne šifrarnike. Vraća true ako je zapis sačuvan.
Future<bool> otvoriTekstualnuFormu(
  BuildContext context, {
  required String naslov,
  required List<PoljeForme> polja,
  required Future<void> Function(Map<String, String?> vrijednosti) snimi,
  required String porukaUspjeha,
  String? napomena,
}) async {
  final rezultat = await showDialog<bool>(
    context: context,
    builder: (_) => _TekstualnaForma(naslov: naslov, polja: polja, snimi: snimi, porukaUspjeha: porukaUspjeha, napomena: napomena),
  );
  return rezultat ?? false;
}

class _TekstualnaForma extends StatefulWidget {
  const _TekstualnaForma({required this.naslov, required this.polja, required this.snimi, required this.porukaUspjeha, this.napomena});

  final String naslov;
  final List<PoljeForme> polja;
  final Future<void> Function(Map<String, String?> vrijednosti) snimi;
  final String porukaUspjeha;
  final String? napomena;

  @override
  State<_TekstualnaForma> createState() => _TekstualnaFormaState();
}

class _TekstualnaFormaState extends State<_TekstualnaForma> {
  final _formKey = GlobalKey<FormState>();
  late final Map<String, TextEditingController> _kontroleri = {
    for (final p in widget.polja) p.kljuc: TextEditingController(text: p.pocetnaVrijednost),
  };
  bool _snima = false;
  ApiException? _greska;

  @override
  void dispose() {
    for (final c in _kontroleri.values) {
      c.dispose();
    }
    super.dispose();
  }

  Future<void> _snimi() async {
    setState(() => _greska = null);
    if (!_formKey.currentState!.validate()) return;

    setState(() => _snima = true);
    try {
      await widget.snimi({
        for (final e in _kontroleri.entries) e.key: e.value.text.trim().isEmpty ? null : e.value.text.trim(),
      });
      if (!mounted) return;
      prikaziUspjeh(context, widget.porukaUspjeha);
      Navigator.pop(context, true);
    } on ApiException catch (e) {
      setState(() => _greska = e);
    } finally {
      if (mounted) setState(() => _snima = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return FormaDijalog(
      naslov: widget.naslov,
      sirina: 460,
      child: Form(
        key: _formKey,
        autovalidateMode: AutovalidateMode.onUserInteraction,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            if (widget.napomena != null) ...[
              Text(widget.napomena!, style: const TextStyle(color: EvetBoje.tekstSivi)),
              const SizedBox(height: 16),
            ],
            for (final p in widget.polja) ...[
              TextFormField(
                controller: _kontroleri[p.kljuc],
                maxLines: p.maksimalnoRedova,
                decoration: InputDecoration(labelText: p.labela, errorText: _greska?.greskaZaPolje(p.kljuc)),
                validator: p.validator,
              ),
              const SizedBox(height: 16),
            ],
            if (_greska case final g? when g.greske.isEmpty) ...[PorukaGreske(g.poruka), const SizedBox(height: 16)],
            FilledButton(onPressed: _snima ? null : _snimi, child: const Text('Sačuvaj')),
          ],
        ),
      ),
    );
  }
}
