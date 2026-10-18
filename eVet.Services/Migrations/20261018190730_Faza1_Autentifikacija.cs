using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eVet.Services.Migrations
{
    /// <inheritdoc />
    public partial class Faza1_Autentifikacija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "KorisniciUloge");

            migrationBuilder.DropColumn(
                name: "VrijemeBrisanja",
                table: "KorisniciUloge");

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "Korisnik",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Korisnik",
                columns: new[] { "KorisnikId", "DatumRegistracije", "Email", "Ime", "IsDeleted", "JeAktivan", "KorisnickoIme", "LozinkaHash", "LozinkaSalt", "Prezime", "SecurityStamp", "Slika", "Telefon", "VrijemeBrisanja" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "desktop@example.com", "Emina", false, true, "desktop", "bkaa1iPvKhORMC4PjajiWan1PrVF1y5HPwM11x66x0I=", "TFnQ0BSZcBD91PuWE3IxNA==", "Hadžić", "4fd76901-8c6e-4b42-91aa-e456ba00d4a9", null, "+387 61 111 111", null },
                    { 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "veterinar@example.com", "Nikola", false, true, "veterinar", "dmi9esv9G+zdqBU0ovmr2nQ+qDusgxJzeUKezGw5oNg=", "AKvNyrkpSh8okoyRonbPZA==", "Nikolić", "4f0e6b9f-14b8-4f10-8f20-53328d6c79bd", null, "+387 61 222 222", null },
                    { 3, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "veterinar2@example.com", "Amar", false, true, "veterinar2", "e0m5KX0r5HcG2uDoLWSYKFn9Ue9a1jeTq0pxHghsj78=", "pIqi4lQgiAvlO9C4E0bm0Q==", "Kovačević", "cbadf0af-9f32-40ee-8e9f-bd7fee1db8a2", null, "+387 61 333 333", null },
                    { 4, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "veterinar3@example.com", "Selma", false, true, "veterinar3", "1MAVfQaWwMqk37qODmNmdQVpng2fW0IdkEsKcnjdmuE=", "pUUFtIh/wrZB+xcnPKHYvQ==", "Begić", "e8261264-9b52-40af-b1eb-5003406a1f0e", null, "+387 61 444 444", null },
                    { 5, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "mobile@example.com", "Haris", false, true, "mobile", "NKFpC8Cv9fFFrVBPqBX1PNjjibalVo6HcZrSIuEUr2E=", "PU6Onf9pS/NxpBCLyTXiIw==", "Delić", "6b612f4c-dbed-44d6-9b1d-e7f8b1c05cd4", null, "+387 62 555 555", null },
                    { 6, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), "klijent2@example.com", "Lejla", false, true, "klijent2", "Uh4fclz66uNSZfK9ulrnSV2Fv8EkkbgWp8Sn0nY/yF4=", "EFFCugrGfdoGlgQgVyA7vA==", "Mehić", "6af64aed-222f-4988-bb39-79d17e4552a4", null, "+387 62 666 666", null }
                });

            migrationBuilder.InsertData(
                table: "Usluga",
                columns: new[] { "UslugaId", "Cijena", "DatumObjavljivanja", "IsDeleted", "Naziv", "Opis", "Slika", "Trajanje", "VrijemeBrisanja", "VrstaId" },
                values: new object[,]
                {
                    { 1, 40m, new DateTime(2026, 1, 10, 8, 0, 0, 0, DateTimeKind.Utc), false, "Opći pregled", "Kompletan klinički pregled ljubimca uz savjetovanje vlasnika.", null, 30, null, 1 },
                    { 2, 25m, new DateTime(2026, 1, 10, 8, 0, 0, 0, DateTimeKind.Utc), false, "Kontrolni pregled", "Kontrola nakon terapije ili intervencije.", null, 20, null, 1 },
                    { 3, 35m, new DateTime(2026, 1, 10, 8, 0, 0, 0, DateTimeKind.Utc), false, "Vakcinacija protiv bjesnila", "Obavezna godišnja vakcinacija uz upis u pasoš ljubimca.", null, 15, null, 2 },
                    { 4, 50m, new DateTime(2026, 1, 10, 8, 0, 0, 0, DateTimeKind.Utc), false, "Kombinovana vakcina", "Zaštita od štenećaka, parvoviroze, hepatitisa i leptospiroze.", null, 15, null, 2 },
                    { 5, 45m, new DateTime(2026, 1, 10, 8, 0, 0, 0, DateTimeKind.Utc), false, "Šišanje psa", "Šišanje i oblikovanje dlake prema pasmini.", null, 60, null, 3 },
                    { 6, 30m, new DateTime(2026, 1, 10, 8, 0, 0, 0, DateTimeKind.Utc), false, "Kupanje i njega krzna", "Kupanje, sušenje i raščešljavanje krzna.", null, 45, null, 3 },
                    { 7, 10m, new DateTime(2026, 1, 10, 8, 0, 0, 0, DateTimeKind.Utc), false, "Rezanje noktiju", "Sigurno skraćivanje noktiju.", null, 10, null, 3 },
                    { 8, 80m, new DateTime(2026, 1, 10, 8, 0, 0, 0, DateTimeKind.Utc), false, "Izbjeljivanje zuba", "Profesionalno izbjeljivanje zuba ljubimca.", null, 45, null, 4 },
                    { 9, 70m, new DateTime(2026, 1, 10, 8, 0, 0, 0, DateTimeKind.Utc), false, "Čišćenje kamenca", "Ultrazvučno uklanjanje zubnog kamenca.", null, 40, null, 4 },
                    { 10, 200m, new DateTime(2026, 1, 10, 8, 0, 0, 0, DateTimeKind.Utc), false, "Sterilizacija", "Hirurški zahvat uz anesteziju i postoperativnu njegu.", null, 90, null, 5 },
                    { 11, 150m, new DateTime(2026, 1, 10, 8, 0, 0, 0, DateTimeKind.Utc), false, "Kastracija", "Hirurški zahvat uz anesteziju i postoperativnu njegu.", null, 60, null, 5 },
                    { 12, 40m, new DateTime(2026, 1, 10, 8, 0, 0, 0, DateTimeKind.Utc), false, "Kompletna krvna slika", "Laboratorijska analiza krvi s nalazom isti dan.", null, 15, null, 6 }
                });

            migrationBuilder.InsertData(
                table: "KorisniciUloge",
                columns: new[] { "KorisnikUlogaId", "DatumDodavanja", "KorisnikId", "UlogaId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), 1, 1 },
                    { 2, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), 2, 2 },
                    { 3, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), 3, 2 },
                    { 4, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), 4, 2 },
                    { 5, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), 5, 3 },
                    { 6, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), 6, 3 }
                });

            migrationBuilder.InsertData(
                table: "Ljubimac",
                columns: new[] { "LjubimacId", "DatumRodjenja", "Ime", "IsDeleted", "KorisnikId", "Napomena", "RasaId", "Slika", "Spol", "Tezina", "VrijemeBrisanja" },
                values: new object[,]
                {
                    { 1, new DateOnly(2021, 4, 12), "Rex", false, 5, null, 2, null, 1, 32.5m, null },
                    { 2, new DateOnly(2022, 8, 3), "Mici", false, 5, null, 9, null, 2, 4.2m, null },
                    { 3, new DateOnly(2020, 11, 20), "Luna", false, 6, null, 3, null, 2, 27.8m, null },
                    { 4, new DateOnly(2023, 2, 14), "Kiki", false, 6, null, 12, null, 1, 0.1m, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "KorisniciUloge",
                keyColumn: "KorisnikUlogaId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "KorisniciUloge",
                keyColumn: "KorisnikUlogaId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "KorisniciUloge",
                keyColumn: "KorisnikUlogaId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "KorisniciUloge",
                keyColumn: "KorisnikUlogaId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "KorisniciUloge",
                keyColumn: "KorisnikUlogaId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "KorisniciUloge",
                keyColumn: "KorisnikUlogaId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Ljubimac",
                keyColumn: "LjubimacId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ljubimac",
                keyColumn: "LjubimacId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Ljubimac",
                keyColumn: "LjubimacId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Ljubimac",
                keyColumn: "LjubimacId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Usluga",
                keyColumn: "UslugaId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Usluga",
                keyColumn: "UslugaId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Usluga",
                keyColumn: "UslugaId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Usluga",
                keyColumn: "UslugaId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Usluga",
                keyColumn: "UslugaId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Usluga",
                keyColumn: "UslugaId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Usluga",
                keyColumn: "UslugaId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Usluga",
                keyColumn: "UslugaId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Usluga",
                keyColumn: "UslugaId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Usluga",
                keyColumn: "UslugaId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Usluga",
                keyColumn: "UslugaId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Usluga",
                keyColumn: "UslugaId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Korisnik",
                keyColumn: "KorisnikId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Korisnik",
                keyColumn: "KorisnikId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Korisnik",
                keyColumn: "KorisnikId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Korisnik",
                keyColumn: "KorisnikId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Korisnik",
                keyColumn: "KorisnikId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Korisnik",
                keyColumn: "KorisnikId",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "Korisnik");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "KorisniciUloge",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "VrijemeBrisanja",
                table: "KorisniciUloge",
                type: "datetime",
                nullable: true);
        }
    }
}
