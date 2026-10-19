using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eVet.Services.Migrations
{
    /// <inheritdoc />
    public partial class ResetLozinke : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResetLozinke",
                columns: table => new
                {
                    ResetLozinkeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    KodHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    KodSalt = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DatumKreiranja = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())")
                        .Annotation("Relational:DefaultConstraintName", "DF_ResetLozinke_DatumKreiranja"),
                    DatumIsteka = table.Column<DateTime>(type: "datetime", nullable: false),
                    BrojPokusaja = table.Column<int>(type: "int", nullable: false),
                    JeIskoristen = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetLozinke", x => x.ResetLozinkeId);
                    table.ForeignKey(
                        name: "FK_ResetLozinke_Korisnik",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResetLozinke_Korisnik",
                table: "ResetLozinke",
                columns: new[] { "KorisnikId", "JeIskoristen" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResetLozinke");
        }
    }
}
