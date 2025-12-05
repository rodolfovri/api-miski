using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablaCompraPago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoPago",
                table: "Compra",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CompraPago",
                columns: table => new
                {
                    IdCompraPago = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCompra = table.Column<int>(type: "integer", nullable: false),
                    TipoPago = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DiasCredito = table.Column<int>(type: "integer", nullable: true),
                    FPago = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MontoAcuenta = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Saldo = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    EstadoPago = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Observacion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    FRegistro = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraPago", x => x.IdCompraPago);
                    table.ForeignKey(
                        name: "FK_CompraPago_Compra",
                        column: x => x.IdCompra,
                        principalTable: "Compra",
                        principalColumn: "IdCompra",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompraPago_IdCompra",
                table: "CompraPago",
                column: "IdCompra");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompraPago");

            migrationBuilder.DropColumn(
                name: "TipoPago",
                table: "Compra");
        }
    }
}
