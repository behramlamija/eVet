namespace eVet.Model.Statistika
{
    public class StatistikaPoslovanja
    {
        public int BrojTermina { get; set; }

        public int BrojZavrsenihTermina { get; set; }

        public int BrojOtkazanihTermina { get; set; }

        public int BrojPruzenihUsluga { get; set; }

        public decimal UkupniPrihod { get; set; }

        public int BrojOperacija { get; set; }

        public int BrojNovihKlijenata { get; set; }

        public string? NajcescaUsluga { get; set; }

        public string? NajzastupljenijaRasa { get; set; }

        public string? TopVeterinar { get; set; }
    }
}
