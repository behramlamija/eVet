using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eVet.Services.Migrations
{
    /// <inheritdoc />
    public partial class PayPalPlacanje : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "UQ_Uplata_Termin_Aktivna",
                table: "Uplata",
                column: "TerminId",
                unique: true,
                filter: "[Status] IN (1, 2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_Uplata_Termin_Aktivna",
                table: "Uplata");
        }
    }
}
