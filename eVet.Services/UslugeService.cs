using eVet.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace eVet.Services
{
    public class UslugeService: IUslugeService
    {
        public List<Usluge> List = new List<Usluge>()
        {
           new Usluge()
           {

           },
           new Usluge()
              {

              }
        };

        public List<Usluge> GetAll()
        {
            return List;
        }
    }
}
