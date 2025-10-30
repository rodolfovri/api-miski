using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposCompraVehiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "CompraVehiculo",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdPersona",
                table: "CompraVehiculo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CompraVehiculo_IdPersona",
                table: "CompraVehiculo",
                column: "IdPersona");

            migrationBuilder.AddForeignKey(
                name: "FK_CompraVehiculo_Persona",
                table: "CompraVehiculo",
                column: "IdPersona",
                principalTable: "Persona",
                principalColumn: "IdPersona",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompraVehiculo_Persona",
                table: "CompraVehiculo");

            migrationBuilder.DropIndex(
                name: "IX_CompraVehiculo_IdPersona",
                table: "CompraVehiculo");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "CompraVehiculo");

            migrationBuilder.DropColumn(
                name: "IdPersona",
                table: "CompraVehiculo");
        }
    }
}
