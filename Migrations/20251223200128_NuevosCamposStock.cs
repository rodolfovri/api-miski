using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class NuevosCamposStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CantidadSacos",
                table: "Stock",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoStock",
                table: "Stock",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadSacos",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "TipoStock",
                table: "Stock");
        }
    }
}
