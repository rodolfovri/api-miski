using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class CambiarNombresEvidencias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TerceraEvindenciaFoto",
                table: "Negociacion",
                newName: "TerceraEvidenciaFoto");

            migrationBuilder.RenameColumn(
                name: "SegundaEvindenciaFoto",
                table: "Negociacion",
                newName: "SegundaEvidenciaFoto");

            migrationBuilder.RenameColumn(
                name: "PrimeraEvindenciaFoto",
                table: "Negociacion",
                newName: "PrimeraEvidenciaFoto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TerceraEvidenciaFoto",
                table: "Negociacion",
                newName: "TerceraEvindenciaFoto");

            migrationBuilder.RenameColumn(
                name: "SegundaEvidenciaFoto",
                table: "Negociacion",
                newName: "SegundaEvindenciaFoto");

            migrationBuilder.RenameColumn(
                name: "PrimeraEvidenciaFoto",
                table: "Negociacion",
                newName: "PrimeraEvindenciaFoto");
        }
    }
}
