using Konstante = eVet.Model.Constants;
using Microsoft.EntityFrameworkCore;

namespace eVet.Services.Database;

public partial class EVetDbContext
{
    private static void SeedReferentnihPodataka(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Uloga>().HasData(
            new Uloga { UlogaId = 1, Naziv = Konstante.Uloge.Vlasnik, Opis = "Vlasnik veterinarske stanice" },
            new Uloga { UlogaId = 2, Naziv = Konstante.Uloge.Veterinar, Opis = "Veterinar uposlen u stanici" },
            new Uloga { UlogaId = 3, Naziv = Konstante.Uloge.Klijent, Opis = "Vlasnik kućnog ljubimca" });

        modelBuilder.Entity<NacinPlacanja>().HasData(
            new NacinPlacanja { NacinPlacanjaId = Konstante.NaciniPlacanja.Gotovina, Naziv = "Gotovina" },
            new NacinPlacanja { NacinPlacanjaId = Konstante.NaciniPlacanja.PayPal, Naziv = "PayPal" });

        modelBuilder.Entity<VrstaUsluge>().HasData(
            new VrstaUsluge { VrstaId = 1, Naziv = "Pregledi" },
            new VrstaUsluge { VrstaId = 2, Naziv = "Vakcinacija", JeVakcinacija = true },
            new VrstaUsluge { VrstaId = 3, Naziv = "Šišanje i njega" },
            new VrstaUsluge { VrstaId = 4, Naziv = "Stomatologija" },
            new VrstaUsluge { VrstaId = 5, Naziv = "Operacije", JeOperacija = true },
            new VrstaUsluge { VrstaId = 6, Naziv = "Laboratorija" });

        modelBuilder.Entity<Grad>().HasData(
            new Grad { GradId = 1, Naziv = "Mostar", PostanskiBroj = "88000" },
            new Grad { GradId = 2, Naziv = "Sarajevo", PostanskiBroj = "71000" },
            new Grad { GradId = 3, Naziv = "Tuzla", PostanskiBroj = "75000" },
            new Grad { GradId = 4, Naziv = "Zenica", PostanskiBroj = "72000" },
            new Grad { GradId = 5, Naziv = "Banja Luka", PostanskiBroj = "78000" },
            new Grad { GradId = 6, Naziv = "Bihać", PostanskiBroj = "77000" },
            new Grad { GradId = 7, Naziv = "Široki Brijeg", PostanskiBroj = "88220" },
            new Grad { GradId = 8, Naziv = "Čapljina", PostanskiBroj = "88300" },
            new Grad { GradId = 9, Naziv = "Konjic", PostanskiBroj = "88400" },
            new Grad { GradId = 10, Naziv = "Stolac", PostanskiBroj = "88360" });

        modelBuilder.Entity<Specijalnost>().HasData(
            new Specijalnost { SpecijalnostId = 1, Naziv = "Opća praksa" },
            new Specijalnost { SpecijalnostId = 2, Naziv = "Hirurgija" },
            new Specijalnost { SpecijalnostId = 3, Naziv = "Dermatologija" },
            new Specijalnost { SpecijalnostId = 4, Naziv = "Stomatologija" },
            new Specijalnost { SpecijalnostId = 5, Naziv = "Interna medicina" });

        modelBuilder.Entity<VrstaLjubimca>().HasData(
            new VrstaLjubimca { VrstaLjubimcaId = 1, Naziv = "Pas" },
            new VrstaLjubimca { VrstaLjubimcaId = 2, Naziv = "Mačka" },
            new VrstaLjubimca { VrstaLjubimcaId = 3, Naziv = "Ptica" },
            new VrstaLjubimca { VrstaLjubimcaId = 4, Naziv = "Mali sisar" });

        modelBuilder.Entity<Rasa>().HasData(
            new Rasa { RasaId = 1, VrstaLjubimcaId = 1, Naziv = "Mješanac" },
            new Rasa { RasaId = 2, VrstaLjubimcaId = 1, Naziv = "Njemački ovčar" },
            new Rasa { RasaId = 3, VrstaLjubimcaId = 1, Naziv = "Labrador retriver" },
            new Rasa { RasaId = 4, VrstaLjubimcaId = 1, Naziv = "Zlatni retriver" },
            new Rasa { RasaId = 5, VrstaLjubimcaId = 1, Naziv = "Francuski buldog" },
            new Rasa { RasaId = 6, VrstaLjubimcaId = 1, Naziv = "Tornjak" },
            new Rasa { RasaId = 7, VrstaLjubimcaId = 1, Naziv = "Maltezer" },
            new Rasa { RasaId = 8, VrstaLjubimcaId = 2, Naziv = "Mješanac" },
            new Rasa { RasaId = 9, VrstaLjubimcaId = 2, Naziv = "Perzijska" },
            new Rasa { RasaId = 10, VrstaLjubimcaId = 2, Naziv = "Britanska kratkodlaka" },
            new Rasa { RasaId = 11, VrstaLjubimcaId = 2, Naziv = "Sijamska" },
            new Rasa { RasaId = 12, VrstaLjubimcaId = 3, Naziv = "Papagaj" },
            new Rasa { RasaId = 13, VrstaLjubimcaId = 3, Naziv = "Kanarinac" },
            new Rasa { RasaId = 14, VrstaLjubimcaId = 4, Naziv = "Hrčak" },
            new Rasa { RasaId = 15, VrstaLjubimcaId = 4, Naziv = "Zamorac" },
            new Rasa { RasaId = 16, VrstaLjubimcaId = 4, Naziv = "Kunić" });
    }
}
