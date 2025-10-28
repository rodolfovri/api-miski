using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class CambiarRelacionPersonaPorUsuarioNegociacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_AprobadaPor",
                table: "Negociacion");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioIdUsuario",
                table: "Negociacion",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioIdUsuario",
                table: "LlegadaPlanta",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_UsuarioIdUsuario",
                table: "Negociacion",
                column: "UsuarioIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_LlegadaPlanta_UsuarioIdUsuario",
                table: "LlegadaPlanta",
                column: "UsuarioIdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_LlegadaPlanta_Usuario_UsuarioIdUsuario",
                table: "LlegadaPlanta",
                column: "UsuarioIdUsuario",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_AprobadaPor",
                table: "Negociacion",
                column: "AprobadaPor",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_Usuario_UsuarioIdUsuario",
                table: "Negociacion",
                column: "UsuarioIdUsuario",
                principalTable: "Usuario",
                principalColumn: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LlegadaPlanta_Usuario_UsuarioIdUsuario",
                table: "LlegadaPlanta");

            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_AprobadaPor",
                table: "Negociacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_Usuario_UsuarioIdUsuario",
                table: "Negociacion");

            migrationBuilder.DropIndex(
                name: "IX_Negociacion_UsuarioIdUsuario",
                table: "Negociacion");

            migrationBuilder.DropIndex(
                name: "IX_LlegadaPlanta_UsuarioIdUsuario",
                table: "LlegadaPlanta");

            migrationBuilder.DropColumn(
                name: "UsuarioIdUsuario",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "UsuarioIdUsuario",
                table: "LlegadaPlanta");

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_AprobadaPor",
                table: "Negociacion",
                column: "AprobadaPor",
                principalTable: "Persona",
                principalColumn: "IdPersona",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
