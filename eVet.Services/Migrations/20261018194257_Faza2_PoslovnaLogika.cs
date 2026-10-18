using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eVet.Services.Migrations
{
    /// <inheritdoc />
    public partial class Faza2_PoslovnaLogika : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_VrstaUsluge_Naziv",
                table: "VrstaUsluge");

            migrationBuilder.DropIndex(
                name: "UQ_VrstaLjubimca_Naziv",
                table: "VrstaLjubimca");

            migrationBuilder.DropIndex(
                name: "UQ_Uloga_Naziv",
                table: "Uloga");

            migrationBuilder.DropIndex(
                name: "UQ_StavkeTermina_Termin_Usluga",
                table: "StavkeTermina");

            migrationBuilder.DropIndex(
                name: "UQ_Recenzija_TerminId",
                table: "Recenzija");

            migrationBuilder.DropIndex(
                name: "UQ_Rasa_Vrsta_Naziv",
                table: "Rasa");

            migrationBuilder.DropIndex(
                name: "UQ_Promocija_Kod",
                table: "Promocija");

            migrationBuilder.DropIndex(
                name: "UQ_NacinPlacanja_Naziv",
                table: "NacinPlacanja");

            migrationBuilder.DropIndex(
                name: "UQ_Korisnik_Email",
                table: "Korisnik");

            migrationBuilder.DropIndex(
                name: "UQ_Korisnik_KorisnickoIme",
                table: "Korisnik");

            migrationBuilder.DropIndex(
                name: "UQ_Grad_Naziv",
                table: "Grad");

            migrationBuilder.DropIndex(
                name: "UQ_AktProm_Korisnik_Promocija",
                table: "AktiviranaPromocija");

            migrationBuilder.AddColumn<bool>(
                name: "JeOperacija",
                table: "VrstaUsluge",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "VrstaUsluge",
                keyColumn: "VrstaId",
                keyValue: 1,
                column: "JeOperacija",
                value: false);

            migrationBuilder.UpdateData(
                table: "VrstaUsluge",
                keyColumn: "VrstaId",
                keyValue: 2,
                column: "JeOperacija",
                value: false);

            migrationBuilder.UpdateData(
                table: "VrstaUsluge",
                keyColumn: "VrstaId",
                keyValue: 3,
                column: "JeOperacija",
                value: false);

            migrationBuilder.UpdateData(
                table: "VrstaUsluge",
                keyColumn: "VrstaId",
                keyValue: 4,
                column: "JeOperacija",
                value: false);

            migrationBuilder.UpdateData(
                table: "VrstaUsluge",
                keyColumn: "VrstaId",
                keyValue: 5,
                column: "JeOperacija",
                value: true);

            migrationBuilder.UpdateData(
                table: "VrstaUsluge",
                keyColumn: "VrstaId",
                keyValue: 6,
                column: "JeOperacija",
                value: false);

            migrationBuilder.CreateIndex(
                name: "UQ_VrstaUsluge_Naziv",
                table: "VrstaUsluge",
                column: "Naziv",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UQ_VrstaLjubimca_Naziv",
                table: "VrstaLjubimca",
                column: "Naziv",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UQ_Uloga_Naziv",
                table: "Uloga",
                column: "Naziv",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UQ_StavkeTermina_Termin_Usluga",
                table: "StavkeTermina",
                columns: new[] { "TerminId", "UslugaId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UQ_Recenzija_TerminId",
                table: "Recenzija",
                column: "TerminId",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UQ_Rasa_Vrsta_Naziv",
                table: "Rasa",
                columns: new[] { "VrstaLjubimcaId", "Naziv" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UQ_Promocija_Kod",
                table: "Promocija",
                column: "Kod",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UQ_NacinPlacanja_Naziv",
                table: "NacinPlacanja",
                column: "Naziv",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UQ_Korisnik_Email",
                table: "Korisnik",
                column: "Email",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UQ_Korisnik_KorisnickoIme",
                table: "Korisnik",
                column: "KorisnickoIme",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UQ_Grad_Naziv",
                table: "Grad",
                column: "Naziv",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "UQ_AktProm_Korisnik_Promocija",
                table: "AktiviranaPromocija",
                columns: new[] { "KorisnikId", "PromocijaId" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_VrstaUsluge_Naziv",
                table: "VrstaUsluge");

            migrationBuilder.DropIndex(
                name: "UQ_VrstaLjubimca_Naziv",
                table: "VrstaLjubimca");

            migrationBuilder.DropIndex(
                name: "UQ_Uloga_Naziv",
                table: "Uloga");

            migrationBuilder.DropIndex(
                name: "UQ_StavkeTermina_Termin_Usluga",
                table: "StavkeTermina");

            migrationBuilder.DropIndex(
                name: "UQ_Recenzija_TerminId",
                table: "Recenzija");

            migrationBuilder.DropIndex(
                name: "UQ_Rasa_Vrsta_Naziv",
                table: "Rasa");

            migrationBuilder.DropIndex(
                name: "UQ_Promocija_Kod",
                table: "Promocija");

            migrationBuilder.DropIndex(
                name: "UQ_NacinPlacanja_Naziv",
                table: "NacinPlacanja");

            migrationBuilder.DropIndex(
                name: "UQ_Korisnik_Email",
                table: "Korisnik");

            migrationBuilder.DropIndex(
                name: "UQ_Korisnik_KorisnickoIme",
                table: "Korisnik");

            migrationBuilder.DropIndex(
                name: "UQ_Grad_Naziv",
                table: "Grad");

            migrationBuilder.DropIndex(
                name: "UQ_AktProm_Korisnik_Promocija",
                table: "AktiviranaPromocija");

            migrationBuilder.DropColumn(
                name: "JeOperacija",
                table: "VrstaUsluge");

            migrationBuilder.CreateIndex(
                name: "UQ_VrstaUsluge_Naziv",
                table: "VrstaUsluge",
                column: "Naziv",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_VrstaLjubimca_Naziv",
                table: "VrstaLjubimca",
                column: "Naziv",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Uloga_Naziv",
                table: "Uloga",
                column: "Naziv",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_StavkeTermina_Termin_Usluga",
                table: "StavkeTermina",
                columns: new[] { "TerminId", "UslugaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Recenzija_TerminId",
                table: "Recenzija",
                column: "TerminId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Rasa_Vrsta_Naziv",
                table: "Rasa",
                columns: new[] { "VrstaLjubimcaId", "Naziv" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Promocija_Kod",
                table: "Promocija",
                column: "Kod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_NacinPlacanja_Naziv",
                table: "NacinPlacanja",
                column: "Naziv",
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
                name: "UQ_Grad_Naziv",
                table: "Grad",
                column: "Naziv",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_AktProm_Korisnik_Promocija",
                table: "AktiviranaPromocija",
                columns: new[] { "KorisnikId", "PromocijaId" },
                unique: true);
        }
    }
}
