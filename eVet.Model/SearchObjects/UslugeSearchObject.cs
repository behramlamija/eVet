using System;
using System.Collections.Generic;
using System.Text;

namespace eVet.Model.SearchObjects
{
    public class UslugeSearchObject: BaseSearchObject
    {
        public string? FTS { get; set; }
        public int? VrstaId { get; set; }
        public int? Trajanje { get; set; }
    }
}
