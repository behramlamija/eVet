using eVet.Model;
using System;
using System.Collections.Generic;

namespace eVet.Model
{

    public partial class Uloga
    {
        public int UlogaId { get; set; }

        public string Naziv { get; set; } = null!;

        public string? Opis { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? VrijemeBrisanja { get; set; }

    }
}