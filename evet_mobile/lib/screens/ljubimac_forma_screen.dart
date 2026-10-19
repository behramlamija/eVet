import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Dodavanje ili uređivanje ljubimca klijenta. Vraća true ako je ljubimac sačuvan ili obrisan.
class LjubimacFormaScreen extends StatefulWidget {
  const LjubimacFormaScreen({super.key, this.ljubimac});

  final Ljubimci? ljubimac;

  @override
  State<LjubimacFormaScreen> createState() => _LjubimacFormaScreenState();
}

class _LjubimacFormaScreenState extends State<LjubimacFormaScreen> {
  final _formKey = GlobalKey<FormState>();
  late final Ljubimci? _l = widget.ljubimac;
  late final _ime = TextEditingController(text: _l?.ime);
  late final _tezina = TextEditingController(text: _l?.tezina?.toString().replaceAll('.', ','));
  late final _napomena = TextEditingController(text: _l?.napomena);
  late int? _vrstaId = _l?.vrstaLjubimcaId;
  late int? _rasaId = _l?.rasaId;
  late Spol? _spol = _l?.spol;
  late DateTime? _datumRodjenja = _l?.datumRodjenja;
  late final Future<PagedResult<VrstaLjubimca>> _vrste = context.read<VrsteLjubimacaProvider>().get(pageSize: BaseProvider.maxPageSize);
  List<Rasa> _rase = const [];
  OdabranaSlika? _slika;
  bool _snima = false;
  ApiException? _greska;

  bool get _uredjivanje => _l != null;

  @override
  void initState() {
    super.initState();
    if (_vrstaId != null) _ucitajRase(_vrstaId!);
  }

  @override
  void dispose() {
    _ime.dispose();
    _tezina.dispose();
    _napomena.dispose();
    super.dispose();
  }

  Future<void> _ucitajRase(int vrstaId) async {
    try {
      final rase = await context.read<RaseProvider>().get(filter: {'VrstaLjubimcaId': vrstaId}, pageSize: BaseProvider.maxPageSize);
      if (mounted) setState(() => _rase = rase.resultList);
    } on ApiException catch (e) {
      if (mounted) setState(() => _greska = e);
    }
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

    final request = LjubimciUpsertRequest(
      ime: _ime.text.trim(),
      rasaId: _rasaId!,
      spol: _spol!,
      datumRodjenja: _datumRodjenja,
      tezina: Validatori.parsirajBroj(_tezina.text),
      napomena: _napomena.text.trim().isEmpty ? null : _napomena.text.trim(),
    ).toJson();

    setState(() => _snima = true);
    try {
      final ljubimci = context.read<LjubimciProvider>();
      final sacuvan = _uredjivanje ? await ljubimci.update(_l!.ljubimacId, request) : await ljubimci.insert(request);
      if (_slika case final slika?) {
        if (!mounted) return;
        await context.read<SlikeProvider>().postavi(TipSlike.ljubimac, sacuvan.ljubimacId, bytes: slika.bytes, nazivDatoteke: slika.naziv, contentType: slika.contentType);
      }
      if (!mounted) return;
      prikaziUspjeh(context, _uredjivanje ? 'Podaci ljubimca ${sacuvan.ime} su sačuvani.' : 'Ljubimac ${sacuvan.ime} je uspješno dodan.');
      Navigator.pop(context, true);
    } on ApiException catch (e) {
      setState(() => _greska = e);
    } finally {
      if (mounted) setState(() => _snima = false);
    }
  }

  Future<void> _obrisi() async {
    final potvrda = await potvrdiAkciju(context, naslov: 'Brisanje ljubimca', poruka: 'Obrisati ljubimca ${_l!.ime}?', potvrda: 'Obriši', opasna: true);
    if (!potvrda || !mounted) return;
    if (await izvrsiAkciju(context, () => context.read<LjubimciProvider>().delete(_l.ljubimacId), uspjeh: 'Ljubimac ${_l.ime} je obrisan.') && mounted) {
      Navigator.pop(context, true);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(_uredjivanje ? 'Uređivanje ljubimca' : 'Novi ljubimac'),
        actions: [if (_uredjivanje) IconButton(tooltip: 'Obriši ljubimca', onPressed: _obrisi, icon: const Icon(Icons.delete_outline))],
      ),
      body: Form(
        key: _formKey,
        autovalidateMode: AutovalidateMode.onUserInteraction,
        child: ListView(
          padding: const EdgeInsets.all(16),
          children: [
            Center(
              child: Column(
                children: [
                  if (_uredjivanje && _slika == null)
                    EvetSlika(slike: context.read<SlikeProvider>(), tip: TipSlike.ljubimac, id: _l!.ljubimacId, imaSliku: _l.imaSliku, velicina: 96)
                  else
                    CircleAvatar(radius: 48, child: Icon(_slika == null ? Icons.pets : Icons.check, size: 40)),
                  TextButton.icon(onPressed: _odaberiSliku, icon: const Icon(Icons.photo_camera_outlined), label: Text(_slika?.naziv ?? 'Odaberi sliku (opcionalno)')),
                ],
              ),
            ),
            TextFormField(
              controller: _ime,
              decoration: InputDecoration(labelText: 'Ime ljubimca', errorText: _greska?.greskaZaPolje('Ime')),
              validator: Validatori.tekst('Ime ljubimca', maksimalnaDuzina: 100),
            ),
            const SizedBox(height: 16),
            FutureBuilder<PagedResult<VrstaLjubimca>>(
              future: _vrste,
              builder: (context, s) => DropdownButtonFormField<int>(
                initialValue: _vrstaId,
                decoration: const InputDecoration(labelText: 'Vrsta'),
                items: [for (final v in s.data?.resultList ?? const <VrstaLjubimca>[]) DropdownMenuItem(value: v.vrstaLjubimcaId, child: Text(v.naziv))],
                onChanged: (v) {
                  setState(() {
                    _vrstaId = v;
                    _rasaId = null;
                    _rase = const [];
                  });
                  if (v != null) _ucitajRase(v);
                },
                validator: (v) => Validatori.odabir(v, 'Vrsta'),
              ),
            ),
            const SizedBox(height: 16),
            DropdownButtonFormField<int>(
              key: ValueKey(_vrstaId),
              initialValue: _rase.any((r) => r.rasaId == _rasaId) ? _rasaId : null,
              decoration: InputDecoration(labelText: 'Rasa', helperText: _vrstaId == null ? 'Najprije odaberite vrstu.' : null),
              items: [for (final r in _rase) DropdownMenuItem(value: r.rasaId, child: Text(r.naziv))],
              onChanged: _vrstaId == null ? null : (v) => setState(() => _rasaId = v),
              validator: (v) => Validatori.odabir(v, 'Rasa'),
            ),
            const SizedBox(height: 16),
            FormField<Spol>(
              initialValue: _spol,
              validator: (_) => Validatori.odabir(_spol, 'Spol'),
              builder: (state) => InputDecorator(
                decoration: InputDecoration(labelText: 'Spol', errorText: state.errorText, border: InputBorder.none, filled: false),
                child: SegmentedButton<Spol>(
                  emptySelectionAllowed: true,
                  segments: [for (final s in Spol.values) ButtonSegment(value: s, label: Text(nazivSpola(s)))],
                  selected: {?_spol},
                  onSelectionChanged: (s) {
                    setState(() => _spol = s.firstOrNull);
                    state.didChange(_spol);
                  },
                ),
              ),
            ),
            const SizedBox(height: 8),
            InputDecorator(
              decoration: const InputDecoration(labelText: 'Datum rođenja (opcionalno)'),
              child: Row(
                children: [
                  Expanded(child: Text(_datumRodjenja == null ? 'Nije unesen' : formatDatum(_datumRodjenja))),
                  IconButton(
                    tooltip: 'Odaberite datum rođenja',
                    icon: const Icon(Icons.calendar_today_outlined),
                    onPressed: () async {
                      final d = await showDatePicker(context: context, initialDate: _datumRodjenja ?? DateTime.now(), firstDate: DateTime(1990), lastDate: DateTime.now());
                      if (d != null) setState(() => _datumRodjenja = d);
                    },
                  ),
                ],
              ),
            ),
            const SizedBox(height: 16),
            TextFormField(
              controller: _tezina,
              keyboardType: const TextInputType.numberWithOptions(decimal: true),
              decoration: const InputDecoration(labelText: 'Težina (opcionalno)', suffixText: 'kg'),
              validator: Validatori.broj('Težina', min: 0.01, max: 200, obavezan: false, jedinica: ' kg'),
            ),
            const SizedBox(height: 16),
            TextFormField(
              controller: _napomena,
              maxLines: 3,
              decoration: const InputDecoration(labelText: 'Napomena (opcionalno)', hintText: 'npr. alergije, posebne potrebe'),
              validator: Validatori.tekst('Napomena', obavezan: false, maksimalnaDuzina: 1000),
            ),
            if (_greska case final g? when g.greske.isEmpty || g.greskaZaPolje('Ime') == null) ...[const SizedBox(height: 16), PorukaGreske(g.poruka)],
            const SizedBox(height: 24),
            FilledButton(onPressed: _snima ? null : _snimi, child: Text(_uredjivanje ? 'Sačuvaj izmjene' : 'Dodaj ljubimca')),
          ],
        ),
      ),
    );
  }
}
