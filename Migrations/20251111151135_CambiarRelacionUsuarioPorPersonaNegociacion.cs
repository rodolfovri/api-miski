using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class CambiarRelacionUsuarioPorPersonaNegociacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_Comisionista",
                table: "Negociacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_Persona_ProveedorIdPersona",
                table: "Negociacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_Comisionista",
                table: "Negociacion",
                column: "IdComisionista",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_Persona_ProveedorIdPersona",
                table: "Negociacion",
                column: "ProveedorIdPersona",
                principalTable: "Persona",
                principalColumn: "IdPersona",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_Comisionista",
                table: "Negociacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_Persona_ProveedorIdPersona",
                table: "Negociacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_Comisionista",
                table: "Negociacion",
                column: "IdComisionista",
                principalTable: "Persona",
                principalColumn: "IdPersona",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_Persona_ProveedorIdPersona",
                table: "Negociacion",
                column: "ProveedorIdPersona",
                principalTable: "Persona",
                principalColumn: "IdPersona");
        }
    }
}
