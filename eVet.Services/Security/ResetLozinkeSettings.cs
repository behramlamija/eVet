using System.ComponentModel.DataAnnotations;

namespace eVet.Services.Security
{
    // Vrijednosti se čitaju iz .env datoteke (ResetLozinke__*).
    public class ResetLozinkeSettings
    {
        public const string SectionName = "ResetLozinke";

        [Range(1, 1440)]
        public int TrajanjeMinuta { get; set; }

        [Range(1, 20)]
        public int MaksimalnoPokusaja { get; set; }
    }
}
