using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class CambiarRelacionStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stock_Producto",
                table: "Stock");

            migrationBuilder.RenameColumn(
                name: "IdProducto",
                table: "Stock",
                newName: "IdVariedadProducto");

            migrationBuilder.RenameIndex(
                name: "IX_Stock_IdProducto",
                table: "Stock",
                newName: "IX_Stock_IdVariedadProducto");

            migrationBuilder.AddForeignKey(
                name: "FK_Stock_VariedadProducto",
                table: "Stock",
                column: "IdVariedadProducto",
                principalTable: "VariedadProducto",
                principalColumn: "IdVariedadProducto",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stock_VariedadProducto",
                table: "Stock");

            migrationBuilder.RenameColumn(
                name: "IdVariedadProducto",
                table: "Stock",
                newName: "IdProducto");

            migrationBuilder.RenameIndex(
                name: "IX_Stock_IdVariedadProducto",
                table: "Stock",
                newName: "IX_Stock_IdProducto");

            migrationBuilder.AddForeignKey(
                name: "FK_Stock_Producto",
                table: "Stock",
                column: "IdProducto",
                principalTable: "Producto",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
