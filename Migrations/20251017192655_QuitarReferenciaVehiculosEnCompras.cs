using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class QuitarReferenciaVehiculosEnCompras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compra_Vehiculo_VehiculoIdVehiculo",
                table: "Compra");

            migrationBuilder.DropIndex(
                name: "IX_Compra_VehiculoIdVehiculo",
                table: "Compra");

            migrationBuilder.DropColumn(
                name: "VehiculoIdVehiculo",
                table: "Compra");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VehiculoIdVehiculo",
                table: "Compra",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Compra_VehiculoIdVehiculo",
                table: "Compra",
                column: "VehiculoIdVehiculo");

            migrationBuilder.AddForeignKey(
                name: "FK_Compra_Vehiculo_VehiculoIdVehiculo",
                table: "Compra",
                column: "VehiculoIdVehiculo",
                principalTable: "Vehiculo",
                principalColumn: "IdVehiculo",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
