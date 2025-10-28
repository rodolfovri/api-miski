using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class QuitarCampoLicenciaTablaVehiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Licencia",
                table: "Vehiculo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Licencia",
                table: "Vehiculo",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
