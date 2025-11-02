using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCampoEnLote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Comision",
                table: "Lote",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observacion",
                table: "Lote",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comision",
                table: "Lote");

            migrationBuilder.DropColumn(
                name: "Observacion",
                table: "Lote");
        }
    }
}
