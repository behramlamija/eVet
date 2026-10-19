import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

/// Dodavanje ili uređivanje veterinara. Pri uređivanju se lozinka mijenja samo ako je to izričito odabrano.
Future<bool> otvoriFormuVeterinara(BuildContext context, {Korisnici? veterinar}) async {
  final ulogeZahtjev = context.read<UlogeProvider>().get(filter: {'Naziv': Uloge.veterinar});
  final gradoviZahtjev = context.read<GradoviProvider>().get(pageSize: BaseProvider.maxPageSize);
  final specijalnostiZahtjev = context.read<SpecijalnostiProvider>().get(pageSize: BaseProvider.maxPageSize);
  final PagedResult<Uloga> uloge;
  final PagedResult<Grad> gradovi;
  final PagedResult<Specijalnost> specijalnosti;
  try {
    uloge = await ulogeZahtjev;
    gradovi = await gradoviZahtjev;
    specijalnosti = await specijalnostiZahtjev;
  } on ApiException catch (e) {
    if (context.mounted) prikaziGresku(context, e.poruka);
    return false;
  }
  if (!context.mounted) return false;

  final ulogaVeterinara = uloge.resultList.where((u) => u.naziv == Uloge.veterinar).firstOrNull;
  if (ulogaVeterinara == null) {
    prikaziGresku(context, 'Uloga "${Uloge.veterinar}" ne postoji u sistemu.');
    return false;
  }

  final rezultat = await showDialog<bool>(
    context: context,
    builder: (_) => _VeterinarForma(
      veterinar: veterinar,
      ulogaVeterinaraId: ulogaVeterinara.ulogaId,
      gradovi: gradovi.resultList,
      specijalnosti: specijalnosti.resultList,
    ),
  );
  return rezultat ?? false;
}

class _VeterinarForma extends StatefulWidget {
  const _VeterinarForma({required this.veterinar, required this.ulogaVeterinaraId, required this.gradovi, required this.specijalnosti});

  final Korisnici? veterinar;
  final int ulogaVeterinaraId;
  final List<Grad> gradovi;
  final List<Specijalnost> specijalnosti;

  @override
  State<_VeterinarForma> createState() => _VeterinarFormaState();
}

class _VeterinarFormaState extends State<_VeterinarForma> {
  final _formKey = GlobalKey<FormState>();
  late final Korisnici? _v = widget.veterinar;
  late final _ime = TextEditingController(text: _v?.ime);
  late final _prezime = TextEditingController(text: _v?.prezime);
  late final _korisnickoIme = TextEditingController(text: _v?.korisnickoIme);
  late final _email = TextEditingController(text: _v?.email);
  late final _telefon = TextEditingController(text: _v?.telefon);
  late final _zvanje = TextEditingController(text: _v?.zvanje);
  final _lozinka = TextEditingController();
  final _potvrda = TextEditingController();
  late DateTime? _datumRodjenja = _v?.datumRodjenja;
  late DateTime? _datumZaposlenja = _v?.datumZaposlenja;
  late int? _gradId = _v?.gradRodjenjaId;
  late int? _specijalnostId = _v?.specijalnostId;
  late bool _jeAktivan = _v?.jeAktivan ?? true;
  late bool _promijeniLozinku = _v == null;
  OdabranaSlika? _slika;
  bool _snima = false;
  ApiException? _greska;

  bool get _uredjivanje => _v != null;

  @override
  void dispose() {
    for (final c in [_ime, _prezime, _korisnickoIme, _email, _telefon, _zvanje, _lozinka, _potvrda]) {
      c.dispose();
    }
    super.dispose();
  }

  String? _opcionalno(TextEditingController c) => c.text.trim().isEmpty ? null : c.text.trim();

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

    // Pri uređivanju se zadržavaju postojeće uloge korisnika.
    final ulogeIds = _uredjivanje ? _v!.uloge.map((u) => u.ulogaId).toList() : [widget.ulogaVeterinaraId];

    setState(() => _snima = true);
    try {
      final korisnici = context.read<KorisniciProvider>();
      final Korisnici sacuvan;
      if (_uredjivanje) {
        sacuvan = await korisnici.update(_v!.korisnikId, KorisniciUpdateRequest(
          ime: _ime.text.trim(),
          prezime: _prezime.text.trim(),
          email: _email.text.trim(),
          telefon: _opcionalno(_telefon),
          jeAktivan: _jeAktivan,
          ulogeIds: ulogeIds,
          lozinka: _promijeniLozinku ? _lozinka.text : null,
          lozinkaPotvrda: _promijeniLozinku ? _potvrda.text : null,
          datumRodjenja: _datumRodjenja,
          gradRodjenjaId: _gradId,
          datumZaposlenja: _datumZaposlenja,
          zvanje: _opcionalno(_zvanje),
          specijalnostId: _specijalnostId,
        ).toJson());
      } else {
        sacuvan = await korisnici.insert(KorisniciInsertRequest(
          ime: _ime.text.trim(),
          prezime: _prezime.text.trim(),
          korisnickoIme: _korisnickoIme.text.trim(),
          email: _email.text.trim(),
          telefon: _opcionalno(_telefon),
          lozinka: _lozinka.text,
          lozinkaPotvrda: _potvrda.text,
          jeAktivan: _jeAktivan,
          ulogeIds: ulogeIds,
          datumRodjenja: _datumRodjenja,
          gradRodjenjaId: _gradId,
          datumZaposlenja: _datumZaposlenja,
          zvanje: _opcionalno(_zvanje),
          specijalnostId: _specijalnostId,
        ).toJson());
      }

      if (_slika case final slika?) {
        if (!mounted) return;
        await context.read<SlikeProvider>().postavi(TipSlike.korisnik, sacuvan.korisnikId, bytes: slika.bytes, nazivDatoteke: slika.naziv, contentType: slika.contentType);
      }
      if (!mounted) return;
      prikaziUspjeh(context, _uredjivanje ? 'Podaci veterinara ${sacuvan.ime} ${sacuvan.prezime} su sačuvani.' : 'Veterinar ${sacuvan.ime} ${sacuvan.prezime} je dodan.');
      Navigator.pop(context, true);
    } on ApiException catch (e) {
      setState(() => _greska = e);
    } finally {
      if (mounted) setState(() => _snima = false);
    }
  }

  Widget _polje(TextEditingController c, String labela, String kljuc, FormFieldValidator<String> validator, {bool lozinka = false, bool omoguceno = true}) {
    return TextFormField(
      controller: c,
      obscureText: lozinka,
      enabled: omoguceno,
      decoration: InputDecoration(labelText: labela, errorText: _greska?.greskaZaPolje(kljuc)),
      validator: validator,
    );
  }

  Widget _datum(String labela, DateTime? vrijednost, ValueChanged<DateTime?> promjena, {required DateTime prvi, required DateTime zadnji}) {
    return InputDecorator(
      decoration: InputDecoration(labelText: labela),
      child: Row(
        children: [
          Expanded(child: Text(vrijednost == null ? 'Nije uneseno' : formatDatum(vrijednost))),
          IconButton(
            tooltip: 'Odaberite datum',
            icon: const Icon(Icons.calendar_today_outlined),
            onPressed: () async {
              final d = await showDatePicker(context: context, initialDate: vrijednost ?? zadnji, firstDate: prvi, lastDate: zadnji);
              if (d != null) promjena(d);
            },
          ),
          if (vrijednost != null) IconButton(tooltip: 'Ukloni datum', icon: const Icon(Icons.clear), onPressed: () => promjena(null)),
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    final danas = DateTime.now();
    const razmak = SizedBox(width: 12, height: 16);

    return FormaDijalog(
      naslov: _uredjivanje ? 'Uređivanje veterinara' : 'Novi veterinar',
      sirina: 720,
      child: Form(
        key: _formKey,
        autovalidateMode: AutovalidateMode.onUserInteraction,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            Row(children: [
              Expanded(child: _polje(_ime, 'Ime', 'Ime', Validatori.tekst('Ime', maksimalnaDuzina: 100))),
              razmak,
              Expanded(child: _polje(_prezime, 'Prezime', 'Prezime', Validatori.tekst('Prezime', maksimalnaDuzina: 100))),
            ]),
            razmak,
            Row(children: [
              Expanded(
                child: _polje(_korisnickoIme, 'Korisničko ime', 'KorisnickoIme', _uredjivanje ? (_) => null : Validatori.korisnickoIme, omoguceno: !_uredjivanje),
              ),
              razmak,
              Expanded(child: _polje(_email, 'Email', 'Email', Validatori.email)),
            ]),
            razmak,
            Row(children: [
              Expanded(child: _polje(_telefon, 'Telefon (opcionalno)', 'Telefon', Validatori.telefonOpcionalno)),
              razmak,
              Expanded(child: _polje(_zvanje, 'Zvanje (opcionalno)', 'Zvanje', Validatori.tekst('Zvanje', obavezan: false, maksimalnaDuzina: 100))),
            ]),
            razmak,
            Row(children: [
              Expanded(
                child: _datum('Datum rođenja (opcionalno)', _datumRodjenja, (d) => setState(() => _datumRodjenja = d),
                    prvi: DateTime(1940), zadnji: DateTime(danas.year - 18, danas.month, danas.day)),
              ),
              razmak,
              Expanded(
                child: DropdownButtonFormField<int?>(
                  initialValue: _gradId,
                  decoration: const InputDecoration(labelText: 'Mjesto rođenja (opcionalno)'),
                  items: [
                    const DropdownMenuItem(value: null, child: Text('Nije odabrano')),
                    for (final g in widget.gradovi) DropdownMenuItem(value: g.gradId, child: Text(g.naziv)),
                  ],
                  onChanged: (v) => setState(() => _gradId = v),
                ),
              ),
            ]),
            razmak,
            Row(children: [
              Expanded(child: _datum('Datum zapošljavanja (opcionalno)', _datumZaposlenja, (d) => setState(() => _datumZaposlenja = d), prvi: DateTime(1980), zadnji: danas)),
              razmak,
              Expanded(
                child: DropdownButtonFormField<int?>(
                  initialValue: _specijalnostId,
                  decoration: const InputDecoration(labelText: 'Specijalnost (opcionalno)'),
                  items: [
                    const DropdownMenuItem(value: null, child: Text('Nije odabrano')),
                    for (final s in widget.specijalnosti) DropdownMenuItem(value: s.specijalnostId, child: Text(s.naziv)),
                  ],
                  onChanged: (v) => setState(() => _specijalnostId = v),
                ),
              ),
            ]),
            if (_greska?.greskaZaPolje('DatumRodjenja') ?? _greska?.greskaZaPolje('DatumZaposlenja') case final g?)
              Padding(padding: const EdgeInsets.only(top: 8), child: Text(g, style: TextStyle(color: Theme.of(context).colorScheme.error))),
            const SizedBox(height: 8),
            SwitchListTile(
              contentPadding: EdgeInsets.zero,
              title: const Text('Aktivan'),
              subtitle: const Text('Neaktivan veterinar se ne može prijaviti niti primati nove termine.'),
              value: _jeAktivan,
              onChanged: (v) => setState(() => _jeAktivan = v),
            ),
            Row(
              children: [
                OutlinedButton.icon(onPressed: _odaberiSliku, icon: const Icon(Icons.image_outlined), label: const Text('Odaberi sliku')),
                const SizedBox(width: 12),
                Expanded(
                  child: Text(
                    _slika?.naziv ?? (_v?.imaSliku == true ? 'Slika je postavljena.' : 'Slika nije odabrana (opcionalno).'),
                    overflow: TextOverflow.ellipsis,
                    style: const TextStyle(color: EvetBoje.tekstSivi),
                  ),
                ),
              ],
            ),
            if (_uredjivanje)
              CheckboxListTile(
                contentPadding: EdgeInsets.zero,
                title: const Text('Promijeni lozinku'),
                subtitle: const Text('Stara lozinka nije potrebna kada lozinku mijenja vlasnik.'),
                value: _promijeniLozinku,
                onChanged: (v) => setState(() {
                  _promijeniLozinku = v ?? false;
                  _lozinka.clear();
                  _potvrda.clear();
                }),
              ),
            if (_promijeniLozinku) ...[
              const SizedBox(height: 8),
              Row(children: [
                Expanded(child: _polje(_lozinka, _uredjivanje ? 'Nova lozinka' : 'Lozinka', 'Lozinka', Validatori.lozinka, lozinka: true)),
                razmak,
                Expanded(child: _polje(_potvrda, 'Potvrda lozinke', 'LozinkaPotvrda', Validatori.potvrdaLozinke(() => _lozinka.text), lozinka: true)),
              ]),
            ],
            if (_greska case final g? when g.greske.isEmpty) ...[const SizedBox(height: 16), PorukaGreske(g.poruka)],
            const SizedBox(height: 24),
            FilledButton(onPressed: _snima ? null : _snimi, child: Text(_uredjivanje ? 'Sačuvaj izmjene' : 'Dodaj veterinara')),
          ],
        ),
      ),
    );
  }
}
