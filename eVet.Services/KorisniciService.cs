using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq.Dynamic.Core;
namespace eVet.Services
{
    public class KorisniciService : IKorisniciService
    {
        public _210210Context Context { get; set; }
        public IMapper Mapper { get; set; }
        public KorisniciService(_210210Context context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public virtual Model.PagedResult<Korisnici> GetAll(KorisniciSearchObject searchObject)
        {
            List<Model.Korisnici> result = new List<Model.Korisnici>();

            var query = Context.Korisniks.AsQueryable();

            if(!string.IsNullOrWhiteSpace(searchObject.ImeGTE))
                {
                    query = query.Where(x => x.Ime.StartsWith(searchObject.ImeGTE));
            }

            if (!string.IsNullOrWhiteSpace(searchObject.PrezimeGTE))
            {
                query = query.Where(x => x.Prezime.StartsWith(searchObject.PrezimeGTE));
            }

            if (!string.IsNullOrWhiteSpace(searchObject.Email))
            {
                query = query.Where(x => x.Email.StartsWith(searchObject.Email));
            }

            if (!string.IsNullOrWhiteSpace(searchObject.KorisnickoIme))
            {
                query = query.Where(x => x.KorisnickoIme.StartsWith(searchObject.KorisnickoIme));
            }

            if (searchObject.IsKorisniciUlogeIncluded == true)
            {
                query = query.Include(x => x.KorisniciUloges).ThenInclude(x => x.Uloga);
            }

            int count= query.Count();

            if (!string.IsNullOrWhiteSpace(searchObject.OrderBy))
            {
                var items= searchObject.OrderBy.Split(',');
                if(items.Length>2 || items.Length==0)
                {
                    throw new Exception("Mozete sortirati samo po jednom polju!");
                }
                if (items.Length == 1)
                {
                    query = query.OrderBy("@0", searchObject.OrderBy);
                }
                else
                {
                    query = query.OrderBy(string.Format("{0} {1}", items[0], items[1]));
                }

                query =query.OrderBy(searchObject.OrderBy);
            }

            if (searchObject?.Page.HasValue == true && searchObject?.PageSize.HasValue == true)
            {
                query = query.Skip(searchObject.Page.Value * searchObject.PageSize.Value).Take(searchObject.PageSize.Value);
            }


            

            var list=query.ToList();

            var resultList = Mapper.Map(list, result);
            Model.PagedResult<Korisnici> response = new Model.PagedResult<Korisnici>();

            response.ResultList = resultList;
            response.Count = count;


            return response;
        }

        public Korisnici Insert(KorisniciInsertRequest request)
        {
            if (request.Lozinka != request.LozinkaPotvrda)
            {
                throw new Exception("Lozinke se ne poklapaju!");
            }

            Database.Korisnik entity = new Database.Korisnik();
            Mapper.Map(request, entity);

            entity.LozinkaSalt = GenerateSalt();
            entity.LozinkaHash = GenerateHash(entity.LozinkaSalt, request.Lozinka);

            Context.Add(entity);
            Context.SaveChanges();

            return Mapper.Map<Korisnici>(entity);
        }


        public static string GenerateSalt()
        {
            byte[] byteArray = RandomNumberGenerator.GetBytes(16);
            return Convert.ToBase64String(byteArray);
        }

        public static string GenerateHash(string lozinkaSalt, string lozinka)
        {
            byte[] src = Convert.FromBase64String(lozinkaSalt);
            byte[] bytes = Encoding.Unicode.GetBytes(lozinka);
            byte[] dst = new byte[src.Length + bytes.Length];

            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);
        }

        public Korisnici Update(int id, KorisniciUpdateRequest request)
        {
            var entity = Context.Korisniks.Find(id);
            Mapper.Map(request, entity);

            if (request.Lozinka != null)
            {
                if (request.Lozinka != request.LozinkaPotvrda)
                {
                    throw new Exception("Lozinke se ne poklapaju!");
                }
                else
                {
                    entity.LozinkaSalt = GenerateSalt();
                    entity.LozinkaHash = GenerateHash(entity.LozinkaSalt, request.Lozinka);

                }
            }


            Context.SaveChanges();
            return Mapper.Map<Korisnici>(entity);
        }
    }
}
