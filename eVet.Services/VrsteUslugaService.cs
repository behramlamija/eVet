using eVet.Model.SearchObjects;
using eVet.Services.Database;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace eVet.Services
{
    public class VrsteUslugaService: BaseService <Model.VrstaUsluge, VrstaUslugeSearchObject, Database.VrstaUsluge>, IVrsteUslugaService
    {
        public VrsteUslugaService(_210210Context context, IMapper mapper): base(context, mapper)
        {
            
        }


       
    }
}
