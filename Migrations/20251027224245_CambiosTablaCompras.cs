using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class CambiosTablaCompras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdMoneda",
                table: "Compra",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdTipoCambio",
                table: "Compra",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Compra_IdMoneda",
                table: "Compra",
                column: "IdMoneda");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_IdTipoCambio",
                table: "Compra",
                column: "IdTipoCambio");

            migrationBuilder.AddForeignKey(
                name: "FK_Compra_Moneda",
                table: "Compra",
                column: "IdMoneda",
                principalTable: "Moneda",
                principalColumn: "IdMoneda",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Compra_TipoCambio",
                table: "Compra",
                column: "IdTipoCambio",
                principalTable: "TipoCambio",
                principalColumn: "IdTipoCambio",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compra_Moneda",
                table: "Compra");

            migrationBuilder.DropForeignKey(
                name: "FK_Compra_TipoCambio",
                table: "Compra");

            migrationBuilder.DropIndex(
                name: "IX_Compra_IdMoneda",
                table: "Compra");

            migrationBuilder.DropIndex(
                name: "IX_Compra_IdTipoCambio",
                table: "Compra");

            migrationBuilder.DropColumn(
                name: "IdMoneda",
                table: "Compra");

            migrationBuilder.DropColumn(
                name: "IdTipoCambio",
                table: "Compra");
        }
    }
}
