using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class QuitarRestriccionUsuarioUbicacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ubicacion_IdUsuario",
                table: "Ubicacion");

            migrationBuilder.CreateIndex(
                name: "IX_Ubicacion_IdUsuario",
                table: "Ubicacion",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ubicacion_IdUsuario",
                table: "Ubicacion");

            migrationBuilder.CreateIndex(
                name: "IX_Ubicacion_IdUsuario",
                table: "Ubicacion",
                column: "IdUsuario",
                unique: true);
        }
    }
}
