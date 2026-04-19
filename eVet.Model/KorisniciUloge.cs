using System;
using System.Collections.Generic;

namespace eVet.Model
{

    public partial class KorisniciUloge
    {
        public int KorisnikUlogaId { get; set; }

        public int KorisnikId { get; set; }

        public int UlogaId { get; set; }

        public DateTime DatumDodavanja { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? VrijemeBrisanja { get; set; }

        public virtual Uloga Uloga { get; set; } = null!;
    }
}