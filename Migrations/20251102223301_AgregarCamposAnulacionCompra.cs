using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposAnulacionCompra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Correlativo",
                table: "Compra",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FAnulacion",
                table: "Compra",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuarioAnulacion",
                table: "Compra",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoAnulacion",
                table: "Compra",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Compra_IdUsuarioAnulacion",
                table: "Compra",
                column: "IdUsuarioAnulacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Compra_UsuarioAnulacion",
                table: "Compra",
                column: "IdUsuarioAnulacion",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compra_UsuarioAnulacion",
                table: "Compra");

            migrationBuilder.DropIndex(
                name: "IX_Compra_IdUsuarioAnulacion",
                table: "Compra");

            migrationBuilder.DropColumn(
                name: "Correlativo",
                table: "Compra");

            migrationBuilder.DropColumn(
                name: "FAnulacion",
                table: "Compra");

            migrationBuilder.DropColumn(
                name: "IdUsuarioAnulacion",
                table: "Compra");

            migrationBuilder.DropColumn(
                name: "MotivoAnulacion",
                table: "Compra");
        }
    }
}
