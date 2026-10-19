import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Forma za propisivanje ili izmjenu terapije. Vraća true ako je terapija sačuvana.
Future<bool> otvoriFormuTerapije(BuildContext context, {required int ljubimacId, required String ljubimacIme, Terapije? terapija}) async {
  final rezultat = await showDialog<bool>(
    context: context,
    builder: (_) => _TerapijaForma(ljubimacId: ljubimacId, ljubimacIme: ljubimacIme, terapija: terapija),
  );
  return rezultat ?? false;
}

class _TerapijaForma extends StatefulWidget {
  const _TerapijaForma({required this.ljubimacId, required this.ljubimacIme, this.terapija});

  final int ljubimacId;
  final String ljubimacIme;
  final Terapije? terapija;

  @override
  State<_TerapijaForma> createState() => _TerapijaFormaState();
}

class _TerapijaFormaState extends State<_TerapijaForma> {
  final _formKey = GlobalKey<FormState>();
  late final _dijagnoza = TextEditingController(text: widget.terapija?.dijagnoza);
  late final _lijek = TextEditingController(text: widget.terapija?.lijek);
  late final _doza = TextEditingController(text: widget.terapija?.doza);
  late final _trajanje = TextEditingController(text: widget.terapija?.trajanjeDana?.toString());
  late final _napomena = TextEditingController(text: widget.terapija?.napomena);
  late DateTime? _datumKontrolnog = widget.terapija?.datumKontrolnog;
  bool _snima = false;
  String? _greska;

  bool get _uredjivanje => widget.terapija != null;

  @override
  void dispose() {
    for (final c in [_dijagnoza, _lijek, _doza, _trajanje, _napomena]) {
      c.dispose();
    }
    super.dispose();
  }

  String? _opcionalno(TextEditingController c) => c.text.trim().isEmpty ? null : c.text.trim();

  Future<void> _snimi() async {
    setState(() => _greska = null);
    if (!_formKey.currentState!.validate()) return;

    final request = TerapijeUpsertRequest(
      ljubimacId: widget.ljubimacId,
      terminId: widget.terapija?.terminId,
      dijagnoza: _dijagnoza.text.trim(),
      lijek: _opcionalno(_lijek),
      doza: _opcionalno(_doza),
      trajanjeDana: int.tryParse(_trajanje.text.trim()),
      napomena: _opcionalno(_napomena),
      datumKontrolnog: _datumKontrolnog,
    ).toJson();

    setState(() => _snima = true);
    try {
      final terapije = context.read<TerapijeProvider>();
      if (_uredjivanje) {
        await terapije.update(widget.terapija!.terapijaId, request);
      } else {
        await terapije.insert(request);
      }
      if (!mounted) return;
      prikaziUspjeh(context, _uredjivanje ? 'Terapija je izmijenjena.' : 'Terapija za ${widget.ljubimacIme} je uspješno dodana.');
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
      naslov: _uredjivanje ? 'Izmjena terapije - ${widget.ljubimacIme}' : 'Nova terapija - ${widget.ljubimacIme}',
      child: Form(
        key: _formKey,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            TextFormField(controller: _dijagnoza, decoration: const InputDecoration(labelText: 'Dijagnoza'), validator: Validatori.tekst('Dijagnoza', maksimalnaDuzina: 500)),
            const SizedBox(height: 16),
            Row(
              children: [
                Expanded(
                  child: TextFormField(
                    controller: _lijek,
                    decoration: const InputDecoration(labelText: 'Lijek (opcionalno)'),
                    validator: Validatori.tekst('Lijek', obavezan: false),
                  ),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: TextFormField(
                    controller: _doza,
                    decoration: const InputDecoration(labelText: 'Doza (opcionalno)', hintText: 'npr. 250 mg'),
                    validator: Validatori.tekst('Doza', obavezan: false, maksimalnaDuzina: 100),
                  ),
                ),
              ],
            ),
            const SizedBox(height: 16),
            Row(
              children: [
                Expanded(
                  child: TextFormField(
                    controller: _trajanje,
                    decoration: const InputDecoration(labelText: 'Trajanje (opcionalno)', suffixText: 'dana'),
                    validator: (v) {
                      if (v == null || v.trim().isEmpty) return null;
                      final broj = int.tryParse(v.trim());
                      return (broj == null || broj < 1 || broj > 365) ? 'Trajanje mora biti cijeli broj između 1 i 365 dana.' : null;
                    },
                  ),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: InputDecorator(
                    decoration: const InputDecoration(labelText: 'Kontrolni pregled (opcionalno)'),
                    child: Row(
                      children: [
                        Expanded(child: Text(_datumKontrolnog == null ? 'Nije zakazan' : formatDatum(_datumKontrolnog))),
                        IconButton(
                          tooltip: 'Odaberite datum kontrole',
                          icon: const Icon(Icons.calendar_today_outlined),
                          onPressed: () async {
                            final d = await showDatePicker(
                              context: context,
                              initialDate: _datumKontrolnog ?? DateTime.now().add(const Duration(days: 7)),
                              firstDate: DateTime.now(),
                              lastDate: DateTime.now().add(const Duration(days: 365)),
                            );
                            if (d != null) setState(() => _datumKontrolnog = d);
                          },
                        ),
                        if (_datumKontrolnog != null)
                          IconButton(tooltip: 'Ukloni datum', icon: const Icon(Icons.clear), onPressed: () => setState(() => _datumKontrolnog = null)),
                      ],
                    ),
                  ),
                ),
              ],
            ),
            const SizedBox(height: 16),
            TextFormField(
              controller: _napomena,
              maxLines: 3,
              decoration: const InputDecoration(labelText: 'Napomena (opcionalno)'),
              validator: Validatori.tekst('Napomena', obavezan: false, maksimalnaDuzina: 1000),
            ),
            if (_greska != null) ...[const SizedBox(height: 16), PorukaGreske(_greska!)],
            const SizedBox(height: 24),
            FilledButton(onPressed: _snima ? null : _snimi, child: Text(_uredjivanje ? 'Sačuvaj izmjene' : 'Dodaj terapiju')),
          ],
        ),
      ),
    );
  }
}
