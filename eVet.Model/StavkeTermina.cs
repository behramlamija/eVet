namespace eVet.Model
{
    public class StavkeTermina
    {
        public int StavkaTerminaId { get; set; }

        public int UslugaId { get; set; }

        public string UslugaNaziv { get; set; } = null!;

        public decimal Cijena { get; set; }
    }
}
