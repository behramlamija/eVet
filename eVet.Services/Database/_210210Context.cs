using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace eVet.Services.Database;

public partial class _210210Context : DbContext
{
    public _210210Context()
    {
    }

    public _210210Context(DbContextOptions<_210210Context> options)
        : base(options)
    {
    }

    public virtual DbSet<AktiviranaPromocija> AktiviranaPromocijas { get; set; }

    public virtual DbSet<KorisniciUloge> KorisniciUloges { get; set; }

    public virtual DbSet<Korisnik> Korisniks { get; set; }

    public virtual DbSet<Ljubimac> Ljubimacs { get; set; }

    public virtual DbSet<NacinPlacanja> NacinPlacanjas { get; set; }

    public virtual DbSet<Obavijest> Obavijests { get; set; }

    public virtual DbSet<Promocija> Promocijas { get; set; }

    public virtual DbSet<Recenzija> Recenzijas { get; set; }

    public virtual DbSet<RecenzijaOdgovor> RecenzijaOdgovors { get; set; }

    public virtual DbSet<StavkeTermina> StavkeTerminas { get; set; }

    public virtual DbSet<Terapija> Terapijas { get; set; }

    public virtual DbSet<Termin> Termins { get; set; }

    public virtual DbSet<Uloga> Ulogas { get; set; }

    public virtual DbSet<Usluga> Uslugas { get; set; }

    public virtual DbSet<VrstaUsluge> VrstaUsluges { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=210210;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AktiviranaPromocija>(entity =>
        {
            entity.ToTable("AktiviranaPromocija");

            entity.HasIndex(e => e.KorisnikId, "IX_AktProm_KorisnikId");

            entity.HasIndex(e => e.PromocijaId, "IX_AktProm_PromocijaId");

            entity.Property(e => e.DatumAktiviranja)
                .HasDefaultValueSql("(getdate())", "DF_AktProm_Datum")
                .HasColumnType("datetime");
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.AktiviranaPromocijas)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AktProm_Korisnik");

            entity.HasOne(d => d.Promocija).WithMany(p => p.AktiviranaPromocijas)
                .HasForeignKey(d => d.PromocijaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AktProm_Promocija");
        });

        modelBuilder.Entity<KorisniciUloge>(entity =>
        {
            entity.HasKey(e => e.KorisnikUlogaId);

            entity.ToTable("KorisniciUloge");

            entity.HasIndex(e => e.KorisnikId, "IX_KorisniciUloge_KorisnikId");

            entity.HasIndex(e => e.UlogaId, "IX_KorisniciUloge_UlogaId");

            entity.Property(e => e.DatumDodavanja)
                .HasDefaultValueSql("(getdate())", "DF_KorUloge_Datum")
                .HasColumnType("datetime");
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.KorisniciUloges)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KorUloge_Korisnik");

            entity.HasOne(d => d.Uloga).WithMany(p => p.KorisniciUloges)
                .HasForeignKey(d => d.UlogaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KorUloge_Uloga");
        });

        modelBuilder.Entity<Korisnik>(entity =>
        {
            entity.ToTable("Korisnik");

            entity.HasIndex(e => e.Email, "UQ_Korisnik_Email").IsUnique();

            entity.HasIndex(e => e.KorisnickoIme, "UQ_Korisnik_KorisnickoIme").IsUnique();

            entity.Property(e => e.DatumRegistracije)
                .HasDefaultValueSql("(getdate())", "DF_Korisnik_DatumReg")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Ime).HasMaxLength(100);
            entity.Property(e => e.JeAktivan).HasDefaultValue(true, "DF_Korisnik_JeAktivan");
            entity.Property(e => e.KorisnickoIme).HasMaxLength(50);
            entity.Property(e => e.Prezime).HasMaxLength(100);
            entity.Property(e => e.Telefon).HasMaxLength(50);
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");
        });

        modelBuilder.Entity<Ljubimac>(entity =>
        {
            entity.ToTable("Ljubimac");

            entity.HasIndex(e => e.KorisnikId, "IX_Ljubimac_KorisnikId");

            entity.Property(e => e.Ime).HasMaxLength(100);
            entity.Property(e => e.Napomena).HasColumnType("text");
            entity.Property(e => e.Rasa).HasMaxLength(100);
            entity.Property(e => e.Spol).HasMaxLength(10);
            entity.Property(e => e.Tezina).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");
            entity.Property(e => e.Vrsta).HasMaxLength(50);

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Ljubimacs)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ljubimac_Korisnik");
        });

        modelBuilder.Entity<NacinPlacanja>(entity =>
        {
            entity.ToTable("NacinPlacanja");

            entity.Property(e => e.Naziv).HasMaxLength(100);
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");
        });

        modelBuilder.Entity<Obavijest>(entity =>
        {
            entity.ToTable("Obavijest");

            entity.HasIndex(e => e.KorisnikId, "IX_Obavijest_KorisnikId");

            entity.Property(e => e.DatumObavijesti)
                .HasDefaultValueSql("(getdate())", "DF_Obavijest_Datum")
                .HasColumnType("datetime");
            entity.Property(e => e.Naslov).HasMaxLength(255);
            entity.Property(e => e.Sadrzaj).HasColumnType("text");
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Obavijests)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Obavijest_Korisnik");
        });

        modelBuilder.Entity<Promocija>(entity =>
        {
            entity.ToTable("Promocija");

            entity.HasIndex(e => e.UslugaId, "IX_Promocija_UslugaId");

            entity.Property(e => e.DatumKraja)
                .HasDefaultValueSql("(getdate())", "DF_Promocija_Kraj")
                .HasColumnType("datetime");
            entity.Property(e => e.DatumPocetka)
                .HasDefaultValueSql("(getdate())", "DF_Promocija_Pocetak")
                .HasColumnType("datetime");
            entity.Property(e => e.Kod).HasMaxLength(150);
            entity.Property(e => e.Naziv).HasMaxLength(255);
            entity.Property(e => e.Opis).HasColumnType("text");
            entity.Property(e => e.Popust).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status).HasDefaultValue(true, "DF_Promocija_Status");
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Usluga).WithMany(p => p.Promocijas)
                .HasForeignKey(d => d.UslugaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Promocija_Usluga");
        });

        modelBuilder.Entity<Recenzija>(entity =>
        {
            entity.ToTable("Recenzija");

            entity.HasIndex(e => e.KorisnikId, "IX_Recenzija_KorisnikId");

            entity.HasIndex(e => e.UslugaId, "IX_Recenzija_UslugaId");

            entity.Property(e => e.BrojDislajkova).HasDefaultValue(0, "DF_Recenzija_Dislajkovi");
            entity.Property(e => e.BrojLajkova).HasDefaultValue(0, "DF_Recenzija_Lajkovi");
            entity.Property(e => e.DatumDodavanja)
                .HasDefaultValueSql("(getdate())", "DF_Recenzija_Datum")
                .HasColumnType("datetime");
            entity.Property(e => e.Komentar).HasColumnType("text");
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.Recenzijas)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recenzija_Korisnik");

            entity.HasOne(d => d.Usluga).WithMany(p => p.Recenzijas)
                .HasForeignKey(d => d.UslugaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recenzija_Usluga");
        });

        modelBuilder.Entity<RecenzijaOdgovor>(entity =>
        {
            entity.ToTable("RecenzijaOdgovor");

            entity.HasIndex(e => e.RecenzijaId, "IX_RecOdg_RecenzijaId");

            entity.Property(e => e.BrojDislajkova).HasDefaultValue(0, "DF_RecOdg_Dislajkovi");
            entity.Property(e => e.BrojLajkova).HasDefaultValue(0, "DF_RecOdg_Lajkovi");
            entity.Property(e => e.DatumDodavanja)
                .HasDefaultValueSql("(getdate())", "DF_RecOdg_Datum")
                .HasColumnType("datetime");
            entity.Property(e => e.Komentar).HasColumnType("text");
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.RecenzijaOdgovors)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecOdg_Korisnik");

            entity.HasOne(d => d.Recenzija).WithMany(p => p.RecenzijaOdgovors)
                .HasForeignKey(d => d.RecenzijaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RecOdg_Recenzija");
        });

        modelBuilder.Entity<StavkeTermina>(entity =>
        {
            entity.ToTable("StavkeTermina");

            entity.HasIndex(e => e.TerminId, "IX_StavkeTermina_TerminId");

            entity.HasIndex(e => e.UslugaId, "IX_StavkeTermina_UslugaId");

            entity.Property(e => e.Cijena).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Termin).WithMany(p => p.StavkeTerminas)
                .HasForeignKey(d => d.TerminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StavkeTermina_Termin");

            entity.HasOne(d => d.Usluga).WithMany(p => p.StavkeTerminas)
                .HasForeignKey(d => d.UslugaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StavkeTermina_Usluga");
        });

        modelBuilder.Entity<Terapija>(entity =>
        {
            entity.ToTable("Terapija");

            entity.HasIndex(e => e.LjubimacId, "IX_Terapija_LjubimacId");

            entity.HasIndex(e => e.VeterinarId, "IX_Terapija_VeterinarId");

            entity.Property(e => e.DatumTerapije)
                .HasDefaultValueSql("(getdate())", "DF_Terapija_Datum")
                .HasColumnType("datetime");
            entity.Property(e => e.Dijagnoza).HasMaxLength(500);
            entity.Property(e => e.Doza).HasMaxLength(100);
            entity.Property(e => e.Lijek).HasMaxLength(255);
            entity.Property(e => e.Napomena).HasColumnType("text");
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Ljubimac).WithMany(p => p.Terapijas)
                .HasForeignKey(d => d.LjubimacId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Terapija_Ljubimac");

            entity.HasOne(d => d.Termin).WithMany(p => p.Terapijas)
                .HasForeignKey(d => d.TerminId)
                .HasConstraintName("FK_Terapija_Termin");

            entity.HasOne(d => d.Veterinar).WithMany(p => p.Terapijas)
                .HasForeignKey(d => d.VeterinarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Terapija_Veterinar");
        });

        modelBuilder.Entity<Termin>(entity =>
        {
            entity.ToTable("Termin");

            entity.HasIndex(e => e.KorisnikId, "IX_Termin_KorisnikId");

            entity.HasIndex(e => e.LjubimacId, "IX_Termin_LjubimacId");

            entity.HasIndex(e => e.VeterinarId, "IX_Termin_VeterinarId");

            entity.Property(e => e.DatumTermina).HasDefaultValueSql("(getdate())", "DF_Termin_Datum");
            entity.Property(e => e.Sifra).HasMaxLength(150);
            entity.Property(e => e.StateMachine).HasMaxLength(100);
            entity.Property(e => e.UkupnaCijena).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.AktiviranaPromocija).WithMany(p => p.Termins)
                .HasForeignKey(d => d.AktiviranaPromocijaId)
                .HasConstraintName("FK_Termin_AktPromocija");

            entity.HasOne(d => d.Korisnik).WithMany(p => p.TerminKorisniks)
                .HasForeignKey(d => d.KorisnikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Termin_Korisnik");

            entity.HasOne(d => d.Ljubimac).WithMany(p => p.Termins)
                .HasForeignKey(d => d.LjubimacId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Termin_Ljubimac");

            entity.HasOne(d => d.NacinPlacanja).WithMany(p => p.Termins)
                .HasForeignKey(d => d.NacinPlacanjaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Termin_NacinPlacanja");

            entity.HasOne(d => d.Veterinar).WithMany(p => p.TerminVeterinars)
                .HasForeignKey(d => d.VeterinarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Termin_Veterinar");
        });

        modelBuilder.Entity<Uloga>(entity =>
        {
            entity.ToTable("Uloga");

            entity.Property(e => e.Naziv).HasMaxLength(50);
            entity.Property(e => e.Opis).HasMaxLength(255);
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");
        });

        modelBuilder.Entity<Usluga>(entity =>
        {
            entity.ToTable("Usluga");

            entity.HasIndex(e => e.VrstaId, "IX_Usluga_VrstaId");

            entity.Property(e => e.Cijena).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.DatumObjavljivanja)
                .HasDefaultValueSql("(getdate())", "DF_Usluga_Datum")
                .HasColumnType("datetime");
            entity.Property(e => e.Naziv).HasMaxLength(255);
            entity.Property(e => e.Opis).HasColumnType("text");
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");

            entity.HasOne(d => d.Vrsta).WithMany(p => p.Uslugas)
                .HasForeignKey(d => d.VrstaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usluga_Vrsta");
        });

        modelBuilder.Entity<VrstaUsluge>(entity =>
        {
            entity.HasKey(e => e.VrstaId);

            entity.ToTable("VrstaUsluge");

            entity.Property(e => e.Naziv).HasMaxLength(100);
            entity.Property(e => e.VrijemeBrisanja).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
