using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemovePersonaIdUbicacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ubicacion_Persona_PersonaIdPersona",
                table: "Ubicacion");

            migrationBuilder.DropIndex(
                name: "IX_Ubicacion_PersonaIdPersona",
                table: "Ubicacion");

            migrationBuilder.DropColumn(
                name: "PersonaIdPersona",
                table: "Ubicacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PersonaIdPersona",
                table: "Ubicacion",
                type: "int",
                nullable: true);

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
        }
    }
}
