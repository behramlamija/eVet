namespace eVet.Model.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;

        public DateTime DatumIsteka { get; set; }

        public Korisnici Korisnik { get; set; } = null!;
    }
}
