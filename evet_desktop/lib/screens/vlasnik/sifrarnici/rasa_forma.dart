import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Forma rase; vrsta ljubimca se bira iz padajuće liste. Ne otvara se ako nema nijedne vrste.
Future<bool> otvoriFormuRase(BuildContext context, Rasa? rasa) async {
  final PagedResult<VrstaLjubimca> vrste;
  try {
    vrste = await context.read<VrsteLjubimacaProvider>().get(pageSize: BaseProvider.maxPageSize);
  } on ApiException catch (e) {
    if (context.mounted) prikaziGresku(context, e.poruka);
    return false;
  }
  if (!context.mounted) return false;
  if (vrste.resultList.isEmpty) {
    prikaziGresku(context, 'Rasu nije moguće dodati: najprije dodajte barem jednu vrstu ljubimca.');
    return false;
  }

  final rezultat = await showDialog<bool>(context: context, builder: (_) => _RasaForma(rasa: rasa, vrste: vrste.resultList));
  return rezultat ?? false;
}

class _RasaForma extends StatefulWidget {
  const _RasaForma({required this.rasa, required this.vrste});

  final Rasa? rasa;
  final List<VrstaLjubimca> vrste;

  @override
  State<_RasaForma> createState() => _RasaFormaState();
}

class _RasaFormaState extends State<_RasaForma> {
  final _formKey = GlobalKey<FormState>();
  late final _naziv = TextEditingController(text: widget.rasa?.naziv);
  late int? _vrstaId = widget.rasa?.vrstaLjubimcaId;
  bool _snima = false;
  String? _greska;

  @override
  void dispose() {
    _naziv.dispose();
    super.dispose();
  }

  Future<void> _snimi() async {
    setState(() => _greska = null);
    if (!_formKey.currentState!.validate()) return;

    final request = RasaUpsertRequest(naziv: _naziv.text.trim(), vrstaLjubimcaId: _vrstaId!).toJson();
    setState(() => _snima = true);
    try {
      final rase = context.read<RaseProvider>();
      if (widget.rasa == null) {
        await rase.insert(request);
      } else {
        await rase.update(widget.rasa!.rasaId, request);
      }
      if (!mounted) return;
      prikaziUspjeh(context, widget.rasa == null ? 'Rasa "${_naziv.text.trim()}" je dodana.' : 'Rasa "${_naziv.text.trim()}" je sačuvana.');
      Navigator.pop(context, true);
    } on ApiException catch (e) {
      setState(() => _greska = e.poruka);
    } finally {
      if (mounted) setState(() => _snima = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return FormaDijalog(
      naslov: widget.rasa == null ? 'Nova rasa' : 'Uređivanje rase',
      sirina: 460,
      child: Form(
        key: _formKey,
        autovalidateMode: AutovalidateMode.onUserInteraction,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            DropdownButtonFormField<int>(
              initialValue: _vrstaId,
              decoration: const InputDecoration(labelText: 'Vrsta ljubimca'),
              items: [for (final v in widget.vrste) DropdownMenuItem(value: v.vrstaLjubimcaId, child: Text(v.naziv))],
              onChanged: (v) => setState(() => _vrstaId = v),
              validator: (v) => Validatori.odabir(v, 'Vrsta ljubimca'),
            ),
            const SizedBox(height: 16),
            TextFormField(controller: _naziv, decoration: const InputDecoration(labelText: 'Naziv rase'), validator: Validatori.tekst('Naziv rase', maksimalnaDuzina: 100)),
            if (_greska != null) ...[const SizedBox(height: 16), PorukaGreske(_greska!)],
            const SizedBox(height: 24),
            FilledButton(onPressed: _snima ? null : _snimi, child: const Text('Sačuvaj')),
          ],
        ),
      ),
    );
  }
}
