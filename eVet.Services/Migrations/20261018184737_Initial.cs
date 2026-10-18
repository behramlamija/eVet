using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eVet.Services.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grad",
                columns: table => new
                {
                    GradId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostanskiBroj = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grad", x => x.GradId);
                });

            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    KorisnikId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    KorisnickoIme = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Slika = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LozinkaHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LozinkaSalt = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    JeAktivan = table.Column<bool>(type: "bit", nullable: false),
                    DatumRegistracije = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Korisnik_DatumReg"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.KorisnikId);
                });

            migrationBuilder.CreateTable(
                name: "NacinPlacanja",
                columns: table => new
                {
                    NacinPlacanjaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NacinPlacanja", x => x.NacinPlacanjaId);
                });

            migrationBuilder.CreateTable(
                name: "Uloga",
                columns: table => new
                {
                    UlogaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uloga", x => x.UlogaId);
                });

            migrationBuilder.CreateTable(
                name: "VrstaLjubimca",
                columns: table => new
                {
                    VrstaLjubimcaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VrstaLjubimca", x => x.VrstaLjubimcaId);
                });

            migrationBuilder.CreateTable(
                name: "VrstaUsluge",
                columns: table => new
                {
                    VrstaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slika = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VrstaUsluge", x => x.VrstaId);
                });

            migrationBuilder.CreateTable(
                name: "Obaveza",
                columns: table => new
                {
                    ObavezaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VeterinarId = table.Column<int>(type: "int", nullable: false),
                    Naslov = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DatumVrijeme = table.Column<DateTime>(type: "datetime", nullable: false),
                    JeZavrsena = table.Column<bool>(type: "bit", nullable: false),
                    DatumKreiranja = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Obaveza_DatumKreiranja"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obaveza", x => x.ObavezaId);
                    table.ForeignKey(
                        name: "FK_Obaveza_Veterinar",
                        column: x => x.VeterinarId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Obavijest",
                columns: table => new
                {
                    ObavijestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    Naslov = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Sadrzaj = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    JePogledana = table.Column<bool>(type: "bit", nullable: false),
                    DatumObavijesti = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Obavijest_Datum"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obavijest", x => x.ObavijestId);
                    table.ForeignKey(
                        name: "FK_Obavijest_Korisnik",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KorisniciUloge",
                columns: table => new
                {
                    KorisnikUlogaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    UlogaId = table.Column<int>(type: "int", nullable: false),
                    DatumDodavanja = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                        .Annotation("Relational:DefaultConstraintName", "DF_KorUloge_Datum"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KorisniciUloge", x => x.KorisnikUlogaId);
                    table.ForeignKey(
                        name: "FK_KorUloge_Korisnik",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KorUloge_Uloga",
                        column: x => x.UlogaId,
                        principalTable: "Uloga",
                        principalColumn: "UlogaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rasa",
                columns: table => new
                {
                    RasaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VrstaLjubimcaId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rasa", x => x.RasaId);
                    table.ForeignKey(
                        name: "FK_Rasa_VrstaLjubimca",
                        column: x => x.VrstaLjubimcaId,
                        principalTable: "VrstaLjubimca",
                        principalColumn: "VrstaLjubimcaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usluga",
                columns: table => new
                {
                    UslugaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Cijena = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Trajanje = table.Column<int>(type: "int", nullable: false),
                    Slika = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DatumObjavljivanja = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Usluga_Datum"),
                    VrstaId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usluga", x => x.UslugaId);
                    table.ForeignKey(
                        name: "FK_Usluga_Vrsta",
                        column: x => x.VrstaId,
                        principalTable: "VrstaUsluge",
                        principalColumn: "VrstaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ljubimac",
                columns: table => new
                {
                    LjubimacId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RasaId = table.Column<int>(type: "int", nullable: false),
                    DatumRodjenja = table.Column<DateOnly>(type: "date", nullable: true),
                    Spol = table.Column<int>(type: "int", nullable: false),
                    Tezina = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Napomena = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Slika = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ljubimac", x => x.LjubimacId);
                    table.ForeignKey(
                        name: "FK_Ljubimac_Korisnik",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ljubimac_Rasa",
                        column: x => x.RasaId,
                        principalTable: "Rasa",
                        principalColumn: "RasaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Promocija",
                columns: table => new
                {
                    PromocijaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Kod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Popust = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    DatumPocetka = table.Column<DateTime>(type: "datetime", nullable: false),
                    DatumKraja = table.Column<DateTime>(type: "datetime", nullable: false),
                    UslugaId = table.Column<int>(type: "int", nullable: false),
                    JeAktivna = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promocija", x => x.PromocijaId);
                    table.ForeignKey(
                        name: "FK_Promocija_Usluga",
                        column: x => x.UslugaId,
                        principalTable: "Usluga",
                        principalColumn: "UslugaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AktiviranaPromocija",
                columns: table => new
                {
                    AktiviranaPromocijaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromocijaId = table.Column<int>(type: "int", nullable: false),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    Iskoristena = table.Column<bool>(type: "bit", nullable: false),
                    DatumAktiviranja = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                        .Annotation("Relational:DefaultConstraintName", "DF_AktProm_Datum"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AktiviranaPromocija", x => x.AktiviranaPromocijaId);
                    table.ForeignKey(
                        name: "FK_AktProm_Korisnik",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AktProm_Promocija",
                        column: x => x.PromocijaId,
                        principalTable: "Promocija",
                        principalColumn: "PromocijaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Termin",
                columns: table => new
                {
                    TerminId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    VeterinarId = table.Column<int>(type: "int", nullable: false),
                    LjubimacId = table.Column<int>(type: "int", nullable: false),
                    Sifra = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DatumTermina = table.Column<DateOnly>(type: "date", nullable: false),
                    VrijemePocetka = table.Column<TimeOnly>(type: "time", nullable: false),
                    VrijemeKraja = table.Column<TimeOnly>(type: "time", nullable: false),
                    UkupnoTrajanje = table.Column<int>(type: "int", nullable: false),
                    IznosPopusta = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CijenaPrevoza = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UkupnaCijena = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    NacinPlacanjaId = table.Column<int>(type: "int", nullable: false),
                    AktiviranaPromocijaId = table.Column<int>(type: "int", nullable: true),
                    JePrevoz = table.Column<bool>(type: "bit", nullable: false),
                    AdresaPrevoza = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GradPrevozaId = table.Column<int>(type: "int", nullable: true),
                    DatumKreiranja = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Termin_DatumKreiranja"),
                    DatumPromjeneStatusa = table.Column<DateTime>(type: "datetime", nullable: true),
                    StatusPromijenioId = table.Column<int>(type: "int", nullable: true),
                    RazlogPromjeneStatusa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Termin", x => x.TerminId);
                    table.ForeignKey(
                        name: "FK_Termin_AktPromocija",
                        column: x => x.AktiviranaPromocijaId,
                        principalTable: "AktiviranaPromocija",
                        principalColumn: "AktiviranaPromocijaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Termin_GradPrevoza",
                        column: x => x.GradPrevozaId,
                        principalTable: "Grad",
                        principalColumn: "GradId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Termin_Korisnik",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Termin_Ljubimac",
                        column: x => x.LjubimacId,
                        principalTable: "Ljubimac",
                        principalColumn: "LjubimacId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Termin_NacinPlacanja",
                        column: x => x.NacinPlacanjaId,
                        principalTable: "NacinPlacanja",
                        principalColumn: "NacinPlacanjaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Termin_StatusPromijenio",
                        column: x => x.StatusPromijenioId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Termin_Veterinar",
                        column: x => x.VeterinarId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recenzija",
                columns: table => new
                {
                    RecenzijaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    TerminId = table.Column<int>(type: "int", nullable: false),
                    UslugaPozitivna = table.Column<bool>(type: "bit", nullable: false),
                    VeterinarPozitivan = table.Column<bool>(type: "bit", nullable: false),
                    Komentar = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DatumDodavanja = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Recenzija_Datum"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recenzija", x => x.RecenzijaId);
                    table.ForeignKey(
                        name: "FK_Recenzija_Korisnik",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recenzija_Termin",
                        column: x => x.TerminId,
                        principalTable: "Termin",
                        principalColumn: "TerminId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StavkeTermina",
                columns: table => new
                {
                    StavkaTerminaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TerminId = table.Column<int>(type: "int", nullable: false),
                    UslugaId = table.Column<int>(type: "int", nullable: false),
                    Cijena = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StavkeTermina", x => x.StavkaTerminaId);
                    table.ForeignKey(
                        name: "FK_StavkeTermina_Termin",
                        column: x => x.TerminId,
                        principalTable: "Termin",
                        principalColumn: "TerminId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StavkeTermina_Usluga",
                        column: x => x.UslugaId,
                        principalTable: "Usluga",
                        principalColumn: "UslugaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Terapija",
                columns: table => new
                {
                    TerapijaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LjubimacId = table.Column<int>(type: "int", nullable: false),
                    VeterinarId = table.Column<int>(type: "int", nullable: false),
                    TerminId = table.Column<int>(type: "int", nullable: true),
                    Dijagnoza = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Lijek = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Doza = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Napomena = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DatumTerapije = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Terapija_Datum"),
                    DatumKontrolnog = table.Column<DateOnly>(type: "date", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terapija", x => x.TerapijaId);
                    table.ForeignKey(
                        name: "FK_Terapija_Ljubimac",
                        column: x => x.LjubimacId,
                        principalTable: "Ljubimac",
                        principalColumn: "LjubimacId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Terapija_Termin",
                        column: x => x.TerminId,
                        principalTable: "Termin",
                        principalColumn: "TerminId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Terapija_Veterinar",
                        column: x => x.VeterinarId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Uplata",
                columns: table => new
                {
                    UplataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TerminId = table.Column<int>(type: "int", nullable: false),
                    Iznos = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Valuta = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PayPalOrderId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PayPalCaptureId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DatumKreiranja = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                        .Annotation("Relational:DefaultConstraintName", "DF_Uplata_DatumKreiranja"),
                    DatumPlacanja = table.Column<DateTime>(type: "datetime", nullable: true),
                    IznosRefunda = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PayPalRefundId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DatumRefunda = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uplata", x => x.UplataId);
                    table.ForeignKey(
                        name: "FK_Uplata_Termin",
                        column: x => x.TerminId,
                        principalTable: "Termin",
                        principalColumn: "TerminId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecenzijaOdgovor",
                columns: table => new
                {
                    RecenzijaOdgovorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecenzijaId = table.Column<int>(type: "int", nullable: false),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    Komentar = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DatumDodavanja = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                        .Annotation("Relational:DefaultConstraintName", "DF_RecOdg_Datum"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecenzijaOdgovor", x => x.RecenzijaOdgovorId);
                    table.ForeignKey(
                        name: "FK_RecOdg_Korisnik",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecOdg_Recenzija",
                        column: x => x.RecenzijaId,
                        principalTable: "Recenzija",
                        principalColumn: "RecenzijaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Grad",
                columns: new[] { "GradId", "IsDeleted", "Naziv", "PostanskiBroj", "VrijemeBrisanja" },
                values: new object[,]
                {
                    { 1, false, "Mostar", "88000", null },
                    { 2, false, "Sarajevo", "71000", null },
                    { 3, false, "Tuzla", "75000", null },
                    { 4, false, "Zenica", "72000", null },
                    { 5, false, "Banja Luka", "78000", null },
                    { 6, false, "Bihać", "77000", null },
                    { 7, false, "Široki Brijeg", "88220", null },
                    { 8, false, "Čapljina", "88300", null },
                    { 9, false, "Konjic", "88400", null },
                    { 10, false, "Stolac", "88360", null }
                });

            migrationBuilder.InsertData(
                table: "NacinPlacanja",
                columns: new[] { "NacinPlacanjaId", "IsDeleted", "Naziv", "VrijemeBrisanja" },
                values: new object[,]
                {
                    { 1, false, "Gotovina", null },
                    { 2, false, "PayPal", null }
                });

            migrationBuilder.InsertData(
                table: "Uloga",
                columns: new[] { "UlogaId", "IsDeleted", "Naziv", "Opis", "VrijemeBrisanja" },
                values: new object[,]
                {
                    { 1, false, "Vlasnik", "Vlasnik veterinarske stanice", null },
                    { 2, false, "Veterinar", "Veterinar uposlen u stanici", null },
                    { 3, false, "Klijent", "Vlasnik kućnog ljubimca", null }
                });

            migrationBuilder.InsertData(
                table: "VrstaLjubimca",
                columns: new[] { "VrstaLjubimcaId", "IsDeleted", "Naziv", "VrijemeBrisanja" },
                values: new object[,]
                {
                    { 1, false, "Pas", null },
                    { 2, false, "Mačka", null },
                    { 3, false, "Ptica", null },
                    { 4, false, "Mali sisar", null }
                });

            migrationBuilder.InsertData(
                table: "VrstaUsluge",
                columns: new[] { "VrstaId", "IsDeleted", "Naziv", "Slika", "VrijemeBrisanja" },
                values: new object[,]
                {
                    { 1, false, "Pregledi", null, null },
                    { 2, false, "Vakcinacija", null, null },
                    { 3, false, "Šišanje i njega", null, null },
                    { 4, false, "Stomatologija", null, null },
                    { 5, false, "Operacije", null, null },
                    { 6, false, "Laboratorija", null, null }
                });

            migrationBuilder.InsertData(
                table: "Rasa",
                columns: new[] { "RasaId", "IsDeleted", "Naziv", "VrijemeBrisanja", "VrstaLjubimcaId" },
                values: new object[,]
                {
                    { 1, false, "Mješanac", null, 1 },
                    { 2, false, "Njemački ovčar", null, 1 },
                    { 3, false, "Labrador retriver", null, 1 },
                    { 4, false, "Zlatni retriver", null, 1 },
                    { 5, false, "Francuski buldog", null, 1 },
                    { 6, false, "Tornjak", null, 1 },
                    { 7, false, "Maltezer", null, 1 },
                    { 8, false, "Mješanac", null, 2 },
                    { 9, false, "Perzijska", null, 2 },
                    { 10, false, "Britanska kratkodlaka", null, 2 },
                    { 11, false, "Sijamska", null, 2 },
                    { 12, false, "Papagaj", null, 3 },
                    { 13, false, "Kanarinac", null, 3 },
                    { 14, false, "Hrčak", null, 4 },
                    { 15, false, "Zamorac", null, 4 },
                    { 16, false, "Kunić", null, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AktProm_PromocijaId",
                table: "AktiviranaPromocija",
                column: "PromocijaId");

            migrationBuilder.CreateIndex(
                name: "UQ_AktProm_Korisnik_Promocija",
                table: "AktiviranaPromocija",
                columns: new[] { "KorisnikId", "PromocijaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Grad_Naziv",
                table: "Grad",
                column: "Naziv",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KorisniciUloge_UlogaId",
                table: "KorisniciUloge",
                column: "UlogaId");

            migrationBuilder.CreateIndex(
                name: "UQ_KorisniciUloge_Korisnik_Uloga",
                table: "KorisniciUloge",
                columns: new[] { "KorisnikId", "UlogaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Korisnik_Email",
                table: "Korisnik",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Korisnik_KorisnickoIme",
                table: "Korisnik",
                column: "KorisnickoIme",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ljubimac_KorisnikId",
                table: "Ljubimac",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Ljubimac_RasaId",
                table: "Ljubimac",
                column: "RasaId");

            migrationBuilder.CreateIndex(
                name: "UQ_NacinPlacanja_Naziv",
                table: "NacinPlacanja",
                column: "Naziv",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Obaveza_Veterinar_Datum",
                table: "Obaveza",
                columns: new[] { "VeterinarId", "DatumVrijeme" });

            migrationBuilder.CreateIndex(
                name: "IX_Obavijest_KorisnikId",
                table: "Obavijest",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Promocija_UslugaId",
                table: "Promocija",
                column: "UslugaId");

            migrationBuilder.CreateIndex(
                name: "UQ_Promocija_Kod",
                table: "Promocija",
                column: "Kod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Rasa_Vrsta_Naziv",
                table: "Rasa",
                columns: new[] { "VrstaLjubimcaId", "Naziv" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recenzija_KorisnikId",
                table: "Recenzija",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "UQ_Recenzija_TerminId",
                table: "Recenzija",
                column: "TerminId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecOdg_KorisnikId",
                table: "RecenzijaOdgovor",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_RecOdg_RecenzijaId",
                table: "RecenzijaOdgovor",
                column: "RecenzijaId");

            migrationBuilder.CreateIndex(
                name: "IX_StavkeTermina_UslugaId",
                table: "StavkeTermina",
                column: "UslugaId");

            migrationBuilder.CreateIndex(
                name: "UQ_StavkeTermina_Termin_Usluga",
                table: "StavkeTermina",
                columns: new[] { "TerminId", "UslugaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Terapija_LjubimacId",
                table: "Terapija",
                column: "LjubimacId");

            migrationBuilder.CreateIndex(
                name: "IX_Terapija_TerminId",
                table: "Terapija",
                column: "TerminId");

            migrationBuilder.CreateIndex(
                name: "IX_Terapija_VeterinarId",
                table: "Terapija",
                column: "VeterinarId");

            migrationBuilder.CreateIndex(
                name: "IX_Termin_AktiviranaPromocijaId",
                table: "Termin",
                column: "AktiviranaPromocijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Termin_GradPrevozaId",
                table: "Termin",
                column: "GradPrevozaId");

            migrationBuilder.CreateIndex(
                name: "IX_Termin_KorisnikId",
                table: "Termin",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Termin_LjubimacId",
                table: "Termin",
                column: "LjubimacId");

            migrationBuilder.CreateIndex(
                name: "IX_Termin_NacinPlacanjaId",
                table: "Termin",
                column: "NacinPlacanjaId");

            migrationBuilder.CreateIndex(
                name: "IX_Termin_StatusPromijenioId",
                table: "Termin",
                column: "StatusPromijenioId");

            migrationBuilder.CreateIndex(
                name: "IX_Termin_Veterinar_Datum",
                table: "Termin",
                columns: new[] { "VeterinarId", "DatumTermina" });

            migrationBuilder.CreateIndex(
                name: "UQ_Termin_Sifra",
                table: "Termin",
                column: "Sifra",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Uloga_Naziv",
                table: "Uloga",
                column: "Naziv",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Uplata_TerminId",
                table: "Uplata",
                column: "TerminId");

            migrationBuilder.CreateIndex(
                name: "UQ_Uplata_PayPalOrderId",
                table: "Uplata",
                column: "PayPalOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usluga_VrstaId",
                table: "Usluga",
                column: "VrstaId");

            migrationBuilder.CreateIndex(
                name: "UQ_VrstaLjubimca_Naziv",
                table: "VrstaLjubimca",
                column: "Naziv",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_VrstaUsluge_Naziv",
                table: "VrstaUsluge",
                column: "Naziv",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KorisniciUloge");

            migrationBuilder.DropTable(
                name: "Obaveza");

            migrationBuilder.DropTable(
                name: "Obavijest");

            migrationBuilder.DropTable(
                name: "RecenzijaOdgovor");

            migrationBuilder.DropTable(
                name: "StavkeTermina");

            migrationBuilder.DropTable(
                name: "Terapija");

            migrationBuilder.DropTable(
                name: "Uplata");

            migrationBuilder.DropTable(
                name: "Uloga");

            migrationBuilder.DropTable(
                name: "Recenzija");

            migrationBuilder.DropTable(
                name: "Termin");

            migrationBuilder.DropTable(
                name: "AktiviranaPromocija");

            migrationBuilder.DropTable(
                name: "Grad");

            migrationBuilder.DropTable(
                name: "Ljubimac");

            migrationBuilder.DropTable(
                name: "NacinPlacanja");

            migrationBuilder.DropTable(
                name: "Promocija");

            migrationBuilder.DropTable(
                name: "Korisnik");

            migrationBuilder.DropTable(
                name: "Rasa");

            migrationBuilder.DropTable(
                name: "Usluga");

            migrationBuilder.DropTable(
                name: "VrstaLjubimca");

            migrationBuilder.DropTable(
                name: "VrstaUsluge");
        }
    }
}
