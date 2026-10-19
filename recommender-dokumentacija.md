# Sistem preporuke – „Korisnici također rezervišu”

## Cilj

U mobilnoj aplikaciji klijentu se predlažu dodatne usluge na osnovu ponašanja drugih klijenata.
To je opisano u prijavi teme, poglavlje „Sistem preporuke”: ako klijenti koji rezervišu „Šišanje psa” često u istom terminu rezervišu i „Izbjeljivanje zuba”, onda se „Izbjeljivanje zuba” preporučuje svakome ko pregleda „Šišanje psa” ili ga doda u korpu.

## Algoritam

Koristi se **item-item collaborative filtering** zasnovan na zajedničkom pojavljivanju usluga (co-occurrence) u istoj narudžbi.
Jedna narudžba je jedan termin. Usluge narudžbe su stavke termina (tabela `StavkaTermina`).

Za polaznu uslugu **A** i kandidata **B**:

| Veličina | Značenje |
|---|---|
| `podrška(A)` | broj termina u kojima je rezervisana usluga A |
| `podrška(A, B)` | broj termina u kojima su A i B rezervisane zajedno |
| `pouzdanost(A → B) = podrška(A, B) / podrška(A)` | udio termina s uslugom A u kojima je rezervisana i B |

Kandidati se rangiraju:
1. po pouzdanosti (opadajuće);
2. zatim po broju zajedničkih termina;
3. zatim po ID-u usluge, da redoslijed bude stabilan.

Vraća se najviše 5 preporuka (`PreporukeService.MaksimalnoPreporuka`).

### Više polaznih usluga (korpa)

U korpi se preporuke traže za sve usluge iz korpe odjednom:
- Kandidat je usluga koja je bila u terminu s **bilo kojom** uslugom iz korpe, a sama nije u korpi.
- Ako je kandidat povezan s više usluga iz korpe, uzima se **najjača veza**: najveća pouzdanost, pa veći broj zajedničkih termina. Ta polazna usluga se navodi u objašnjenju.

### Koji podaci ulaze u model

- Ulaz su stvarni termini koje klijenti kreiraju u mobilnoj aplikaciji: `POST /Termini` upisuje termin i njegove stavke.
- Uzimaju se termini u statusima **Na čekanju**, **Potvrđen** i **Završen**.
- **Otkazani** i **odbijeni** termini se isključuju jer ta kombinacija usluga nije stvarno korištena.
- Obrisane usluge se ne preporučuju (globalni filter za soft delete).
- Kako raste broj rezervacija, preporuke postaju preciznije.
- Testni podaci (`EVetDbContext.TestniPodaci.cs`, migracija `HistorijaTermina`) sadrže 18 završenih termina iz 2026. godine. S njima preporuke rade odmah po pokretanju.

### Model i osvježavanje

- Ne postoji zaseban istrenirani model koji se čuva i periodično obnavlja.
- Matrica zajedničkih pojavljivanja računa se pri svakom zahtjevu jednim SQL upitom (self-join `StavkaTermina` po `TerminId`, grupisanje po paru usluga).
- Zbog toga svaki novi termin odmah utiče na preporuke, bez ponovnog treniranja.
- Za broj usluga i termina u jednoj veterinarskoj stanici ovaj upit je brz.
- Ako bi podataka bilo mnogo više, isti rezultat bi se mogao unaprijed računati u pozadinskom servisu (npr. u Workeru nakon događaja „termin kreiran”).

### Hladni start

- Za uslugu koja još nije rezervisana zajedno ni s jednom drugom uslugom, preporuka nema.
- Na detaljima usluge tada se prikazuje poruka da još nema dovoljno rezervacija. U korpi se sekcija ne prikazuje.
- Ne koristi se zamjenski pristup (npr. najpopularnije usluge), jer prijava opisuje samo collaborative filtering.

## Objašnjenje preporuke

Svaka preporuka nosi tekstualno objašnjenje izračunato iz istih brojeva koji su korišteni za rangiranje, npr.:

> Klijenti koji rezervišu uslugu "Šišanje psa" uz nju su rezervisali i ovu uslugu u 3 od 6 termina (50 %).

Odgovor uz objašnjenje sadrži i sirove vrijednosti:
- `UslugaIzvorId` i `UslugaIzvorNaziv`;
- `BrojZajednickihTermina`;
- `BrojTerminaIzvora`;
- `Pouzdanost`.

## API

```
GET /Preporuke?uslugeIds=5
GET /Preporuke?uslugeIds=5&uslugeIds=8
```

- Endpoint zahtijeva prijavu (JWT).
- Odgovor je lista `PreporukeUsluga`: `Usluga` (puni DTO usluge) i polja objašnjenja iz prethodnog odjeljka.
- Prazna lista ili više od 20 polaznih usluga vraća grešku `400` s porukom.

## Prikaz u aplikaciji

| Mjesto | Polazne usluge | Ponašanje |
|---|---|---|
| Detalji usluge (mobilna) | otvorena usluga | Sekcija „Korisnici također rezervišu” ispod detalja; poruka ako preporuka nema |
| Korpa (mobilna) | sve usluge u korpi | Sekcija se osvježava pri svakoj promjeni korpe; skrivena ako preporuka nema |

Svaka preporuka prikazuje naziv, cijenu i objašnjenje. Dugme za korpu dodaje uslugu **jednim klikom**, a dodir na preporuku otvara detalje te usluge.

## Kod

| Datoteka | Uloga |
|---|---|
| `eVet.Services/PreporukeService.cs` | Algoritam: podrška, pouzdanost, rangiranje i objašnjenje |
| `eVet.API/Controllers/PreporukeController.cs` | Endpoint `GET /Preporuke` |
| `eVet.Model/PreporukeUsluga.cs` | DTO preporuke s objašnjenjem |
| `evet_shared/lib/src/providers/preporuke_provider.dart` | Poziv API-ja iz Fluttera |
| `evet_mobile/lib/widgets/preporuke_sekcija.dart` | Sekcija „Korisnici također rezervišu” |
