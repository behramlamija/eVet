import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Otvara formu za dodavanje ili uređivanje pacijenta. Vraća true ako je zapis sačuvan.
/// Forma se ne otvara ako u sistemu nema klijenata kojima bi pacijent pripadao.
Future<bool> otvoriFormuPacijenta(BuildContext context, {Ljubimci? pacijent}) async {
  // Oba zahtjeva kreću istovremeno, a čeka se na oba.
  final klijentiZahtjev = context.read<KlijentiProvider>().get(pageSize: BaseProvider.maxPageSize);
  final vrsteZahtjev = context.read<VrsteLjubimacaProvider>().get(pageSize: BaseProvider.maxPageSize);
  final PagedResult<KorisnikOsnovno> klijenti;
  final PagedResult<VrstaLjubimca> vrste;
  try {
    klijenti = await klijentiZahtjev;
    vrste = await vrsteZahtjev;
  } on ApiException catch (e) {
    if (context.mounted) prikaziGresku(context, e.poruka);
    return false;
  }

  if (!context.mounted) return false;
  if (klijenti.resultList.isEmpty || vrste.resultList.isEmpty) {
    prikaziGresku(context, 'Pacijenta nije moguće dodati: u sistemu nema registrovanih klijenata ili vrsta ljubimaca.');
    return false;
  }

  final rezultat = await showDialog<bool>(
    context: context,
    builder: (_) => _PacijentForma(pacijent: pacijent, klijenti: klijenti.resultList, vrste: vrste.resultList),
  );
  return rezultat ?? false;
}

class _PacijentForma extends StatefulWidget {
  const _PacijentForma({required this.pacijent, required this.klijenti, required this.vrste});

  final Ljubimci? pacijent;
  final List<KorisnikOsnovno> klijenti;
  final List<VrstaLjubimca> vrste;

  @override
  State<_PacijentForma> createState() => _PacijentFormaState();
}

class _PacijentFormaState extends State<_PacijentForma> {
  final _formKey = GlobalKey<FormState>();
  late final _ime = TextEditingController(text: widget.pacijent?.ime);
  late final _tezina = TextEditingController(text: widget.pacijent?.tezina?.toString().replaceAll('.', ','));
  late final _napomena = TextEditingController(text: widget.pacijent?.napomena);
  late int? _vlasnikId = widget.pacijent?.korisnikId;
  late int? _vrstaId = widget.pacijent?.vrstaLjubimcaId;
  late int? _rasaId = widget.pacijent?.rasaId;
  late Spol? _spol = widget.pacijent?.spol;
  late DateTime? _datumRodjenja = widget.pacijent?.datumRodjenja;
  List<Rasa> _rase = const [];
  bool _snima = false;
  ApiException? _greska;

  bool get _uredjivanje => widget.pacijent != null;

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
    final rase = await context.read<RaseProvider>().get(filter: {'VrstaLjubimcaId': vrstaId}, pageSize: BaseProvider.maxPageSize);
    if (mounted) setState(() => _rase = rase.resultList);
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
      vlasnikId: _vlasnikId,
    ).toJson();

    setState(() => _snima = true);
    try {
      final ljubimci = context.read<LjubimciProvider>();
      if (_uredjivanje) {
        await ljubimci.update(widget.pacijent!.ljubimacId, request);
      } else {
        await ljubimci.insert(request);
      }
      if (!mounted) return;
      prikaziUspjeh(context, _uredjivanje ? 'Podaci pacijenta ${_ime.text.trim()} su sačuvani.' : 'Pacijent ${_ime.text.trim()} je uspješno dodan.');
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
      naslov: _uredjivanje ? 'Uređivanje pacijenta' : 'Novi pacijent',
      child: Form(
        key: _formKey,
        autovalidateMode: AutovalidateMode.onUserInteraction,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            TextFormField(
              controller: _ime,
              decoration: InputDecoration(labelText: 'Ime ljubimca', errorText: _greska?.greskaZaPolje('Ime')),
              validator: Validatori.tekst('Ime ljubimca', maksimalnaDuzina: 100),
            ),
            const SizedBox(height: 16),
            DropdownButtonFormField<int>(
              initialValue: _vlasnikId,
              decoration: const InputDecoration(labelText: 'Vlasnik'),
              items: [
                for (final k in widget.klijenti) DropdownMenuItem(value: k.korisnikId, child: Text('${k.ime} ${k.prezime} (${k.email})')),
              ],
              onChanged: (v) => setState(() => _vlasnikId = v),
              validator: (v) => Validatori.odabir(v, 'Vlasnik'),
            ),
            const SizedBox(height: 16),
            Row(
              children: [
                Expanded(
                  child: DropdownButtonFormField<int>(
                    initialValue: _vrstaId,
                    decoration: const InputDecoration(labelText: 'Vrsta'),
                    items: [for (final v in widget.vrste) DropdownMenuItem(value: v.vrstaLjubimcaId, child: Text(v.naziv))],
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
                const SizedBox(width: 12),
                Expanded(
                  child: DropdownButtonFormField<int>(
                    key: ValueKey(_vrstaId),
                    initialValue: _rase.any((r) => r.rasaId == _rasaId) ? _rasaId : null,
                    decoration: InputDecoration(
                      labelText: 'Rasa',
                      helperText: _vrstaId == null ? 'Najprije odaberite vrstu.' : null,
                    ),
                    items: [for (final r in _rase) DropdownMenuItem(value: r.rasaId, child: Text(r.naziv))],
                    onChanged: _vrstaId == null ? null : (v) => setState(() => _rasaId = v),
                    validator: (v) => Validatori.odabir(v, 'Rasa'),
                  ),
                ),
              ],
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
            Row(
              children: [
                Expanded(
                  child: InputDecorator(
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
                        if (_datumRodjenja != null)
                          IconButton(tooltip: 'Ukloni datum', icon: const Icon(Icons.clear), onPressed: () => setState(() => _datumRodjenja = null)),
                      ],
                    ),
                  ),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: TextFormField(
                    controller: _tezina,
                    decoration: const InputDecoration(labelText: 'Težina u kg (opcionalno)', suffixText: 'kg'),
                    validator: Validatori.broj('Težina', min: 0.01, max: 200, obavezan: false, jedinica: ' kg'),
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
            if (_greska case final g? when g.greske.isEmpty || g.greskaZaPolje('Ime') == null) ...[const SizedBox(height: 16), PorukaGreske(g.poruka)],
            const SizedBox(height: 24),
            FilledButton(onPressed: _snima ? null : _snimi, child: Text(_uredjivanje ? 'Sačuvaj izmjene' : 'Dodaj pacijenta')),
          ],
        ),
      ),
    );
  }
}
