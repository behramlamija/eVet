import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../providers/korpa_provider.dart';
import 'ljubimac_forma_screen.dart';
import 'paypal_screen.dart';

/// Zakazivanje termina za usluge iz korpe: ljubimac, veterinar, datum, slobodno vrijeme i način plaćanja.
class ZakazivanjeScreen extends StatefulWidget {
  const ZakazivanjeScreen({super.key});

  @override
  State<ZakazivanjeScreen> createState() => _ZakazivanjeScreenState();
}

class _ZakazivanjeScreenState extends State<ZakazivanjeScreen> {
  late Future<(List<Ljubimci>, List<KorisnikOsnovno>, List<NacinPlacanja>, TerminiKonfiguracija)> _podaci;
  int? _ljubimacId;
  int? _veterinarId;
  DateTime? _datum;
  String? _vrijeme;
  int _nacinPlacanjaId = NaciniPlacanja.gotovina;
  Future<List<String>>? _slobodnaVremena;
  bool _salje = false;
  String? _greska;

  @override
  void initState() {
    super.initState();
    _ucitaj();
  }

  void _ucitaj() {
    final ljubimci = context.read<LjubimciProvider>().get(pageSize: BaseProvider.maxPageSize);
    final veterinari = context.read<VeterinariProvider>().get(pageSize: BaseProvider.maxPageSize);
    final nacini = context.read<NaciniPlacanjaProvider>().get(pageSize: BaseProvider.maxPageSize);
    final konfiguracija = context.read<TerminiProvider>().konfiguracija();
    _podaci = (ljubimci, veterinari, nacini, konfiguracija).wait.then((r) => (r.$1.resultList, r.$2.resultList, r.$3.resultList, r.$4));
  }

  void _ucitajVremena() {
    _vrijeme = null;
    if (_veterinarId == null || _datum == null) {
      _slobodnaVremena = null;
      return;
    }
    _slobodnaVremena = context.read<TerminiProvider>().slobodnaVremena(
          veterinarId: _veterinarId!,
          datum: _datum!,
          trajanje: context.read<KorpaProvider>().ukupnoTrajanje,
        );
  }

  Future<void> _dodajLjubimca() async {
    final dodan = await Navigator.of(context).push<bool>(MaterialPageRoute(builder: (_) => const LjubimacFormaScreen()));
    if (dodan == true) setState(_ucitaj);
  }

  Future<void> _odaberiDatum(TerminiKonfiguracija k) async {
    final danas = DateUtils.dateOnly(DateTime.now());
    final d = await showDatePicker(
      context: context,
      initialDate: _datum ?? danas,
      firstDate: danas,
      lastDate: danas.add(Duration(days: k.maksimalnoDanaUnaprijed)),
    );
    if (d != null) {
      setState(() {
        _datum = d;
        _ucitajVremena();
      });
    }
  }

  Future<void> _zakazi(List<Ljubimci> ljubimci, List<KorisnikOsnovno> veterinari) async {
    setState(() => _greska = null);
    if (_ljubimacId == null || _veterinarId == null || _datum == null || _vrijeme == null) {
      setState(() => _greska = 'Odaberite ljubimca, veterinara, datum i slobodno vrijeme termina.');
      return;
    }

    final korpa = context.read<KorpaProvider>();
    final ljubimac = ljubimci.firstWhere((l) => l.ljubimacId == _ljubimacId);
    final veterinar = veterinari.firstWhere((v) => v.korisnikId == _veterinarId);
    final potvrda = await potvrdiAkciju(
      context,
      naslov: 'Potvrda zakazivanja',
      poruka: 'Zakazati termin za ${ljubimac.ime} kod Dr. ${veterinar.prezime}, ${formatDatum(_datum)} u ${formatVrijeme(_vrijeme!)}? '
          '${_nacinPlacanjaId == NaciniPlacanja.payPal ? 'Nakon zakazivanja otvara se PayPal plaćanje.' : 'Plaćanje se vrši u stanici.'}',
      potvrda: 'Zakaži',
    );
    if (!potvrda || !mounted) return;

    setState(() => _salje = true);
    try {
      final termin = await context.read<TerminiProvider>().kreiraj(TerminInsertRequest(
            veterinarId: _veterinarId!,
            ljubimacId: _ljubimacId!,
            datumTermina: _datum,
            vrijemePocetka: _vrijeme,
            uslugeIds: korpa.stavke.map((u) => u.uslugaId).toList(),
            nacinPlacanjaId: _nacinPlacanjaId,
            jePrevoz: korpa.jePrevoz,
            adresaPrevoza: korpa.jePrevoz ? korpa.adresaPrevoza : null,
            gradPrevozaId: korpa.jePrevoz ? korpa.gradPrevozaId : null,
            kodPromocije: korpa.kupon?.promocijaKod,
          ));
      if (!mounted) return;
      korpa.isprazni();
      if (termin.nacinPlacanjaId == NaciniPlacanja.payPal) {
        // Termin je kreiran; ako plaćanje ne uspije, može se platiti kasnije iz detalja termina.
        await platiPayPalom(context, termin);
        if (!mounted) return;
        Navigator.of(context).popUntil((r) => r.isFirst);
        return;
      }
      Navigator.of(context).popUntil((r) => r.isFirst);
      prikaziUspjeh(context, 'Termin ${termin.sifra} je zaprimljen (${formatNovac(termin.ukupnaCijena)}) i čeka potvrdu veterinara.');
    } on ApiException catch (e) {
      setState(() => _greska = e.poruka);
      // Vrijeme je možda u međuvremenu zauzeto; osvježava se lista slobodnih vremena.
      setState(_ucitajVremena);
    } finally {
      if (mounted) setState(() => _salje = false);
    }
  }

  Widget _naslov(String tekst) => Padding(
        padding: const EdgeInsets.only(top: 20, bottom: 8),
        child: Text(tekst, style: Theme.of(context).textTheme.titleMedium?.copyWith(fontWeight: FontWeight.bold)),
      );

  @override
  Widget build(BuildContext context) {
    final korpa = context.watch<KorpaProvider>();

    return Scaffold(
      appBar: AppBar(title: const Text('Zakazivanje termina')),
      body: FutureBuilder(
        future: _podaci,
        builder: (context, snapshot) {
          if (snapshot.hasError) {
            return Padding(padding: const EdgeInsets.all(16), child: PorukaGreske(snapshot.error.toString(), onPokusajPonovo: () => setState(_ucitaj)));
          }
          if (!snapshot.hasData) return const Center(child: CircularProgressIndicator());
          final (ljubimci, veterinari, nacini, konfiguracija) = snapshot.data!;

          return ListView(
            padding: const EdgeInsets.all(16),
            children: [
              Text('${korpa.brojStavki} usluga • ukupno ${korpa.ukupnoTrajanje} min', style: const TextStyle(color: EvetBoje.tekstSivi)),
              _naslov('Ljubimac'),
              if (ljubimci.isEmpty)
                Card(
                  child: ListTile(
                    leading: const Icon(Icons.pets),
                    title: const Text('Nemate registrovanih ljubimaca.'),
                    subtitle: const Text('Za zakazivanje termina potrebno je dodati ljubimca.'),
                    trailing: TextButton(onPressed: _dodajLjubimca, child: const Text('Dodaj')),
                  ),
                )
              else
                Wrap(
                  spacing: 8,
                  children: [
                    for (final l in ljubimci)
                      ChoiceChip(
                        avatar: const Icon(Icons.pets, size: 18),
                        label: Text(l.ime),
                        selected: _ljubimacId == l.ljubimacId,
                        onSelected: (_) => setState(() => _ljubimacId = l.ljubimacId),
                      ),
                  ],
                ),
              _naslov('Veterinar'),
              for (final v in veterinari)
                Card(
                  color: _veterinarId == v.korisnikId ? EvetBoje.tamnoplava.withValues(alpha: 0.08) : null,
                  child: ListTile(
                    leading: EvetSlika(slike: context.read<SlikeProvider>(), tip: TipSlike.korisnik, id: v.korisnikId, imaSliku: v.imaSliku, zamjenskaIkona: Icons.person),
                    title: Text('Dr. ${v.ime} ${v.prezime}'),
                    subtitle: Text(v.specijalnostNaziv ?? v.zvanje ?? 'Veterinar'),
                    trailing: _veterinarId == v.korisnikId ? const Icon(Icons.check_circle, color: EvetBoje.tamnoplava) : null,
                    onTap: () => setState(() {
                      _veterinarId = v.korisnikId;
                      _ucitajVremena();
                    }),
                  ),
                ),
              _naslov('Datum i vrijeme'),
              OutlinedButton.icon(
                onPressed: () => _odaberiDatum(konfiguracija),
                icon: const Icon(Icons.calendar_today_outlined),
                label: Text(_datum == null ? 'Odaberite datum' : formatDatum(_datum)),
              ),
              const SizedBox(height: 12),
              if (_slobodnaVremena == null)
                const Text('Odaberite veterinara i datum da vidite slobodna vremena.', style: TextStyle(color: EvetBoje.tekstSivi))
              else
                FutureBuilder<List<String>>(
                  future: _slobodnaVremena,
                  builder: (context, s) {
                    if (s.hasError) return PorukaGreske(s.error.toString());
                    if (!s.hasData) return const LinearProgressIndicator();
                    if (s.data!.isEmpty) return const Text('Nema slobodnih termina za odabrani dan. Odaberite drugi datum ili veterinara.');
                    return Wrap(
                      spacing: 8,
                      runSpacing: 8,
                      children: [
                        for (final v in s.data!)
                          ChoiceChip(label: Text(formatVrijeme(v)), selected: _vrijeme == v, onSelected: (_) => setState(() => _vrijeme = v)),
                      ],
                    );
                  },
                ),
              _naslov('Način plaćanja'),
              RadioGroup<int>(
                groupValue: _nacinPlacanjaId,
                onChanged: (v) => setState(() => _nacinPlacanjaId = v ?? _nacinPlacanjaId),
                child: Column(
                  children: [
                    for (final n in nacini)
                      RadioListTile<int>(
                        contentPadding: EdgeInsets.zero,
                        value: n.nacinPlacanjaId,
                        title: Text(n.naziv),
                        subtitle: Text(n.nacinPlacanjaId == NaciniPlacanja.gotovina
                            ? 'Plaćanje u stanici nakon pružene usluge.'
                            : 'Online plaćanje odmah nakon zakazivanja. Ako termin bude otkazan ili odbijen, novac se automatski vraća.'),
                      ),
                  ],
                ),
              ),
              const Divider(height: 32),
              Row(
                children: [
                  const Text('Procijenjeni iznos', style: TextStyle(fontSize: 16)),
                  const Spacer(),
                  Text(formatNovac(korpa.ukupno(konfiguracija.cijenaPrevoza)), style: const TextStyle(fontSize: 20, fontWeight: FontWeight.bold)),
                ],
              ),
              if (_greska != null) ...[const SizedBox(height: 16), PorukaGreske(_greska!)],
              const SizedBox(height: 24),
              FilledButton(
                onPressed: _salje || ljubimci.isEmpty ? null : () => _zakazi(ljubimci, veterinari),
                child: _salje
                    ? const SizedBox(width: 22, height: 22, child: CircularProgressIndicator(strokeWidth: 2, color: Colors.white))
                    : const Text('Potvrdi zakazivanje'),
              ),
            ],
          );
        },
      ),
    );
  }
}
