using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class CambiarRelacionUbicacionPersona : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ubicacion_Persona",
                table: "Ubicacion");

            migrationBuilder.DropIndex(
                name: "IX_Ubicacion_IdPersona",
                table: "Ubicacion");

            migrationBuilder.RenameColumn(
                name: "IdPersona",
                table: "Ubicacion",
                newName: "IdUsuario");

            migrationBuilder.AddColumn<int>(
                name: "PersonaIdPersona",
                table: "Ubicacion",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ubicacion_IdUsuario",
                table: "Ubicacion",
                column: "IdUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ubicacion_PersonaIdPersona",
                table: "Ubicacion",
                column: "PersonaIdPersona");

            migrationBuilder.AddForeignKey(
                name: "FK_Ubicacion_Persona_PersonaIdPersona",
                table: "Ubicacion",
                column: "PersonaIdPersona",
                principalTable: "Persona",
                principalColumn: "IdPersona",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ubicacion_Usuario",
                table: "Ubicacion",
                column: "IdUsuario",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ubicacion_Persona_PersonaIdPersona",
                table: "Ubicacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Ubicacion_Usuario",
                table: "Ubicacion");

            migrationBuilder.DropIndex(
                name: "IX_Ubicacion_IdUsuario",
                table: "Ubicacion");

            migrationBuilder.DropIndex(
                name: "IX_Ubicacion_PersonaIdPersona",
                table: "Ubicacion");

            migrationBuilder.DropColumn(
                name: "PersonaIdPersona",
                table: "Ubicacion");

            migrationBuilder.RenameColumn(
                name: "IdUsuario",
                table: "Ubicacion",
                newName: "IdPersona");

            migrationBuilder.CreateIndex(
                name: "IX_Ubicacion_IdPersona",
                table: "Ubicacion",
                column: "IdPersona");

            migrationBuilder.AddForeignKey(
                name: "FK_Ubicacion_Persona",
                table: "Ubicacion",
                column: "IdPersona",
                principalTable: "Persona",
                principalColumn: "IdPersona",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
