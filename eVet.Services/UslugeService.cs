using eVet.Model;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eVet.Services
{
    public class UslugeService: BaseService<Model.Usluge, UslugeSearchObject, Database.Usluga>, IUslugeService
    {
       
        public UslugeService(_210210Context context, IMapper mapper):base(context, mapper)
        {
            Context = context;
            Mapper = mapper;
        }
      
        public override IQueryable<Database.Usluga> AddFilter(UslugeSearchObject search, IQueryable<Database.Usluga> query)
        {
            var filteredQuery = base.AddFilter(search, query);

            if(!string.IsNullOrEmpty(search.FTS))
            {
                filteredQuery = filteredQuery.Where(x => x.Naziv.Contains(search.FTS) || x.Opis.Contains(search.FTS));
            }

            return filteredQuery;
        }

    }
}
