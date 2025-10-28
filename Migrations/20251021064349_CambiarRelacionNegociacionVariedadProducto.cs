using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class CambiarRelacionNegociacionVariedadProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_Producto",
                table: "Negociacion");

            migrationBuilder.RenameColumn(
                name: "IdProducto",
                table: "Negociacion",
                newName: "IdVariedadProducto");

            migrationBuilder.RenameIndex(
                name: "IX_Negociacion_IdProducto",
                table: "Negociacion",
                newName: "IX_Negociacion_IdVariedadProducto");

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_VariedadProducto",
                table: "Negociacion",
                column: "IdVariedadProducto",
                principalTable: "VariedadProducto",
                principalColumn: "IdVariedadProducto",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_VariedadProducto",
                table: "Negociacion");

            migrationBuilder.RenameColumn(
                name: "IdVariedadProducto",
                table: "Negociacion",
                newName: "IdProducto");

            migrationBuilder.RenameIndex(
                name: "IX_Negociacion_IdVariedadProducto",
                table: "Negociacion",
                newName: "IX_Negociacion_IdProducto");

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_Producto",
                table: "Negociacion",
                column: "IdProducto",
                principalTable: "Producto",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
