using eVet.Model.Enums;

namespace eVet.Model
{
    public class Termini
    {
        public int TerminId { get; set; }

        public string Sifra { get; set; } = null!;

        public int KorisnikId { get; set; }

        public string KorisnikIme { get; set; } = null!;

        public string KorisnikPrezime { get; set; } = null!;

        public int VeterinarId { get; set; }

        public string VeterinarIme { get; set; } = null!;

        public string VeterinarPrezime { get; set; } = null!;

        public int LjubimacId { get; set; }

        public string LjubimacIme { get; set; } = null!;

        public DateOnly DatumTermina { get; set; }

        public TimeOnly VrijemePocetka { get; set; }

        public TimeOnly VrijemeKraja { get; set; }

        public int UkupnoTrajanje { get; set; }

        public decimal IznosPopusta { get; set; }

        public decimal CijenaPrevoza { get; set; }

        public decimal UkupnaCijena { get; set; }

        public StatusTermina Status { get; set; }

        public int NacinPlacanjaId { get; set; }

        public string NacinPlacanjaNaziv { get; set; } = null!;

        public bool JePlacen { get; set; }

        public bool JeRefundiran { get; set; }

        public bool JePrevoz { get; set; }

        public string? AdresaPrevoza { get; set; }

        public int? GradPrevozaId { get; set; }

        public string? GradPrevozaNaziv { get; set; }

        public DateTime DatumKreiranja { get; set; }

        public DateTime? DatumPromjeneStatusa { get; set; }

        public string? StatusPromijenioIme { get; set; }

        public string? StatusPromijenioPrezime { get; set; }

        public string? RazlogPromjeneStatusa { get; set; }

        public bool ImaRecenziju { get; set; }

        public List<StavkeTermina> Stavke { get; set; } = new List<StavkeTermina>();
    }
}
