using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBancoTipoDocumentoEnNegociacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdBanco",
                table: "Negociacion",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdTipoDocumento",
                table: "Negociacion",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_IdBanco",
                table: "Negociacion",
                column: "IdBanco");

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_IdTipoDocumento",
                table: "Negociacion",
                column: "IdTipoDocumento");

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_Banco",
                table: "Negociacion",
                column: "IdBanco",
                principalTable: "Banco",
                principalColumn: "IdBanco",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_TipoDocumento",
                table: "Negociacion",
                column: "IdTipoDocumento",
                principalTable: "TipoDocumento",
                principalColumn: "IdTipoDocumento",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_Banco",
                table: "Negociacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_TipoDocumento",
                table: "Negociacion");

            migrationBuilder.DropIndex(
                name: "IX_Negociacion_IdBanco",
                table: "Negociacion");

            migrationBuilder.DropIndex(
                name: "IX_Negociacion_IdTipoDocumento",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "IdBanco",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "IdTipoDocumento",
                table: "Negociacion");
        }
    }
}
