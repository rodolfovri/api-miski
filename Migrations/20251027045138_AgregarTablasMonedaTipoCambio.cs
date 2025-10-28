using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablasMonedaTipoCambio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Moneda",
                columns: table => new
                {
                    IdMoneda = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Simbolo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moneda", x => x.IdMoneda);
                });

            migrationBuilder.CreateTable(
                name: "TipoCambio",
                columns: table => new
                {
                    IdTipoCambio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMoneda = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    ValorCompra = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ValorVenta = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    FRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCambio", x => x.IdTipoCambio);
                    table.ForeignKey(
                        name: "FK_TipoCambio_Moneda",
                        column: x => x.IdMoneda,
                        principalTable: "Moneda",
                        principalColumn: "IdMoneda",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TipoCambio_Usuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TipoCambio_IdMoneda",
                table: "TipoCambio",
                column: "IdMoneda");

            migrationBuilder.CreateIndex(
                name: "IX_TipoCambio_IdUsuario",
                table: "TipoCambio",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TipoCambio");

            migrationBuilder.DropTable(
                name: "Moneda");
        }
    }
}
