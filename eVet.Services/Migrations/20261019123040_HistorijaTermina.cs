using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eVet.Services.Migrations
{
    /// <inheritdoc />
    public partial class HistorijaTermina : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Termin",
                columns: new[] { "TerminId", "AdresaPrevoza", "AktiviranaPromocijaId", "CijenaPrevoza", "DatumKreiranja", "DatumPromjeneStatusa", "DatumTermina", "GradPrevozaId", "IsDeleted", "IznosPopusta", "JePrevoz", "KorisnikId", "LjubimacId", "NacinPlacanjaId", "RazlogPromjeneStatusa", "Sifra", "Status", "StatusPromijenioId", "UkupnaCijena", "UkupnoTrajanje", "VeterinarId", "VrijemeBrisanja", "VrijemeKraja", "VrijemePocetka" },
                values: new object[,]
                {
                    { 1001, null, null, 0m, new DateTime(2026, 1, 29, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 3, 9, 45, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 2, 3), null, false, 0m, false, 5, 1, 1, null, "HIST0001", 3, 2, 75m, 45, 2, null, new TimeOnly(9, 45, 0), new TimeOnly(9, 0, 0) },
                    { 1002, null, null, 0m, new DateTime(2026, 2, 5, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 10, 10, 45, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 2, 10), null, false, 0m, false, 6, 3, 1, null, "HIST0002", 3, 3, 90m, 45, 3, null, new TimeOnly(10, 45, 0), new TimeOnly(10, 0, 0) },
                    { 1003, null, null, 0m, new DateTime(2026, 2, 16, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 21, 12, 55, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 2, 21), null, false, 0m, false, 5, 1, 1, null, "HIST0003", 3, 4, 90m, 115, 4, null, new TimeOnly(12, 55, 0), new TimeOnly(11, 0, 0) },
                    { 1004, null, null, 0m, new DateTime(2026, 2, 27, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 4, 11, 15, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 3, 4), null, false, 0m, false, 6, 3, 1, null, "HIST0004", 3, 4, 100m, 105, 4, null, new TimeOnly(11, 15, 0), new TimeOnly(9, 30, 0) },
                    { 1005, null, null, 0m, new DateTime(2026, 3, 7, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 12, 13, 45, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 3, 12), null, false, 0m, false, 5, 2, 1, null, "HIST0005", 3, 2, 80m, 45, 2, null, new TimeOnly(13, 45, 0), new TimeOnly(13, 0, 0) },
                    { 1006, null, null, 0m, new DateTime(2026, 3, 20, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 25, 12, 30, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 3, 25), null, false, 0m, false, 6, 3, 1, null, "HIST0006", 3, 4, 130m, 150, 4, null, new TimeOnly(12, 30, 0), new TimeOnly(10, 0, 0) },
                    { 1007, null, null, 0m, new DateTime(2026, 4, 2, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 7, 10, 25, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 4, 7), null, false, 0m, false, 5, 1, 1, null, "HIST0007", 3, 3, 120m, 85, 3, null, new TimeOnly(10, 25, 0), new TimeOnly(9, 0, 0) },
                    { 1008, null, null, 0m, new DateTime(2026, 4, 10, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 15, 12, 45, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 4, 15), null, false, 0m, false, 6, 4, 1, null, "HIST0008", 3, 2, 80m, 45, 2, null, new TimeOnly(12, 45, 0), new TimeOnly(12, 0, 0) },
                    { 1009, null, null, 0m, new DateTime(2026, 4, 23, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 28, 10, 15, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 4, 28), null, false, 0m, false, 5, 2, 1, null, "HIST0009", 3, 3, 240m, 105, 3, null, new TimeOnly(10, 15, 0), new TimeOnly(8, 30, 0) },
                    { 1010, null, null, 0m, new DateTime(2026, 5, 4, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 9, 12, 10, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 5, 9), null, false, 0m, false, 6, 3, 1, null, "HIST0010", 3, 4, 60m, 70, 4, null, new TimeOnly(12, 10, 0), new TimeOnly(11, 0, 0) },
                    { 1011, null, null, 0m, new DateTime(2026, 5, 15, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 5, 20, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 5, 20), null, false, 0m, false, 5, 1, 1, null, "HIST0011", 3, 2, 125m, 60, 2, null, new TimeOnly(10, 0, 0), new TimeOnly(9, 0, 0) },
                    { 1012, null, null, 0m, new DateTime(2026, 5, 28, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 2, 15, 25, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 6, 2), null, false, 0m, false, 6, 3, 1, null, "HIST0012", 3, 3, 120m, 85, 3, null, new TimeOnly(15, 25, 0), new TimeOnly(14, 0, 0) },
                    { 1013, null, null, 0m, new DateTime(2026, 6, 13, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 18, 11, 55, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 6, 18), null, false, 0m, false, 5, 1, 1, null, "HIST0013", 3, 4, 90m, 115, 4, null, new TimeOnly(11, 55, 0), new TimeOnly(10, 0, 0) },
                    { 1014, null, null, 0m, new DateTime(2026, 7, 1, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 6, 10, 15, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 7, 6), null, false, 0m, false, 6, 3, 1, null, "HIST0014", 3, 2, 190m, 75, 2, null, new TimeOnly(10, 15, 0), new TimeOnly(9, 0, 0) },
                    { 1015, null, null, 0m, new DateTime(2026, 7, 17, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 22, 13, 25, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 7, 22), null, false, 0m, false, 5, 2, 1, null, "HIST0015", 3, 4, 40m, 55, 4, null, new TimeOnly(13, 25, 0), new TimeOnly(12, 30, 0) },
                    { 1016, null, null, 0m, new DateTime(2026, 8, 6, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 11, 11, 45, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 8, 11), null, false, 0m, false, 6, 3, 1, null, "HIST0016", 3, 4, 100m, 105, 4, null, new TimeOnly(11, 45, 0), new TimeOnly(10, 0, 0) },
                    { 1017, null, null, 0m, new DateTime(2026, 8, 27, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 1, 9, 35, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 9, 1), null, false, 0m, false, 5, 1, 1, null, "HIST0017", 3, 2, 65m, 35, 2, null, new TimeOnly(9, 35, 0), new TimeOnly(9, 0, 0) },
                    { 1018, null, null, 0m, new DateTime(2026, 9, 10, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 9, 15, 11, 50, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 9, 15), null, false, 0m, false, 6, 4, 1, null, "HIST0018", 3, 3, 65m, 50, 3, null, new TimeOnly(11, 50, 0), new TimeOnly(11, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "StavkeTermina",
                columns: new[] { "StavkaTerminaId", "Cijena", "IsDeleted", "TerminId", "UslugaId", "VrijemeBrisanja" },
                values: new object[,]
                {
                    { 1001, 40m, false, 1001, 1, null },
                    { 1002, 35m, false, 1001, 3, null },
                    { 1003, 40m, false, 1002, 1, null },
                    { 1004, 50m, false, 1002, 4, null },
                    { 1005, 50m, false, 1003, 5, null },
                    { 1006, 30m, false, 1003, 6, null },
                    { 1007, 10m, false, 1003, 7, null },
                    { 1008, 50m, false, 1004, 5, null },
                    { 1009, 50m, false, 1004, 8, null },
                    { 1010, 40m, false, 1005, 1, null },
                    { 1011, 40m, false, 1005, 12, null },
                    { 1012, 50m, false, 1006, 5, null },
                    { 1013, 30m, false, 1006, 6, null },
                    { 1014, 50m, false, 1006, 8, null },
                    { 1015, 70m, false, 1007, 9, null },
                    { 1016, 50m, false, 1007, 8, null },
                    { 1017, 40m, false, 1008, 1, null },
                    { 1018, 40m, false, 1008, 12, null },
                    { 1019, 40m, false, 1009, 12, null },
                    { 1020, 200m, false, 1009, 10, null },
                    { 1021, 50m, false, 1010, 5, null },
                    { 1022, 10m, false, 1010, 7, null },
                    { 1023, 40m, false, 1011, 1, null },
                    { 1024, 50m, false, 1011, 4, null },
                    { 1025, 35m, false, 1011, 3, null },
                    { 1026, 70m, false, 1012, 9, null },
                    { 1027, 50m, false, 1012, 8, null },
                    { 1028, 50m, false, 1013, 5, null },
                    { 1029, 30m, false, 1013, 6, null },
                    { 1030, 10m, false, 1013, 7, null },
                    { 1031, 40m, false, 1014, 12, null },
                    { 1032, 150m, false, 1014, 11, null },
                    { 1033, 30m, false, 1015, 6, null },
                    { 1034, 10m, false, 1015, 7, null },
                    { 1035, 50m, false, 1016, 5, null },
                    { 1036, 50m, false, 1016, 8, null },
                    { 1037, 25m, false, 1017, 2, null },
                    { 1038, 40m, false, 1017, 12, null },
                    { 1039, 40m, false, 1018, 1, null },
                    { 1040, 25m, false, 1018, 2, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1005);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1006);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1007);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1008);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1009);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1010);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1011);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1012);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1013);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1014);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1015);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1016);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1017);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1018);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1019);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1020);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1021);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1022);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1023);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1024);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1025);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1026);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1027);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1028);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1029);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1030);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1031);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1032);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1033);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1034);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1035);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1036);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1037);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1038);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1039);

            migrationBuilder.DeleteData(
                table: "StavkeTermina",
                keyColumn: "StavkaTerminaId",
                keyValue: 1040);

            migrationBuilder.DeleteData(
                table: "Termin",
                keyColumn: "TerminId",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "Termin",
                keyColumn: "TerminId",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "Termin",
                keyColumn: "TerminId",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "Termin",
                keyColumn: "TerminId",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "Termin",
                keyColumn: "TerminId",
                keyValue: 1005);

            migrationBuilder.DeleteData(
                table: "Termin",
                keyColumn: "TerminId",
                keyValue: 1006);

            migrationBuilder.DeleteData(
                table: "Termin",
                keyColumn: "TerminId",
                keyValue: 1007);

            migrationBuilder.DeleteData(
                table: "Termin",
                keyColumn: "TerminId",
                keyValue: 1008);

            migrationBuilder.DeleteData(
                table: "Termin",
                keyColumn: "TerminId",
                keyValue: 1009);

            migrationBuilder.DeleteData(
                table: "Termin",
                keyColumn: "TerminId",
                keyValue: 1010);

            migrationBuilder.DeleteData(
                table: "Termin",
                keyColumn: "TerminId",
                keyValue: 1011);

            migrationBuilder.DeleteData(
                table: "Termin",
                keyColumn: "TerminId",
                keyValue: 1012);

            migrationBuilder.DeleteData(
                table: "Termin",
                keyColumn: "TerminId",
                keyValue: 1013);

            migrationBuilder.DeleteData(
                table: "Termin",
                keyColumn: "TerminId",
                keyValue: 1014);

            migrationBuilder.DeleteData(
                table: "Termin",
                keyColumn: "TerminId",
                keyValue: 1015);

            migrationBuilder.DeleteData(
                table: "Termin",
                keyColumn: "TerminId",
                keyValue: 1016);

            migrationBuilder.DeleteData(
                table: "Termin",
                keyColumn: "TerminId",
                keyValue: 1017);

            migrationBuilder.DeleteData(
                table: "Termin",
                keyColumn: "TerminId",
                keyValue: 1018);
        }
    }
}
