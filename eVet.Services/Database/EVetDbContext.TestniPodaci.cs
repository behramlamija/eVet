using eVet.Model.Enums;
using Konstante = eVet.Model.Constants;
using eVet.Services.Security;
using Microsoft.EntityFrameworkCore;

namespace eVet.Services.Database;

public partial class EVetDbContext
{
    // Lozinka svih testnih korisnika je "test". Salt je fiksan da bi seed bio deterministički između migracija.
    private const string TestLozinka = "test";

    private static void SeedTestnihPodataka(ModelBuilder modelBuilder)
    {
        var datumRegistracije = new DateTime(2026, 1, 15, 9, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Korisnik>().HasData(
            TestKorisnik(1, "Emina", "Hadžić", "desktop", "+387 61 111 111", "TFnQ0BSZcBD91PuWE3IxNA==", "4fd76901-8c6e-4b42-91aa-e456ba00d4a9", datumRegistracije),
            Veterinar(TestKorisnik(2, "Nikola", "Nikolić", "veterinar", "+387 61 222 222", "AKvNyrkpSh8okoyRonbPZA==", "4f0e6b9f-14b8-4f10-8f20-53328d6c79bd", datumRegistracije), new DateOnly(1990, 12, 6), 1, new DateOnly(2018, 4, 1), "Doktor veterinarske medicine", 2),
            Veterinar(TestKorisnik(3, "Amar", "Kovačević", "veterinar2", "+387 61 333 333", "pIqi4lQgiAvlO9C4E0bm0Q==", "cbadf0af-9f32-40ee-8e9f-bd7fee1db8a2", datumRegistracije), new DateOnly(1994, 3, 18), 2, new DateOnly(2021, 9, 1), "Doktor veterinarske medicine", 1),
            Veterinar(TestKorisnik(4, "Selma", "Begić", "veterinar3", "+387 61 444 444", "pUUFtIh/wrZB+xcnPKHYvQ==", "e8261264-9b52-40af-b1eb-5003406a1f0e", datumRegistracije), new DateOnly(1996, 7, 25), 1, new DateOnly(2023, 2, 15), "Doktorica veterinarske medicine", 4),
            TestKorisnik(5, "Haris", "Delić", "mobile", "+387 62 555 555", "PU6Onf9pS/NxpBCLyTXiIw==", "6b612f4c-dbed-44d6-9b1d-e7f8b1c05cd4", datumRegistracije),
            TestKorisnik(6, "Lejla", "Mehić", "klijent2", "+387 62 666 666", "EFFCugrGfdoGlgQgVyA7vA==", "6af64aed-222f-4988-bb39-79d17e4552a4", datumRegistracije));

        modelBuilder.Entity<KorisnikUloga>().HasData(
            new KorisnikUloga { KorisnikUlogaId = 1, KorisnikId = 1, UlogaId = 1, DatumDodavanja = datumRegistracije },
            new KorisnikUloga { KorisnikUlogaId = 2, KorisnikId = 2, UlogaId = 2, DatumDodavanja = datumRegistracije },
            new KorisnikUloga { KorisnikUlogaId = 3, KorisnikId = 3, UlogaId = 2, DatumDodavanja = datumRegistracije },
            new KorisnikUloga { KorisnikUlogaId = 4, KorisnikId = 4, UlogaId = 2, DatumDodavanja = datumRegistracije },
            new KorisnikUloga { KorisnikUlogaId = 5, KorisnikId = 5, UlogaId = 3, DatumDodavanja = datumRegistracije },
            new KorisnikUloga { KorisnikUlogaId = 6, KorisnikId = 6, UlogaId = 3, DatumDodavanja = datumRegistracije });

        modelBuilder.Entity<Ljubimac>().HasData(
            new Ljubimac { LjubimacId = 1, KorisnikId = 5, Ime = "Rex", RasaId = 2, Spol = Spol.Muski, DatumRodjenja = new DateOnly(2021, 4, 12), Tezina = 32.5m },
            new Ljubimac { LjubimacId = 2, KorisnikId = 5, Ime = "Mici", RasaId = 9, Spol = Spol.Zenski, DatumRodjenja = new DateOnly(2022, 8, 3), Tezina = 4.2m },
            new Ljubimac { LjubimacId = 3, KorisnikId = 6, Ime = "Luna", RasaId = 3, Spol = Spol.Zenski, DatumRodjenja = new DateOnly(2020, 11, 20), Tezina = 27.8m },
            new Ljubimac { LjubimacId = 4, KorisnikId = 6, Ime = "Kiki", RasaId = 12, Spol = Spol.Muski, DatumRodjenja = new DateOnly(2023, 2, 14), Tezina = 0.1m });

        var datumObjave = new DateTime(2026, 1, 10, 8, 0, 0, DateTimeKind.Utc);

        var usluge = new[]
        {
            new Usluga { UslugaId = 1, VrstaId = 1, Naziv = "Opći pregled", Opis = "Kompletan klinički pregled ljubimca uz savjetovanje vlasnika.", Cijena = 40m, Trajanje = 30, DatumObjavljivanja = datumObjave },
            new Usluga { UslugaId = 2, VrstaId = 1, Naziv = "Kontrolni pregled", Opis = "Kontrola nakon terapije ili intervencije.", Cijena = 25m, Trajanje = 20, DatumObjavljivanja = datumObjave },
            new Usluga { UslugaId = 3, VrstaId = 2, Naziv = "Vakcinacija protiv bjesnila", Opis = "Obavezna godišnja vakcinacija uz upis u pasoš ljubimca.", Cijena = 35m, Trajanje = 15, DatumObjavljivanja = datumObjave },
            new Usluga { UslugaId = 4, VrstaId = 2, Naziv = "Kombinovana vakcina", Opis = "Zaštita od štenećaka, parvoviroze, hepatitisa i leptospiroze.", Cijena = 50m, Trajanje = 15, DatumObjavljivanja = datumObjave },
            new Usluga { UslugaId = 5, VrstaId = 3, Naziv = "Šišanje psa", Opis = "Šišanje i oblikovanje dlake prema pasmini.", Cijena = 50m, Trajanje = 60, DatumObjavljivanja = datumObjave },
            new Usluga { UslugaId = 6, VrstaId = 3, Naziv = "Kupanje i njega krzna", Opis = "Kupanje, sušenje i raščešljavanje krzna.", Cijena = 30m, Trajanje = 45, DatumObjavljivanja = datumObjave },
            new Usluga { UslugaId = 7, VrstaId = 3, Naziv = "Rezanje noktiju", Opis = "Sigurno skraćivanje noktiju.", Cijena = 10m, Trajanje = 10, DatumObjavljivanja = datumObjave },
            new Usluga { UslugaId = 8, VrstaId = 4, Naziv = "Izbjeljivanje zuba", Opis = "Za najljepši osmijeh vašeg ljubimca.", Cijena = 50m, Trajanje = 45, DatumObjavljivanja = datumObjave },
            new Usluga { UslugaId = 9, VrstaId = 4, Naziv = "Čišćenje kamenca", Opis = "Ultrazvučno uklanjanje zubnog kamenca.", Cijena = 70m, Trajanje = 40, DatumObjavljivanja = datumObjave },
            new Usluga { UslugaId = 10, VrstaId = 5, Naziv = "Sterilizacija", Opis = "Hirurški zahvat uz anesteziju i postoperativnu njegu.", Cijena = 200m, Trajanje = 90, DatumObjavljivanja = datumObjave },
            new Usluga { UslugaId = 11, VrstaId = 5, Naziv = "Kastracija", Opis = "Hirurški zahvat uz anesteziju i postoperativnu njegu.", Cijena = 150m, Trajanje = 60, DatumObjavljivanja = datumObjave },
            new Usluga { UslugaId = 12, VrstaId = 6, Naziv = "Kompletna krvna slika", Opis = "Laboratorijska analiza krvi s nalazom isti dan.", Cijena = 40m, Trajanje = 15, DatumObjavljivanja = datumObjave }
        };
        modelBuilder.Entity<Usluga>().HasData(usluge);

        SeedHistorijeTermina(modelBuilder, usluge);
    }

    // Završeni termini iz prošlosti: daju podatke za statistiku i za sistem preporuke
    // (usluge koje su rezervisane zajedno u istom terminu).
    private static void SeedHistorijeTermina(ModelBuilder modelBuilder, Usluga[] usluge)
    {
        // (klijent, ljubimac, veterinar, datum, početak, usluge)
        var historija = new (int Korisnik, int Ljubimac, int Veterinar, DateOnly Datum, TimeOnly Pocetak, int[] Usluge)[]
        {
            (5, 1, 2, new DateOnly(2026, 2, 3), new TimeOnly(9, 0), new[] { 1, 3 }),
            (6, 3, 3, new DateOnly(2026, 2, 10), new TimeOnly(10, 0), new[] { 1, 4 }),
            (5, 1, 4, new DateOnly(2026, 2, 21), new TimeOnly(11, 0), new[] { 5, 6, 7 }),
            (6, 3, 4, new DateOnly(2026, 3, 4), new TimeOnly(9, 30), new[] { 5, 8 }),
            (5, 2, 2, new DateOnly(2026, 3, 12), new TimeOnly(13, 0), new[] { 1, 12 }),
            (6, 3, 4, new DateOnly(2026, 3, 25), new TimeOnly(10, 0), new[] { 5, 6, 8 }),
            (5, 1, 3, new DateOnly(2026, 4, 7), new TimeOnly(9, 0), new[] { 9, 8 }),
            (6, 4, 2, new DateOnly(2026, 4, 15), new TimeOnly(12, 0), new[] { 1, 12 }),
            (5, 2, 3, new DateOnly(2026, 4, 28), new TimeOnly(8, 30), new[] { 12, 10 }),
            (6, 3, 4, new DateOnly(2026, 5, 9), new TimeOnly(11, 0), new[] { 5, 7 }),
            (5, 1, 2, new DateOnly(2026, 5, 20), new TimeOnly(9, 0), new[] { 1, 4, 3 }),
            (6, 3, 3, new DateOnly(2026, 6, 2), new TimeOnly(14, 0), new[] { 9, 8 }),
            (5, 1, 4, new DateOnly(2026, 6, 18), new TimeOnly(10, 0), new[] { 5, 6, 7 }),
            (6, 3, 2, new DateOnly(2026, 7, 6), new TimeOnly(9, 0), new[] { 12, 11 }),
            (5, 2, 4, new DateOnly(2026, 7, 22), new TimeOnly(12, 30), new[] { 6, 7 }),
            (6, 3, 4, new DateOnly(2026, 8, 11), new TimeOnly(10, 0), new[] { 5, 8 }),
            (5, 1, 2, new DateOnly(2026, 9, 1), new TimeOnly(9, 0), new[] { 2, 12 }),
            (6, 4, 3, new DateOnly(2026, 9, 15), new TimeOnly(11, 0), new[] { 1, 2 })
        };

        // ID-evi počinju od 1001 da se seed ne sudari s terminima već kreiranim kroz aplikaciju u postojećoj bazi.
        const int PrviId = 1001;
        var cjenovnik = usluge.ToDictionary(x => x.UslugaId);
        var termini = new List<Termin>();
        var stavke = new List<StavkaTermina>();

        for (int i = 0; i < historija.Length; i++)
        {
            var t = historija[i];
            var terminId = PrviId + i;
            var odabrane = t.Usluge.Select(id => cjenovnik[id]).ToList();
            var trajanje = odabrane.Sum(x => x.Trajanje);
            var datumKreiranja = t.Datum.AddDays(-5).ToDateTime(new TimeOnly(18, 0), DateTimeKind.Utc);

            termini.Add(new Termin
            {
                TerminId = terminId,
                KorisnikId = t.Korisnik,
                VeterinarId = t.Veterinar,
                LjubimacId = t.Ljubimac,
                Sifra = $"HIST{i + 1:D4}",
                DatumTermina = t.Datum,
                VrijemePocetka = t.Pocetak,
                VrijemeKraja = t.Pocetak.AddMinutes(trajanje),
                UkupnoTrajanje = trajanje,
                UkupnaCijena = odabrane.Sum(x => x.Cijena),
                Status = StatusTermina.Zavrsen,
                NacinPlacanjaId = Konstante.NaciniPlacanja.Gotovina,
                DatumKreiranja = datumKreiranja,
                DatumPromjeneStatusa = t.Datum.ToDateTime(t.Pocetak.AddMinutes(trajanje), DateTimeKind.Utc),
                StatusPromijenioId = t.Veterinar
            });

            foreach (var usluga in odabrane)
            {
                stavke.Add(new StavkaTermina { StavkaTerminaId = PrviId + stavke.Count, TerminId = terminId, UslugaId = usluga.UslugaId, Cijena = usluga.Cijena });
            }
        }

        modelBuilder.Entity<Termin>().HasData(termini);
        modelBuilder.Entity<StavkaTermina>().HasData(stavke);
    }

    private static Korisnik Veterinar(Korisnik korisnik, DateOnly datumRodjenja, int gradRodjenjaId, DateOnly datumZaposlenja, string zvanje, int specijalnostId)
    {
        korisnik.DatumRodjenja = datumRodjenja;
        korisnik.GradRodjenjaId = gradRodjenjaId;
        korisnik.DatumZaposlenja = datumZaposlenja;
        korisnik.Zvanje = zvanje;
        korisnik.SpecijalnostId = specijalnostId;
        return korisnik;
    }

    private static Korisnik TestKorisnik(int id, string ime, string prezime, string korisnickoIme, string telefon, string salt, string securityStamp, DateTime datumRegistracije)
    {
        return new Korisnik
        {
            KorisnikId = id,
            Ime = ime,
            Prezime = prezime,
            KorisnickoIme = korisnickoIme,
            Email = $"{korisnickoIme}@example.com",
            Telefon = telefon,
            LozinkaSalt = salt,
            LozinkaHash = PasswordHasher.Hash(TestLozinka, salt),
            SecurityStamp = securityStamp,
            JeAktivan = true,
            DatumRegistracije = datumRegistracije
        };
    }
}
