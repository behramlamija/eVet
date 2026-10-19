using eVet.Model.Enums;
using eVet.Services.Database;
using Mapster;

namespace eVet.Services
{
    // Mapiranja koriste se i za SQL projekciju (ProjectToType) i za mapiranje učitanih entiteta.
    public static class MapsterConfig
    {
        public static void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Korisnik, Model.Korisnici>()
                .Map(dest => dest.ImaSliku, src => src.Slika != null)
                .Map(dest => dest.Uloge, src => src.KorisniciUloge.Select(ku => ku.Uloga))
                .Map(dest => dest.GodineIskustva, src => src.DatumZaposlenja != null ? DateTime.UtcNow.Year - src.DatumZaposlenja.Value.Year : (int?)null);

            config.NewConfig<Korisnik, Model.KorisnikOsnovno>()
                .Map(dest => dest.ImaSliku, src => src.Slika != null);

            config.NewConfig<Usluga, Model.Usluge>()
                .Map(dest => dest.ImaSliku, src => src.Slika != null);

            config.NewConfig<VrstaUsluge, Model.VrstaUsluge>()
                .Map(dest => dest.ImaSliku, src => src.Slika != null);

            config.NewConfig<Ljubimac, Model.Ljubimci>()
                .Map(dest => dest.VrstaLjubimcaId, src => src.Rasa.VrstaLjubimcaId)
                .Map(dest => dest.VrstaLjubimcaNaziv, src => src.Rasa.VrstaLjubimca.Naziv)
                .Map(dest => dest.ImaSliku, src => src.Slika != null);

            config.NewConfig<Terapija, Model.Terapije>()
                .Map(dest => dest.JeZavrsena, src => src.TrajanjeDana != null && src.DatumTerapije.AddDays(src.TrajanjeDana.Value) < DateTime.UtcNow);

            config.NewConfig<AktiviranaPromocija, Model.AktiviranePromocije>()
                .Map(dest => dest.UslugaId, src => src.Promocija.UslugaId)
                .Map(dest => dest.UslugaNaziv, src => src.Promocija.Usluga.Naziv);

            config.NewConfig<Termin, Model.Termini>()
                .Map(dest => dest.JePlacen, src => src.Uplate.Any(u => u.Status == StatusUplate.Placeno))
                .Map(dest => dest.ImaRecenziju, src => src.Recenzija != null)
                .Map(dest => dest.Stavke, src => src.StavkeTermina);

            config.NewConfig<Recenzija, Model.Recenzije>()
                .Map(dest => dest.VeterinarId, src => src.Termin.VeterinarId)
                .Map(dest => dest.VeterinarIme, src => src.Termin.Veterinar.Ime)
                .Map(dest => dest.VeterinarPrezime, src => src.Termin.Veterinar.Prezime)
                .Map(dest => dest.Odgovori, src => src.RecenzijaOdgovori);
        }
    }
}
