using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using MapsterMapper;

namespace eVet.Services
{
    // Obaveze (podsjetnik) su lične: veterinar vidi i mijenja samo svoje.
    public class ObavezeService : BaseCRUDService<Model.Obaveze, ObavezeSearchObject, Obaveza, ObavezeUpsertRequest, ObavezeUpsertRequest>, IObavezeService
    {
        private readonly ICurrentUserService _currentUser;

        public ObavezeService(EVetDbContext context, IMapper mapper, ICurrentUserService currentUser) : base(context, mapper)
        {
            _currentUser = currentUser;
        }

        protected override string NazivEntiteta => "Obaveza";

        protected override IQueryable<Obaveza> ApplyPristup(IQueryable<Obaveza> query)
        {
            int veterinarId = _currentUser.KorisnikId;
            return query.Where(x => x.VeterinarId == veterinarId);
        }

        protected override IQueryable<Obaveza> AddFilter(ObavezeSearchObject search, IQueryable<Obaveza> query)
        {
            if (search.Datum.HasValue)
            {
                var od = search.Datum.Value.ToDateTime(TimeOnly.MinValue);
                var @do = od.AddDays(1);
                query = query.Where(x => x.DatumVrijeme >= od && x.DatumVrijeme < @do);
            }

            if (search.JeZavrsena.HasValue)
            {
                query = query.Where(x => x.JeZavrsena == search.JeZavrsena);
            }

            return query;
        }

        protected override Task BeforeInsertAsync(ObavezeUpsertRequest request, Obaveza entity, CancellationToken cancellationToken)
        {
            entity.VeterinarId = _currentUser.KorisnikId;
            entity.DatumKreiranja = DateTime.UtcNow;
            return Task.CompletedTask;
        }
    }
}
