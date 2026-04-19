using eVet.Model;
using eVet.Model.SearchObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace eVet.Services
{
    public interface IService <TModel, TSearch> where TSearch:BaseSearchObject
    {
        public PagedResult<TModel> GetPaged(TSearch search);

        public TModel GetById(int id);
    }
}
