using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace eVet.Services.Database;

public partial class EVetDbContext : DbContext
{
    public EVetDbContext(DbContextOptions<EVetDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AktiviranaPromocija> AktiviranePromocije { get; set; }

    public virtual DbSet<Grad> Gradovi { get; set; }

    public virtual DbSet<KorisnikUloga> KorisniciUloge { get; set; }

    public virtual DbSet<Korisnik> Korisnici { get; set; }

    public virtual DbSet<Ljubimac> Ljubimci { get; set; }

    public virtual DbSet<NacinPlacanja> NaciniPlacanja { get; set; }

    public virtual DbSet<Obavijest> Obavijesti { get; set; }

    public virtual DbSet<Obaveza> Obaveze { get; set; }

    public virtual DbSet<Promocija> Promocije { get; set; }

    public virtual DbSet<Rasa> Rase { get; set; }

    public virtual DbSet<Recenzija> Recenzije { get; set; }

    public virtual DbSet<RecenzijaOdgovor> RecenzijaOdgovori { get; set; }

    public virtual DbSet<ResetLozinke> ResetiLozinki { get; set; }

    public virtual DbSet<Specijalnost> Specijalnosti { get; set; }

    public virtual DbSet<StavkaTermina> StavkeTermina { get; set; }

    public virtual DbSet<Terapija> Terapije { get; set; }

    public virtual DbSet<Termin> Termini { get; set; }

    public virtual DbSet<Uloga> Uloge { get; set; }

    public virtual DbSet<Uplata> Uplate { get; set; }

    public virtual DbSet<Usluga> Usluge { get; set; }

    public virtual DbSet<VrstaLjubimca> VrsteLjubimaca { get; set; }

    public virtual DbSet<VrstaUsluge> VrsteUsluga { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AktiviranaPromocija>(entity =>
        {
            entity.ToTable("AktiviranaPromocija");

            entity.HasIndex(e => new { e.KorisnikId, e.PromocijaId }, "UQ_AktProm_Korisnik_Promocija").IsUnique().HasFilter("[IsDeleted] = 0");

            entity.HasIndex(e => e.PromocijaId, "IX_AktProm_PromocijaId");

            entity.Property(e => e.DatumAktiviranja)
                .HasDefaultValueSql("(getutcdate())", "DF_AktProm_Datum")
                .HasColumnType("datetime");
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.AktiviranePromocije)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_AktProm_Korisnik");

            entity.HasOne(d => d.Promocija).WithMany(p => p.AktiviranePromocije)
                .HasForeignKey(d => d.PromocijaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_AktProm_Promocija");
        });

        modelBuilder.Entity<Grad>(entity =>
        {
            entity.ToTable("Grad");

            entity.HasIndex(e => e.Naziv, "UQ_Grad_Naziv").IsUnique().HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.Naziv).HasMaxLength(100);
            entity.Property(e => e.PostanskiBroj).HasMaxLength(10);
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");
        });

        modelBuilder.Entity<KorisnikUloga>(entity =>
        {
            entity.ToTable("KorisniciUloge");

            entity.HasIndex(e => new { e.KorisnikId, e.UlogaId }, "UQ_KorisniciUloge_Korisnik_Uloga").IsUnique();

            entity.HasIndex(e => e.UlogaId, "IX_KorisniciUloge_UlogaId");

            entity.Property(e => e.DatumDodavanja)
                .HasDefaultValueSql("(getutcdate())", "DF_KorUloge_Datum")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.KorisniciUloge)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_KorUloge_Korisnik");

            entity.HasOne(d => d.Uloga).WithMany(p => p.KorisniciUloge)
                .HasForeignKey(d => d.UlogaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_KorUloge_Uloga");
        });

        modelBuilder.Entity<Korisnik>(entity =>
        {
            entity.ToTable("Korisnik");

            entity.HasIndex(e => e.Email, "UQ_Korisnik_Email").IsUnique().HasFilter("[IsDeleted] = 0");

            entity.HasIndex(e => e.KorisnickoIme, "UQ_Korisnik_KorisnickoIme").IsUnique().HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.DatumRegistracije)
                .HasDefaultValueSql("(getutcdate())", "DF_Korisnik_DatumReg")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Ime).HasMaxLength(100);
            entity.Property(e => e.KorisnickoIme).HasMaxLength(50);
            entity.Property(e => e.LozinkaHash).HasMaxLength(255);
            entity.Property(e => e.LozinkaSalt).HasMaxLength(255);
            entity.Property(e => e.Prezime).HasMaxLength(100);
            entity.Property(e => e.SecurityStamp).HasMaxLength(50);
            entity.Property(e => e.Telefon).HasMaxLength(50);
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");
            entity.Property(e => e.Zvanje).HasMaxLength(100);

            entity.HasOne(d => d.GradRodjenja).WithMany(p => p.Korisnici)
                .HasForeignKey(d => d.GradRodjenjaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Korisnik_GradRodjenja");

            entity.HasOne(d => d.Specijalnost).WithMany(p => p.Veterinari)
                .HasForeignKey(d => d.SpecijalnostId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Korisnik_Specijalnost");
        });

        modelBuilder.Entity<Ljubimac>(entity =>
        {
            entity.ToTable("Ljubimac");

            entity.HasIndex(e => e.KorisnikId, "IX_Ljubimac_KorisnikId");

            entity.HasIndex(e => e.RasaId, "IX_Ljubimac_RasaId");

            entity.Property(e => e.Ime).HasMaxLength(100);
            entity.Property(e => e.Napomena).HasMaxLength(1000);
            entity.Property(e => e.Tezina).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Ljubimci)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Ljubimac_Korisnik");

            entity.HasOne(d => d.Rasa).WithMany(p => p.Ljubimci)
                .HasForeignKey(d => d.RasaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Ljubimac_Rasa");
        });

        modelBuilder.Entity<NacinPlacanja>(entity =>
        {
            entity.ToTable("NacinPlacanja");

            entity.HasIndex(e => e.Naziv, "UQ_NacinPlacanja_Naziv").IsUnique().HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.Naziv).HasMaxLength(100);
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");
        });

        modelBuilder.Entity<Obavijest>(entity =>
        {
            entity.ToTable("Obavijest");

            entity.HasIndex(e => e.KorisnikId, "IX_Obavijest_KorisnikId");

            entity.Property(e => e.DatumObavijesti)
                .HasDefaultValueSql("(getutcdate())", "DF_Obavijest_Datum")
                .HasColumnType("datetime");
            entity.Property(e => e.Naslov).HasMaxLength(255);
            entity.Property(e => e.Sadrzaj).HasMaxLength(2000);
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Obavijesti)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Obavijest_Korisnik");
        });

        modelBuilder.Entity<Obaveza>(entity =>
        {
            entity.ToTable("Obaveza");

            entity.HasIndex(e => new { e.VeterinarId, e.DatumVrijeme }, "IX_Obaveza_Veterinar_Datum");

            entity.Property(e => e.DatumKreiranja)
                .HasDefaultValueSql("(getutcdate())", "DF_Obaveza_DatumKreiranja")
                .HasColumnType("datetime");
            entity.Property(e => e.DatumVrijeme).HasColumnType("datetime");
            entity.Property(e => e.Naslov).HasMaxLength(255);
            entity.Property(e => e.Opis).HasMaxLength(1000);
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Veterinar).WithMany(p => p.Obaveze)
                .HasForeignKey(d => d.VeterinarId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Obaveza_Veterinar");
        });

        modelBuilder.Entity<Promocija>(entity =>
        {
            entity.ToTable("Promocija");

            entity.HasIndex(e => e.Kod, "UQ_Promocija_Kod").IsUnique().HasFilter("[IsDeleted] = 0");

            entity.HasIndex(e => e.UslugaId, "IX_Promocija_UslugaId");

            entity.Property(e => e.DatumKraja).HasColumnType("datetime");
            entity.Property(e => e.DatumPocetka).HasColumnType("datetime");
            entity.Property(e => e.Kod).HasMaxLength(50);
            entity.Property(e => e.Naziv).HasMaxLength(255);
            entity.Property(e => e.Opis).HasMaxLength(1000);
            entity.Property(e => e.Popust).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Usluga).WithMany(p => p.Promocije)
                .HasForeignKey(d => d.UslugaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Promocija_Usluga");
        });

        modelBuilder.Entity<Rasa>(entity =>
        {
            entity.ToTable("Rasa");

            entity.HasIndex(e => new { e.VrstaLjubimcaId, e.Naziv }, "UQ_Rasa_Vrsta_Naziv").IsUnique().HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.Naziv).HasMaxLength(100);
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.VrstaLjubimca).WithMany(p => p.Rase)
                .HasForeignKey(d => d.VrstaLjubimcaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Rasa_VrstaLjubimca");
        });

        modelBuilder.Entity<Recenzija>(entity =>
        {
            entity.ToTable("Recenzija");

            entity.HasIndex(e => e.KorisnikId, "IX_Recenzija_KorisnikId");

            entity.HasIndex(e => e.TerminId, "UQ_Recenzija_TerminId").IsUnique().HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.DatumDodavanja)
                .HasDefaultValueSql("(getutcdate())", "DF_Recenzija_Datum")
                .HasColumnType("datetime");
            entity.Property(e => e.Komentar).HasMaxLength(1000);
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Recenzije)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Recenzija_Korisnik");

            entity.HasOne(d => d.Termin).WithOne(p => p.Recenzija)
                .HasForeignKey<Recenzija>(d => d.TerminId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Recenzija_Termin");
        });

        modelBuilder.Entity<RecenzijaOdgovor>(entity =>
        {
            entity.ToTable("RecenzijaOdgovor");

            entity.HasIndex(e => e.KorisnikId, "IX_RecOdg_KorisnikId");

            entity.HasIndex(e => e.RecenzijaId, "IX_RecOdg_RecenzijaId");

            entity.Property(e => e.DatumDodavanja)
                .HasDefaultValueSql("(getutcdate())", "DF_RecOdg_Datum")
                .HasColumnType("datetime");
            entity.Property(e => e.Komentar).HasMaxLength(1000);
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.RecenzijaOdgovori)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_RecOdg_Korisnik");

            entity.HasOne(d => d.Recenzija).WithMany(p => p.RecenzijaOdgovori)
                .HasForeignKey(d => d.RecenzijaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_RecOdg_Recenzija");
        });

        modelBuilder.Entity<ResetLozinke>(entity =>
        {
            entity.ToTable("ResetLozinke");

            entity.HasIndex(e => new { e.KorisnikId, e.JeIskoristen }, "IX_ResetLozinke_Korisnik");

            entity.Property(e => e.DatumIsteka).HasColumnType("datetime");
            entity.Property(e => e.DatumKreiranja)
                .HasDefaultValueSql("(getutcdate())", "DF_ResetLozinke_DatumKreiranja")
                .HasColumnType("datetime");
            entity.Property(e => e.KodHash).HasMaxLength(255);
            entity.Property(e => e.KodSalt).HasMaxLength(255);

            entity.HasOne(d => d.Korisnik).WithMany()
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ResetLozinke_Korisnik");
        });

        modelBuilder.Entity<Specijalnost>(entity =>
        {
            entity.ToTable("Specijalnost");

            entity.HasIndex(e => e.Naziv, "UQ_Specijalnost_Naziv").IsUnique().HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.Naziv).HasMaxLength(100);
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");
        });

        modelBuilder.Entity<StavkaTermina>(entity =>
        {
            entity.ToTable("StavkeTermina");

            entity.HasIndex(e => new { e.TerminId, e.UslugaId }, "UQ_StavkeTermina_Termin_Usluga").IsUnique().HasFilter("[IsDeleted] = 0");

            entity.HasIndex(e => e.UslugaId, "IX_StavkeTermina_UslugaId");

            entity.Property(e => e.Cijena).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Termin).WithMany(p => p.StavkeTermina)
                .HasForeignKey(d => d.TerminId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_StavkeTermina_Termin");

            entity.HasOne(d => d.Usluga).WithMany(p => p.StavkeTermina)
                .HasForeignKey(d => d.UslugaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_StavkeTermina_Usluga");
        });

        modelBuilder.Entity<Terapija>(entity =>
        {
            entity.ToTable("Terapija");

            entity.HasIndex(e => e.LjubimacId, "IX_Terapija_LjubimacId");

            entity.HasIndex(e => e.TerminId, "IX_Terapija_TerminId");

            entity.HasIndex(e => e.VeterinarId, "IX_Terapija_VeterinarId");

            entity.Property(e => e.DatumTerapije)
                .HasDefaultValueSql("(getutcdate())", "DF_Terapija_Datum")
                .HasColumnType("datetime");
            entity.Property(e => e.Dijagnoza).HasMaxLength(500);
            entity.Property(e => e.Doza).HasMaxLength(100);
            entity.Property(e => e.Lijek).HasMaxLength(255);
            entity.Property(e => e.Napomena).HasMaxLength(1000);
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Ljubimac).WithMany(p => p.Terapije)
                .HasForeignKey(d => d.LjubimacId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Terapija_Ljubimac");

            entity.HasOne(d => d.Termin).WithMany(p => p.Terapije)
                .HasForeignKey(d => d.TerminId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Terapija_Termin");

            entity.HasOne(d => d.Veterinar).WithMany(p => p.Terapije)
                .HasForeignKey(d => d.VeterinarId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Terapija_Veterinar");
        });

        modelBuilder.Entity<Termin>(entity =>
        {
            entity.ToTable("Termin");

            entity.HasIndex(e => e.Sifra, "UQ_Termin_Sifra").IsUnique();

            entity.HasIndex(e => e.KorisnikId, "IX_Termin_KorisnikId");

            entity.HasIndex(e => e.LjubimacId, "IX_Termin_LjubimacId");

            entity.HasIndex(e => new { e.VeterinarId, e.DatumTermina }, "IX_Termin_Veterinar_Datum");

            entity.Property(e => e.AdresaPrevoza).HasMaxLength(255);
            entity.Property(e => e.CijenaPrevoza).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DatumKreiranja)
                .HasDefaultValueSql("(getutcdate())", "DF_Termin_DatumKreiranja")
                .HasColumnType("datetime");
            entity.Property(e => e.DatumPromjeneStatusa).HasColumnType("datetime");
            entity.Property(e => e.IznosPopusta).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RazlogPromjeneStatusa).HasMaxLength(500);
            entity.Property(e => e.Sifra).HasMaxLength(20);
            entity.Property(e => e.UkupnaCijena).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.AktiviranaPromocija).WithMany(p => p.Termini)
                .HasForeignKey(d => d.AktiviranaPromocijaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Termin_AktPromocija");

            entity.HasOne(d => d.GradPrevoza).WithMany(p => p.Termini)
                .HasForeignKey(d => d.GradPrevozaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Termin_GradPrevoza");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.TerminiKlijenta)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Termin_Korisnik");

            entity.HasOne(d => d.Ljubimac).WithMany(p => p.Termini)
                .HasForeignKey(d => d.LjubimacId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Termin_Ljubimac");

            entity.HasOne(d => d.NacinPlacanja).WithMany(p => p.Termini)
                .HasForeignKey(d => d.NacinPlacanjaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Termin_NacinPlacanja");

            entity.HasOne(d => d.StatusPromijenio).WithMany()
                .HasForeignKey(d => d.StatusPromijenioId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Termin_StatusPromijenio");

            entity.HasOne(d => d.Veterinar).WithMany(p => p.TerminiVeterinara)
                .HasForeignKey(d => d.VeterinarId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Termin_Veterinar");
        });

        modelBuilder.Entity<Uloga>(entity =>
        {
            entity.ToTable("Uloga");

            entity.HasIndex(e => e.Naziv, "UQ_Uloga_Naziv").IsUnique().HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.Naziv).HasMaxLength(50);
            entity.Property(e => e.Opis).HasMaxLength(255);
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");
        });

        modelBuilder.Entity<Uplata>(entity =>
        {
            entity.ToTable("Uplata");

            entity.HasIndex(e => e.TerminId, "IX_Uplata_TerminId");

            // Najviše jedna otvorena (NaCekanju) ili plaćena (Placeno) uplata po terminu sprječava dvostruko plaćanje.
            entity.HasIndex(e => e.TerminId, "UQ_Uplata_Termin_Aktivna")
                .IsUnique()
                .HasFilter("[Status] IN (1, 2)");

            entity.HasIndex(e => e.PayPalOrderId, "UQ_Uplata_PayPalOrderId").IsUnique();

            entity.Property(e => e.DatumKreiranja)
                .HasDefaultValueSql("(getutcdate())", "DF_Uplata_DatumKreiranja")
                .HasColumnType("datetime");
            entity.Property(e => e.DatumPlacanja).HasColumnType("datetime");
            entity.Property(e => e.DatumRefunda).HasColumnType("datetime");
            entity.Property(e => e.Iznos).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.IznosRefunda).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PayPalCaptureId).HasMaxLength(100);
            entity.Property(e => e.PayPalOrderId).HasMaxLength(100);
            entity.Property(e => e.PayPalRefundId).HasMaxLength(100);
            entity.Property(e => e.Valuta).HasMaxLength(3);

            entity.HasOne(d => d.Termin).WithMany(p => p.Uplate)
                .HasForeignKey(d => d.TerminId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Uplata_Termin");
        });

        modelBuilder.Entity<Usluga>(entity =>
        {
            entity.ToTable("Usluga");

            entity.HasIndex(e => e.VrstaId, "IX_Usluga_VrstaId");

            entity.Property(e => e.Cijena).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DatumObjavljivanja)
                .HasDefaultValueSql("(getutcdate())", "DF_Usluga_Datum")
                .HasColumnType("datetime");
            entity.Property(e => e.Naziv).HasMaxLength(255);
            entity.Property(e => e.Opis).HasMaxLength(2000);
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Vrsta).WithMany(p => p.Usluge)
                .HasForeignKey(d => d.VrstaId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Usluga_Vrsta");
        });

        modelBuilder.Entity<VrstaLjubimca>(entity =>
        {
            entity.ToTable("VrstaLjubimca");

            entity.HasIndex(e => e.Naziv, "UQ_VrstaLjubimca_Naziv").IsUnique().HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.Naziv).HasMaxLength(100);
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");
        });

        modelBuilder.Entity<VrstaUsluge>(entity =>
        {
            entity.HasKey(e => e.VrstaId);

            entity.ToTable("VrstaUsluge");

            entity.HasIndex(e => e.Naziv, "UQ_VrstaUsluge_Naziv").IsUnique().HasFilter("[IsDeleted] = 0");

            entity.Property(e => e.Naziv).HasMaxLength(100);
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");
        });

        DodajSoftDeleteFiltere(modelBuilder);
        OznaciUtcVremena(modelBuilder);

        SeedReferentnihPodataka(modelBuilder);
        SeedTestnihPodataka(modelBuilder);
    }

    // Vremena se čuvaju u UTC-u (DateTime.UtcNow); pri čitanju se označavaju kao UTC kako bi API vratio oznaku "Z"
    // i klijent ih ispravno prikazao u lokalnoj zoni. Vrijeme obaveze je lokalno vrijeme koje bira veterinar.
    private static void OznaciUtcVremena(ModelBuilder modelBuilder)
    {
        var utc = new ValueConverter<DateTime, DateTime>(
            v => v.Kind == DateTimeKind.Local ? v.ToUniversalTime() : v,
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        var utcNullable = new ValueConverter<DateTime?, DateTime?>(
            v => v.HasValue && v.Value.Kind == DateTimeKind.Local ? v.Value.ToUniversalTime() : v,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()))
        {
            if (property.DeclaringType.ClrType == typeof(Obaveza) && property.Name == nameof(Obaveza.DatumVrijeme))
            {
                continue;
            }

            if (property.ClrType == typeof(DateTime))
            {
                property.SetValueConverter(utc);
            }
            else if (property.ClrType == typeof(DateTime?))
            {
                property.SetValueConverter(utcNullable);
            }
        }
    }

    private static void DodajSoftDeleteFiltere(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                     .Where(t => typeof(ISoftDelete).IsAssignableFrom(t.ClrType)))
        {
            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var filter = Expression.Lambda(
                Expression.Not(Expression.Property(parameter, nameof(ISoftDelete.IsDeleted))),
                parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
        }
    }
}
