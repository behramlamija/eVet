import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Dodavanje ili uređivanje usluge. Ne otvara se ako ne postoji nijedna vrsta usluge.
Future<bool> otvoriFormuUsluge(BuildContext context, {Usluge? usluga}) async {
  final PagedResult<VrstaUsluge> vrste;
  try {
    vrste = await context.read<VrsteUslugaProvider>().get(pageSize: BaseProvider.maxPageSize);
  } on ApiException catch (e) {
    if (context.mounted) prikaziGresku(context, e.poruka);
    return false;
  }
  if (!context.mounted) return false;
  if (vrste.resultList.isEmpty) {
    prikaziGresku(context, 'Uslugu nije moguće dodati: najprije dodajte vrstu usluge u Šifrarnicima.');
    return false;
  }

  final rezultat = await showDialog<bool>(context: context, builder: (_) => _UslugaForma(usluga: usluga, vrste: vrste.resultList));
  return rezultat ?? false;
}

class _UslugaForma extends StatefulWidget {
  const _UslugaForma({required this.usluga, required this.vrste});

  final Usluge? usluga;
  final List<VrstaUsluge> vrste;

  @override
  State<_UslugaForma> createState() => _UslugaFormaState();
}

class _UslugaFormaState extends State<_UslugaForma> {
  final _formKey = GlobalKey<FormState>();
  late final _naziv = TextEditingController(text: widget.usluga?.naziv);
  late final _opis = TextEditingController(text: widget.usluga?.opis);
  late final _cijena = TextEditingController(text: widget.usluga?.cijena.toStringAsFixed(2).replaceAll('.', ','));
  late final _trajanje = TextEditingController(text: widget.usluga?.trajanje.toString());
  late int? _vrstaId = widget.usluga?.vrstaId;
  OdabranaSlika? _slika;
  bool _snima = false;
  ApiException? _greska;

  bool get _uredjivanje => widget.usluga != null;

  @override
  void dispose() {
    for (final c in [_naziv, _opis, _cijena, _trajanje]) {
      c.dispose();
    }
    super.dispose();
  }

  Future<void> _odaberiSliku() async {
    try {
      final slika = await odaberiSliku();
      if (slika != null) setState(() => _slika = slika);
    } on ApiException catch (e) {
      setState(() => _greska = e);
    }
  }

  Future<void> _snimi() async {
    setState(() => _greska = null);
    if (!_formKey.currentState!.validate()) return;

    final request = UslugeUpsertRequest(
      naziv: _naziv.text.trim(),
      opis: _opis.text.trim().isEmpty ? null : _opis.text.trim(),
      cijena: Validatori.parsirajBroj(_cijena.text)!,
      trajanje: int.parse(_trajanje.text.trim()),
      vrstaId: _vrstaId!,
    ).toJson();

    setState(() => _snima = true);
    try {
      final usluge = context.read<UslugeProvider>();
      final sacuvana = _uredjivanje ? await usluge.update(widget.usluga!.uslugaId, request) : await usluge.insert(request);
      if (_slika case final slika?) {
        if (!mounted) return;
        await context.read<SlikeProvider>().postavi(TipSlike.usluga, sacuvana.uslugaId, bytes: slika.bytes, nazivDatoteke: slika.naziv, contentType: slika.contentType);
      }
      if (!mounted) return;
      prikaziUspjeh(context, _uredjivanje ? 'Usluga "${sacuvana.naziv}" je sačuvana.' : 'Usluga "${sacuvana.naziv}" je dodana u katalog.');
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
      naslov: _uredjivanje ? 'Uređivanje usluge' : 'Nova usluga',
      child: Form(
        key: _formKey,
        autovalidateMode: AutovalidateMode.onUserInteraction,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            TextFormField(
              controller: _naziv,
              decoration: InputDecoration(labelText: 'Naziv', errorText: _greska?.greskaZaPolje('Naziv')),
              validator: Validatori.tekst('Naziv'),
            ),
            const SizedBox(height: 16),
            DropdownButtonFormField<int>(
              initialValue: _vrstaId,
              decoration: const InputDecoration(labelText: 'Vrsta usluge'),
              items: [for (final v in widget.vrste) DropdownMenuItem(value: v.vrstaId, child: Text(v.naziv))],
              onChanged: (v) => setState(() => _vrstaId = v),
              validator: (v) => Validatori.odabir(v, 'Vrsta usluge'),
            ),
            const SizedBox(height: 16),
            Row(
              children: [
                Expanded(
                  child: TextFormField(
                    controller: _cijena,
                    decoration: const InputDecoration(labelText: 'Cijena', suffixText: 'KM'),
                    validator: Validatori.broj('Cijena', min: 0.01, max: 100000, jedinica: ' KM'),
                  ),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: TextFormField(
                    controller: _trajanje,
                    decoration: const InputDecoration(labelText: 'Trajanje', suffixText: 'min'),
                    validator: (v) {
                      final broj = int.tryParse(v?.trim() ?? '');
                      return (broj == null || broj < 5 || broj > 480) ? 'Trajanje mora biti cijeli broj između 5 i 480 minuta.' : null;
                    },
                  ),
                ),
              ],
            ),
            const SizedBox(height: 16),
            TextFormField(
              controller: _opis,
              maxLines: 3,
              decoration: const InputDecoration(labelText: 'Opis (opcionalno)'),
              validator: Validatori.tekst('Opis', obavezan: false, maksimalnaDuzina: 2000),
            ),
            const SizedBox(height: 16),
            Row(
              children: [
                OutlinedButton.icon(onPressed: _odaberiSliku, icon: const Icon(Icons.image_outlined), label: const Text('Odaberi sliku')),
                const SizedBox(width: 12),
                Expanded(
                  child: Text(
                    _slika?.naziv ?? (widget.usluga?.imaSliku == true ? 'Slika je postavljena.' : 'Slika nije odabrana (opcionalno).'),
                    overflow: TextOverflow.ellipsis,
                    style: const TextStyle(color: EvetBoje.tekstSivi),
                  ),
                ),
              ],
            ),
            if (_greska case final g? when g.greske.isEmpty) ...[const SizedBox(height: 16), PorukaGreske(g.poruka)],
            const SizedBox(height: 24),
            FilledButton(onPressed: _snima ? null : _snimi, child: Text(_uredjivanje ? 'Sačuvaj izmjene' : 'Dodaj uslugu')),
          ],
        ),
      ),
    );
  }
}
