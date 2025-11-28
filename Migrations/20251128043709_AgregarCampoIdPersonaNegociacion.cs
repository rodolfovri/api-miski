using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCampoIdPersonaNegociacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_Persona_ProveedorIdPersona",
                table: "Negociacion");

            migrationBuilder.AddColumn<int>(
                name: "IdPersonaProveedor",
                table: "Negociacion",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_IdPersonaProveedor",
                table: "Negociacion",
                column: "IdPersonaProveedor");

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_PersonaProveedor",
                table: "Negociacion",
                column: "IdPersonaProveedor",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_PersonaProveedor",
                table: "Negociacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_Persona_ProveedorIdPersona",
                table: "Negociacion");

            migrationBuilder.DropIndex(
                name: "IX_Negociacion_IdPersonaProveedor",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "IdPersonaProveedor",
                table: "Negociacion");

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_Persona_ProveedorIdPersona",
                table: "Negociacion",
                column: "ProveedorIdPersona",
                principalTable: "Persona",
                principalColumn: "IdPersona",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
