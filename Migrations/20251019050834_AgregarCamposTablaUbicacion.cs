using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposTablaUbicacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoSenasa",
                table: "Ubicacion",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ComprobantePdf",
                table: "Ubicacion",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DomicilioLegal",
                table: "Ubicacion",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GiroEstablecimiento",
                table: "Ubicacion",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroRuc",
                table: "Ubicacion",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RazonSocial",
                table: "Ubicacion",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoSenasa",
                table: "Ubicacion");

            migrationBuilder.DropColumn(
                name: "ComprobantePdf",
                table: "Ubicacion");

            migrationBuilder.DropColumn(
                name: "DomicilioLegal",
                table: "Ubicacion");

            migrationBuilder.DropColumn(
                name: "GiroEstablecimiento",
                table: "Ubicacion");

            migrationBuilder.DropColumn(
                name: "NumeroRuc",
                table: "Ubicacion");

            migrationBuilder.DropColumn(
                name: "RazonSocial",
                table: "Ubicacion");
        }
    }
}
