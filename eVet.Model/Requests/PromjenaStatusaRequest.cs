using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class PromjenaStatusaRequest
    {
        [Required(ErrorMessage = "Potrebno je unijeti razlog.")]
        [MaxLength(500, ErrorMessage = "Razlog može imati najviše 500 znakova.")]
        public string Razlog { get; set; } = null!;
    }
}
