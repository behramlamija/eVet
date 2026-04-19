using System;
using System.Collections.Generic;
using System.Text;

namespace eVet.Model.Requests
{
    public class KorisniciUpdateRequest
    {
        public string Ime { get; set; } = null!;

        public string Prezime { get; set; } = null!;

        public string? Telefon { get; set; }

        public byte[]? Slika { get; set; }

        public string? Lozinka { get; set; }

        public string? LozinkaPotvrda { get; set; }

        public bool? JeAktivan { get; set; }
    }
}
