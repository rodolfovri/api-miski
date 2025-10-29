using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class QuitarCampoProductoEnLlegadaPlantaDetalle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LlegadaDetalle_Producto",
                table: "LlegadaPlantaDetalle");

            migrationBuilder.DropIndex(
                name: "IX_LlegadaPlantaDetalle_IdProductoFinal",
                table: "LlegadaPlantaDetalle");

            migrationBuilder.DropColumn(
                name: "IdProductoFinal",
                table: "LlegadaPlantaDetalle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdProductoFinal",
                table: "LlegadaPlantaDetalle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LlegadaPlantaDetalle_IdProductoFinal",
                table: "LlegadaPlantaDetalle",
                column: "IdProductoFinal");

            migrationBuilder.AddForeignKey(
                name: "FK_LlegadaDetalle_Producto",
                table: "LlegadaPlantaDetalle",
                column: "IdProductoFinal",
                principalTable: "Producto",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
