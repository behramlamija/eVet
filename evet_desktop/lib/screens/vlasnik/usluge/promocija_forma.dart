import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Dodavanje ili uređivanje promocije. Ne otvara se ako u katalogu nema usluga.
Future<bool> otvoriFormuPromocije(BuildContext context, {Promocije? promocija}) async {
  final PagedResult<Usluge> usluge;
  try {
    usluge = await context.read<UslugeProvider>().get(pageSize: BaseProvider.maxPageSize);
  } on ApiException catch (e) {
    if (context.mounted) prikaziGresku(context, e.poruka);
    return false;
  }
  if (!context.mounted) return false;
  if (usluge.resultList.isEmpty) {
    prikaziGresku(context, 'Promociju nije moguće dodati: u katalogu nema usluga.');
    return false;
  }

  final rezultat = await showDialog<bool>(context: context, builder: (_) => _PromocijaForma(promocija: promocija, usluge: usluge.resultList));
  return rezultat ?? false;
}

class _PromocijaForma extends StatefulWidget {
  const _PromocijaForma({required this.promocija, required this.usluge});

  final Promocije? promocija;
  final List<Usluge> usluge;

  @override
  State<_PromocijaForma> createState() => _PromocijaFormaState();
}

class _PromocijaFormaState extends State<_PromocijaForma> {
  final _formKey = GlobalKey<FormState>();
  late final _naziv = TextEditingController(text: widget.promocija?.naziv);
  late final _opis = TextEditingController(text: widget.promocija?.opis);
  late final _kod = TextEditingController(text: widget.promocija?.kod);
  late final _popust = TextEditingController(text: widget.promocija?.popust.round().toString());
  late int? _uslugaId = widget.promocija?.uslugaId;
  late DateTime? _pocetak = widget.promocija?.datumPocetka;
  late DateTime? _kraj = widget.promocija?.datumKraja;
  late bool _jeAktivna = widget.promocija?.jeAktivna ?? true;
  bool _snima = false;
  ApiException? _greska;

  bool get _uredjivanje => widget.promocija != null;

  @override
  void dispose() {
    for (final c in [_naziv, _opis, _kod, _popust]) {
      c.dispose();
    }
    super.dispose();
  }

  Future<void> _snimi() async {
    setState(() => _greska = null);
    if (!_formKey.currentState!.validate()) return;

    final request = PromocijeUpsertRequest(
      naziv: _naziv.text.trim(),
      opis: _opis.text.trim().isEmpty ? null : _opis.text.trim(),
      kod: _kod.text.trim(),
      popust: Validatori.parsirajBroj(_popust.text)!,
      // Promocija važi od početka prvog do kraja posljednjeg odabranog dana.
      datumPocetka: DateTime(_pocetak!.year, _pocetak!.month, _pocetak!.day).toUtc(),
      datumKraja: DateTime(_kraj!.year, _kraj!.month, _kraj!.day, 23, 59, 59).toUtc(),
      uslugaId: _uslugaId!,
      jeAktivna: _jeAktivna,
    ).toJson();

    setState(() => _snima = true);
    try {
      final promocije = context.read<PromocijeProvider>();
      if (_uredjivanje) {
        await promocije.update(widget.promocija!.promocijaId, request);
      } else {
        await promocije.insert(request);
      }
      if (!mounted) return;
      prikaziUspjeh(context, _uredjivanje ? 'Promocija "${_naziv.text.trim()}" je sačuvana.' : 'Promocija "${_naziv.text.trim()}" je dodana.');
      Navigator.pop(context, true);
    } on ApiException catch (e) {
      setState(() => _greska = e);
    } finally {
      if (mounted) setState(() => _snima = false);
    }
  }

  Widget _datum(String labela, DateTime? vrijednost, ValueChanged<DateTime> promjena, String? Function() validator) {
    return FormField<DateTime>(
      validator: (_) => validator(),
      builder: (state) => InputDecorator(
        decoration: InputDecoration(labelText: labela, errorText: state.errorText),
        child: Row(
          children: [
            Expanded(child: Text(vrijednost == null ? 'Odaberite datum' : formatDatum(vrijednost))),
            IconButton(
              tooltip: 'Odaberite datum',
              icon: const Icon(Icons.calendar_today_outlined),
              onPressed: () async {
                final d = await showDatePicker(
                  context: context,
                  initialDate: vrijednost ?? DateTime.now(),
                  firstDate: DateTime(2020),
                  lastDate: DateTime.now().add(const Duration(days: 730)),
                );
                if (d != null) {
                  promjena(d);
                  state.didChange(d);
                }
              },
            ),
          ],
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return FormaDijalog(
      naslov: _uredjivanje ? 'Uređivanje promocije' : 'Nova promocija',
      child: Form(
        key: _formKey,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            TextFormField(controller: _naziv, decoration: const InputDecoration(labelText: 'Naziv'), validator: Validatori.tekst('Naziv')),
            const SizedBox(height: 16),
            Row(
              children: [
                Expanded(
                  child: TextFormField(
                    controller: _kod,
                    textCapitalization: TextCapitalization.characters,
                    decoration: InputDecoration(labelText: 'Kod kupona', hintText: 'npr. LJETO-2026', errorText: _greska?.greskaZaPolje('Kod')),
                    validator: Validatori.kodPromocije,
                  ),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: TextFormField(
                    controller: _popust,
                    decoration: const InputDecoration(labelText: 'Popust', suffixText: '%'),
                    validator: Validatori.broj('Popust', min: 1, max: 100, jedinica: ' %'),
                  ),
                ),
              ],
            ),
            const SizedBox(height: 16),
            DropdownButtonFormField<int>(
              initialValue: _uslugaId,
              decoration: const InputDecoration(labelText: 'Usluga na koju se odnosi popust'),
              items: [for (final u in widget.usluge) DropdownMenuItem(value: u.uslugaId, child: Text('${u.naziv} (${formatNovac(u.cijena)})'))],
              onChanged: (v) => setState(() => _uslugaId = v),
              validator: (v) => Validatori.odabir(v, 'Usluga'),
            ),
            const SizedBox(height: 16),
            Row(
              children: [
                Expanded(child: _datum('Početak', _pocetak, (d) => setState(() => _pocetak = d), () => _pocetak == null ? 'Odaberite datum početka.' : null)),
                const SizedBox(width: 12),
                Expanded(
                  child: _datum('Kraj', _kraj, (d) => setState(() => _kraj = d), () {
                    if (_kraj == null) return 'Odaberite datum kraja.';
                    if (_pocetak != null && _kraj!.isBefore(_pocetak!)) return 'Kraj ne može biti prije početka.';
                    return null;
                  }),
                ),
              ],
            ),
            const SizedBox(height: 16),
            TextFormField(
              controller: _opis,
              maxLines: 2,
              decoration: const InputDecoration(labelText: 'Opis (opcionalno)'),
              validator: Validatori.tekst('Opis', obavezan: false, maksimalnaDuzina: 1000),
            ),
            SwitchListTile(
              contentPadding: EdgeInsets.zero,
              title: const Text('Aktivna'),
              subtitle: const Text('Neaktivnu promociju klijenti ne vide i ne mogu je iskoristiti.'),
              value: _jeAktivna,
              onChanged: (v) => setState(() => _jeAktivna = v),
            ),
            if (_greska case final g? when g.greske.isEmpty || g.greskaZaPolje('Kod') == null) ...[const SizedBox(height: 8), PorukaGreske(g.poruka)],
            const SizedBox(height: 24),
            FilledButton(onPressed: _snima ? null : _snimi, child: Text(_uredjivanje ? 'Sačuvaj izmjene' : 'Dodaj promociju')),
          ],
        ),
      ),
    );
  }
}
