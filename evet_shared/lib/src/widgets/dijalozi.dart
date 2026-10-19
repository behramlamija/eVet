import 'package:flutter/material.dart';

import '../api/api_exception.dart';
import '../utils/validatori.dart';

/// Potvrda nepovratne akcije (brisanje, otkazivanje, plaćanje).
Future<bool> potvrdiAkciju(
  BuildContext context, {
  required String naslov,
  required String poruka,
  String potvrda = 'Potvrdi',
  bool opasna = false,
}) async {
  final rezultat = await showDialog<bool>(
    context: context,
    builder: (context) => AlertDialog(
      title: Text(naslov),
      content: Text(poruka),
      actions: [
        TextButton(onPressed: () => Navigator.pop(context, false), child: const Text('Odustani')),
        FilledButton(
          style: opasna ? FilledButton.styleFrom(backgroundColor: Theme.of(context).colorScheme.error) : null,
          onPressed: () => Navigator.pop(context, true),
          child: Text(potvrda),
        ),
      ],
    ),
  );
  return rezultat ?? false;
}

/// Unos obaveznog teksta, npr. razloga odbijanja ili odgovora na recenziju; vraća null ako korisnik odustane.
Future<String?> unesiRazlog(BuildContext context, {required String naslov, required String potvrda, String labela = 'Razlog'}) {
  return showDialog<String>(context: context, builder: (_) => _RazlogDijalog(naslov: naslov, potvrda: potvrda, labela: labela));
}

class _RazlogDijalog extends StatefulWidget {
  const _RazlogDijalog({required this.naslov, required this.potvrda, required this.labela});

  final String naslov;
  final String potvrda;
  final String labela;

  @override
  State<_RazlogDijalog> createState() => _RazlogDijalogState();
}

class _RazlogDijalogState extends State<_RazlogDijalog> {
  final _formKey = GlobalKey<FormState>();
  final _razlog = TextEditingController();

  @override
  void dispose() {
    _razlog.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      title: Text(widget.naslov),
      content: SizedBox(
        width: 420,
        child: Form(
          key: _formKey,
          child: TextFormField(
            controller: _razlog,
            autofocus: true,
            maxLines: 3,
            decoration: InputDecoration(labelText: widget.labela),
            validator: Validatori.tekst(widget.labela, maksimalnaDuzina: 500),
          ),
        ),
      ),
      actions: [
        TextButton(onPressed: () => Navigator.pop(context), child: const Text('Odustani')),
        FilledButton(
          onPressed: () {
            if (_formKey.currentState!.validate()) Navigator.pop(context, _razlog.text.trim());
          },
          child: Text(widget.potvrda),
        ),
      ],
    );
  }
}

/// Poruka o uspješnoj akciji s konkretnim opisom (npr. "Pacijent je uspješno dodan.").
void prikaziUspjeh(BuildContext context, String poruka) {
  ScaffoldMessenger.of(context).showSnackBar(SnackBar(content: Text(poruka), behavior: SnackBarBehavior.floating));
}

void prikaziGresku(BuildContext context, String poruka) {
  ScaffoldMessenger.of(context).showSnackBar(SnackBar(
    content: Text(poruka),
    behavior: SnackBarBehavior.floating,
    backgroundColor: Theme.of(context).colorScheme.error,
  ));
}

/// Izvršava akciju nad API-jem i prikazuje poruku o uspjehu ili poruku greške sa servera.
Future<bool> izvrsiAkciju(BuildContext context, Future<void> Function() akcija, {required String uspjeh}) async {
  try {
    await akcija();
    if (context.mounted) prikaziUspjeh(context, uspjeh);
    return true;
  } on ApiException catch (e) {
    if (context.mounted) prikaziGresku(context, e.poruka);
    return false;
  }
}

/// Okvir forme u dijalogu, s dugmetom "X" za zatvaranje u gornjem desnom uglu.
class FormaDijalog extends StatelessWidget {
  const FormaDijalog({super.key, required this.naslov, required this.child, this.sirina = 560});

  final String naslov;
  final Widget child;
  final double sirina;

  @override
  Widget build(BuildContext context) {
    return Dialog(
      child: ConstrainedBox(
        constraints: BoxConstraints(maxWidth: sirina, maxHeight: MediaQuery.sizeOf(context).height * 0.9),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            Padding(
              padding: const EdgeInsets.fromLTRB(24, 16, 8, 0),
              child: Row(
                children: [
                  Expanded(child: Text(naslov, style: Theme.of(context).textTheme.titleLarge)),
                  IconButton(tooltip: 'Zatvori', icon: const Icon(Icons.close), onPressed: () => Navigator.pop(context)),
                ],
              ),
            ),
            Flexible(child: SingleChildScrollView(padding: const EdgeInsets.all(24), child: child)),
          ],
        ),
      ),
    );
  }
}
