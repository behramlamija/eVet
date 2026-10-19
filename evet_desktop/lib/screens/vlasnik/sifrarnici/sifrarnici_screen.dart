import 'package:evet_shared/evet_shared.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'rasa_forma.dart';
import 'sifrarnik_prikaz.dart';
import 'tekstualna_forma.dart';
import 'vrsta_usluge_forma.dart';

/// Upravljanje referentnim podacima (upute 2.2: CRUD forme za sve referentne podatke).
class SifrarniciScreen extends StatelessWidget {
  const SifrarniciScreen({super.key});

  static Future<bool> _formaNaziva<T>(
    BuildContext context, {
    required String entitet,
    required BaseProvider<T> provider,
    required T? zapis,
    required int Function(T) id,
    required String Function(T) naziv,
    int maksimalnaDuzina = 100,
  }) {
    return otvoriTekstualnuFormu(
      context,
      naslov: zapis == null ? 'Novi zapis: $entitet' : 'Uređivanje: $entitet',
      polja: [
        PoljeForme(kljuc: 'naziv', labela: 'Naziv', pocetnaVrijednost: zapis == null ? null : naziv(zapis), validator: Validatori.tekst('Naziv', maksimalnaDuzina: maksimalnaDuzina)),
      ],
      porukaUspjeha: zapis == null ? '$entitet je dodan(a).' : '$entitet je sačuvan(a).',
      snimi: (v) async {
        final request = NazivUpsertRequest(naziv: v['naziv']!).toJson();
        if (zapis == null) {
          await provider.insert(request);
        } else {
          await provider.update(id(zapis), request);
        }
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    final slike = context.read<SlikeProvider>();
    final gradovi = context.read<GradoviProvider>();
    final vrsteLjubimaca = context.read<VrsteLjubimacaProvider>();
    final specijalnosti = context.read<SpecijalnostiProvider>();
    final naciniPlacanja = context.read<NaciniPlacanjaProvider>();
    final uloge = context.read<UlogeProvider>();

    final tabovi = <(String, Widget)>[
      (
        'Gradovi',
        SifrarnikPrikaz<Grad>(
          konfiguracija: SifrarnikKonfiguracija(
            naziv: 'Gradovi',
            provider: gradovi,
            id: (g) => g.gradId,
            naslov: (g) => g.naziv,
            podnaslov: (g) => 'Poštanski broj: ${g.postanskiBroj}',
            opisZapisa: (g) => 'grad ${g.naziv}',
            otvoriFormu: (context, g) => otvoriTekstualnuFormu(
              context,
              naslov: g == null ? 'Novi grad' : 'Uređivanje grada',
              polja: [
                PoljeForme(kljuc: 'naziv', labela: 'Naziv', pocetnaVrijednost: g?.naziv, validator: Validatori.tekst('Naziv', maksimalnaDuzina: 100)),
                PoljeForme(kljuc: 'postanskiBroj', labela: 'Poštanski broj', pocetnaVrijednost: g?.postanskiBroj, validator: Validatori.postanskiBroj),
              ],
              porukaUspjeha: g == null ? 'Grad je dodan.' : 'Grad je sačuvan.',
              snimi: (v) async {
                final request = GradUpsertRequest(naziv: v['naziv']!, postanskiBroj: v['postanskiBroj']!).toJson();
                g == null ? await gradovi.insert(request) : await gradovi.update(g.gradId, request);
              },
            ),
          ),
        ),
      ),
      (
        'Vrste ljubimaca',
        SifrarnikPrikaz<VrstaLjubimca>(
          konfiguracija: SifrarnikKonfiguracija(
            naziv: 'Vrste ljubimaca',
            provider: vrsteLjubimaca,
            id: (v) => v.vrstaLjubimcaId,
            naslov: (v) => v.naziv,
            opisZapisa: (v) => 'vrstu ljubimca ${v.naziv}',
            otvoriFormu: (context, v) => _formaNaziva(context, entitet: 'Vrsta ljubimca', provider: vrsteLjubimaca, zapis: v, id: (x) => x.vrstaLjubimcaId, naziv: (x) => x.naziv),
          ),
        ),
      ),
      (
        'Rase',
        SifrarnikPrikaz<Rasa>(
          konfiguracija: SifrarnikKonfiguracija(
            naziv: 'Rase',
            provider: context.read<RaseProvider>(),
            id: (r) => r.rasaId,
            naslov: (r) => r.naziv,
            podnaslov: (r) => r.vrstaLjubimcaNaziv,
            opisZapisa: (r) => 'rasu ${r.naziv}',
            otvoriFormu: otvoriFormuRase,
          ),
        ),
      ),
      (
        'Vrste usluga',
        SifrarnikPrikaz<VrstaUsluge>(
          konfiguracija: SifrarnikKonfiguracija(
            naziv: 'Vrste usluga',
            provider: context.read<VrsteUslugaProvider>(),
            id: (v) => v.vrstaId,
            naslov: (v) => v.naziv,
            podnaslov: (v) => [if (v.jeOperacija) 'Operacija', if (v.jeVakcinacija) 'Vakcinacija'].join(', ').ifEmpty(null),
            slika: (v) => EvetSlika(slike: slike, tip: TipSlike.vrstaUsluge, id: v.vrstaId, imaSliku: v.imaSliku, velicina: 40, zamjenskaIkona: Icons.category_outlined),
            opisZapisa: (v) => 'vrstu usluge ${v.naziv}',
            otvoriFormu: otvoriFormuVrsteUsluge,
          ),
        ),
      ),
      (
        'Specijalnosti',
        SifrarnikPrikaz<Specijalnost>(
          konfiguracija: SifrarnikKonfiguracija(
            naziv: 'Specijalnosti',
            provider: specijalnosti,
            id: (s) => s.specijalnostId,
            naslov: (s) => s.naziv,
            opisZapisa: (s) => 'specijalnost ${s.naziv}',
            otvoriFormu: (context, s) => _formaNaziva(context, entitet: 'Specijalnost', provider: specijalnosti, zapis: s, id: (x) => x.specijalnostId, naziv: (x) => x.naziv),
          ),
        ),
      ),
      (
        'Načini plaćanja',
        SifrarnikPrikaz<NacinPlacanja>(
          konfiguracija: SifrarnikKonfiguracija(
            naziv: 'Načini plaćanja',
            provider: naciniPlacanja,
            id: (n) => n.nacinPlacanjaId,
            naslov: (n) => n.naziv,
            opisZapisa: (n) => 'način plaćanja ${n.naziv}',
            razlogBezDodavanja: 'Načini plaćanja su vezani za implementaciju plaćanja (gotovina i PayPal), pa ih nije moguće dodavati ni brisati. Naziv se može promijeniti.',
            razlogBezBrisanja: 'Način plaćanja koristi logika plaćanja i ne može se obrisati.',
            otvoriFormu: (context, n) => _formaNaziva(context, entitet: 'Način plaćanja', provider: naciniPlacanja, zapis: n, id: (x) => x.nacinPlacanjaId, naziv: (x) => x.naziv),
          ),
        ),
      ),
      (
        'Uloge',
        SifrarnikPrikaz<Uloga>(
          konfiguracija: SifrarnikKonfiguracija(
            naziv: 'Uloge',
            provider: uloge,
            id: (u) => u.ulogaId,
            naslov: (u) => u.naziv,
            podnaslov: (u) => u.opis,
            opisZapisa: (u) => 'ulogu ${u.naziv}',
            razlogBezDodavanja: 'Uloge određuju prava pristupa u aplikaciji, pa ih nije moguće dodavati ni brisati. Može se mijenjati opis uloge.',
            razlogBezBrisanja: 'Uloga se koristi za prava pristupa i ne može se obrisati.',
            otvoriFormu: (context, u) => otvoriTekstualnuFormu(
              context,
              naslov: 'Uređivanje uloge ${u!.naziv}',
              napomena: 'Naziv uloge se ne može mijenjati jer ga koristi provjera prava pristupa.',
              polja: [
                PoljeForme(kljuc: 'opis', labela: 'Opis', pocetnaVrijednost: u.opis, maksimalnoRedova: 3, validator: Validatori.tekst('Opis', obavezan: false)),
              ],
              porukaUspjeha: 'Opis uloge ${u.naziv} je sačuvan.',
              snimi: (v) => uloge.update(u.ulogaId, UlogaUpdateRequest(opis: v['opis']).toJson()),
            ),
          ),
        ),
      ),
    ];

    return DefaultTabController(
      length: tabovi.length,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          Padding(
            padding: const EdgeInsets.fromLTRB(32, 24, 32, 0),
            child: Text('ŠIFRARNICI', style: Theme.of(context).textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold)),
          ),
          TabBar(isScrollable: true, tabAlignment: TabAlignment.start, tabs: [for (final t in tabovi) Tab(text: t.$1)]),
          Expanded(child: TabBarView(children: [for (final t in tabovi) t.$2])),
        ],
      ),
    );
  }
}

extension on String {
  String? ifEmpty(String? zamjena) => isEmpty ? zamjena : this;
}
