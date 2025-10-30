using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class CambiarRelacionPersonaUsuarioTablaLlegadaPlanta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LlegadaPlanta_Ingeniero",
                table: "LlegadaPlanta");

            migrationBuilder.AddForeignKey(
                name: "FK_LlegadaPlanta_Ingeniero",
                table: "LlegadaPlanta",
                column: "IdUsuario",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LlegadaPlanta_Ingeniero",
                table: "LlegadaPlanta");

            migrationBuilder.AddForeignKey(
                name: "FK_LlegadaPlanta_Ingeniero",
                table: "LlegadaPlanta",
                column: "IdUsuario",
                principalTable: "Persona",
                principalColumn: "IdPersona",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
