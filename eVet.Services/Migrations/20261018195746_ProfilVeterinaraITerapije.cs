using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eVet.Services.Migrations
{
    /// <inheritdoc />
    public partial class ProfilVeterinaraITerapije : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "JeVakcinacija",
                table: "VrstaUsluge",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TrajanjeDana",
                table: "Terapija",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DatumRodjenja",
                table: "Korisnik",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DatumZaposlenja",
                table: "Korisnik",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GradRodjenjaId",
                table: "Korisnik",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecijalnostId",
                table: "Korisnik",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zvanje",
                table: "Korisnik",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Specijalnost",
                columns: table => new
                {
                    SpecijalnostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeBrisanja = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specijalnost", x => x.SpecijalnostId);
                });

            migrationBuilder.UpdateData(
                table: "Korisnik",
                keyColumn: "KorisnikId",
                keyValue: 1,
                columns: new[] { "DatumRodjenja", "DatumZaposlenja", "GradRodjenjaId", "SpecijalnostId", "Zvanje" },
                values: new object[] { null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Korisnik",
                keyColumn: "KorisnikId",
                keyValue: 2,
                columns: new[] { "DatumRodjenja", "DatumZaposlenja", "GradRodjenjaId", "SpecijalnostId", "Zvanje" },
                values: new object[] { new DateOnly(1990, 12, 6), new DateOnly(2018, 4, 1), 1, 2, "Doktor veterinarske medicine" });

            migrationBuilder.UpdateData(
                table: "Korisnik",
                keyColumn: "KorisnikId",
                keyValue: 3,
                columns: new[] { "DatumRodjenja", "DatumZaposlenja", "GradRodjenjaId", "SpecijalnostId", "Zvanje" },
                values: new object[] { new DateOnly(1994, 3, 18), new DateOnly(2021, 9, 1), 2, 1, "Doktor veterinarske medicine" });

            migrationBuilder.UpdateData(
                table: "Korisnik",
                keyColumn: "KorisnikId",
                keyValue: 4,
                columns: new[] { "DatumRodjenja", "DatumZaposlenja", "GradRodjenjaId", "SpecijalnostId", "Zvanje" },
                values: new object[] { new DateOnly(1996, 7, 25), new DateOnly(2023, 2, 15), 1, 4, "Doktorica veterinarske medicine" });

            migrationBuilder.UpdateData(
                table: "Korisnik",
                keyColumn: "KorisnikId",
                keyValue: 5,
                columns: new[] { "DatumRodjenja", "DatumZaposlenja", "GradRodjenjaId", "SpecijalnostId", "Zvanje" },
                values: new object[] { null, null, null, null, null });

            migrationBuilder.UpdateData(
                table: "Korisnik",
                keyColumn: "KorisnikId",
                keyValue: 6,
                columns: new[] { "DatumRodjenja", "DatumZaposlenja", "GradRodjenjaId", "SpecijalnostId", "Zvanje" },
                values: new object[] { null, null, null, null, null });

            migrationBuilder.InsertData(
                table: "Specijalnost",
                columns: new[] { "SpecijalnostId", "IsDeleted", "Naziv", "VrijemeBrisanja" },
                values: new object[,]
                {
                    { 1, false, "Opća praksa", null },
                    { 2, false, "Hirurgija", null },
                    { 3, false, "Dermatologija", null },
                    { 4, false, "Stomatologija", null },
                    { 5, false, "Interna medicina", null }
                });

            migrationBuilder.UpdateData(
                table: "Usluga",
                keyColumn: "UslugaId",
                keyValue: 5,
                column: "Cijena",
                value: 50m);

            migrationBuilder.UpdateData(
                table: "Usluga",
                keyColumn: "UslugaId",
                keyValue: 8,
                columns: new[] { "Cijena", "Opis" },
                values: new object[] { 50m, "Za najljepši osmijeh vašeg ljubimca." });

            migrationBuilder.UpdateData(
                table: "VrstaUsluge",
                keyColumn: "VrstaId",
                keyValue: 1,
                column: "JeVakcinacija",
                value: false);

            migrationBuilder.UpdateData(
                table: "VrstaUsluge",
                keyColumn: "VrstaId",
                keyValue: 2,
                column: "JeVakcinacija",
                value: true);

            migrationBuilder.UpdateData(
                table: "VrstaUsluge",
                keyColumn: "VrstaId",
                keyValue: 3,
                column: "JeVakcinacija",
                value: false);

            migrationBuilder.UpdateData(
                table: "VrstaUsluge",
                keyColumn: "VrstaId",
                keyValue: 4,
                column: "JeVakcinacija",
                value: false);

            migrationBuilder.UpdateData(
                table: "VrstaUsluge",
                keyColumn: "VrstaId",
                keyValue: 5,
                column: "JeVakcinacija",
                value: false);

            migrationBuilder.UpdateData(
                table: "VrstaUsluge",
                keyColumn: "VrstaId",
                keyValue: 6,
                column: "JeVakcinacija",
                value: false);

            migrationBuilder.CreateIndex(
                name: "IX_Korisnik_GradRodjenjaId",
                table: "Korisnik",
                column: "GradRodjenjaId");

            migrationBuilder.CreateIndex(
                name: "IX_Korisnik_SpecijalnostId",
                table: "Korisnik",
                column: "SpecijalnostId");

            migrationBuilder.CreateIndex(
                name: "UQ_Specijalnost_Naziv",
                table: "Specijalnost",
                column: "Naziv",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_Korisnik_GradRodjenja",
                table: "Korisnik",
                column: "GradRodjenjaId",
                principalTable: "Grad",
                principalColumn: "GradId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Korisnik_Specijalnost",
                table: "Korisnik",
                column: "SpecijalnostId",
                principalTable: "Specijalnost",
                principalColumn: "SpecijalnostId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Korisnik_GradRodjenja",
                table: "Korisnik");

            migrationBuilder.DropForeignKey(
                name: "FK_Korisnik_Specijalnost",
                table: "Korisnik");

            migrationBuilder.DropTable(
                name: "Specijalnost");

            migrationBuilder.DropIndex(
                name: "IX_Korisnik_GradRodjenjaId",
                table: "Korisnik");

            migrationBuilder.DropIndex(
                name: "IX_Korisnik_SpecijalnostId",
                table: "Korisnik");

            migrationBuilder.DropColumn(
                name: "JeVakcinacija",
                table: "VrstaUsluge");

            migrationBuilder.DropColumn(
                name: "TrajanjeDana",
                table: "Terapija");

            migrationBuilder.DropColumn(
                name: "DatumRodjenja",
                table: "Korisnik");

            migrationBuilder.DropColumn(
                name: "DatumZaposlenja",
                table: "Korisnik");

            migrationBuilder.DropColumn(
                name: "GradRodjenjaId",
                table: "Korisnik");

            migrationBuilder.DropColumn(
                name: "SpecijalnostId",
                table: "Korisnik");

            migrationBuilder.DropColumn(
                name: "Zvanje",
                table: "Korisnik");

            migrationBuilder.UpdateData(
                table: "Usluga",
                keyColumn: "UslugaId",
                keyValue: 5,
                column: "Cijena",
                value: 45m);

            migrationBuilder.UpdateData(
                table: "Usluga",
                keyColumn: "UslugaId",
                keyValue: 8,
                columns: new[] { "Cijena", "Opis" },
                values: new object[] { 80m, "Profesionalno izbjeljivanje zuba ljubimca." });
        }
    }
}
