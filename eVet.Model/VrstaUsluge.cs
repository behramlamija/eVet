using eVet.Model;
using System;
using System.Collections.Generic;

namespace eVet.Model
{

    public partial class VrstaUsluge
    {
        public int VrstaId { get; set; }

        public string Naziv { get; set; } = null!;

        public byte[]? Slika { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? VrijemeBrisanja { get; set; }
    }
}