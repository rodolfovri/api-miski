using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class CambiarRelacionLoteCompra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lote_Compra",
                table: "Lote");

            migrationBuilder.DropIndex(
                name: "IX_Lote_IdCompra",
                table: "Lote");

            migrationBuilder.DropColumn(
                name: "IdCompra",
                table: "Lote");

            migrationBuilder.AddColumn<int>(
                name: "IdLote",
                table: "Compra",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Compra_IdLote",
                table: "Compra",
                column: "IdLote",
                unique: true,
                filter: "[IdLote] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Compra_Lote",
                table: "Compra",
                column: "IdLote",
                principalTable: "Lote",
                principalColumn: "IdLote",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compra_Lote",
                table: "Compra");

            migrationBuilder.DropIndex(
                name: "IX_Compra_IdLote",
                table: "Compra");

            migrationBuilder.DropColumn(
                name: "IdLote",
                table: "Compra");

            migrationBuilder.AddColumn<int>(
                name: "IdCompra",
                table: "Lote",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Lote_IdCompra",
                table: "Lote",
                column: "IdCompra");

            migrationBuilder.AddForeignKey(
                name: "FK_Lote_Compra",
                table: "Lote",
                column: "IdCompra",
                principalTable: "Compra",
                principalColumn: "IdCompra",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
