using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposAnulacionNegociacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FAnulacion",
                table: "Negociacion",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuarioAnulacion",
                table: "Negociacion",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoAnulacion",
                table: "Negociacion",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_IdUsuarioAnulacion",
                table: "Negociacion",
                column: "IdUsuarioAnulacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_UsuarioAnulacion",
                table: "Negociacion",
                column: "IdUsuarioAnulacion",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_UsuarioAnulacion",
                table: "Negociacion");

            migrationBuilder.DropIndex(
                name: "IX_Negociacion_IdUsuarioAnulacion",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "FAnulacion",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "IdUsuarioAnulacion",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "MotivoAnulacion",
                table: "Negociacion");
        }
    }
}
