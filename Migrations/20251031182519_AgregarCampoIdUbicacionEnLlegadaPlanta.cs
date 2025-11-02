using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCampoIdUbicacionEnLlegadaPlanta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdUbicacion",
                table: "LlegadaPlanta",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LlegadaPlanta_IdUbicacion",
                table: "LlegadaPlanta",
                column: "IdUbicacion");

            migrationBuilder.AddForeignKey(
                name: "FK_LlegadaPlanta_Ubicacion",
                table: "LlegadaPlanta",
                column: "IdUbicacion",
                principalTable: "Ubicacion",
                principalColumn: "IdUbicacion",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LlegadaPlanta_Ubicacion",
                table: "LlegadaPlanta");

            migrationBuilder.DropIndex(
                name: "IX_LlegadaPlanta_IdUbicacion",
                table: "LlegadaPlanta");

            migrationBuilder.DropColumn(
                name: "IdUbicacion",
                table: "LlegadaPlanta");
        }
    }
}
