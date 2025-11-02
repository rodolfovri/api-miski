using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCampoEstadoRecepcionTablaCompra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EstadoRecepcion",
                table: "Compra",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoRecepcion",
                table: "Compra");
        }
    }
}
