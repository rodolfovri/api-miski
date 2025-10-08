using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablasExtras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compra_Vehiculo",
                table: "Compra");

            migrationBuilder.RenameColumn(
                name: "IdVehiculo",
                table: "Compra",
                newName: "VehiculoIdVehiculo");

            migrationBuilder.RenameIndex(
                name: "IX_Compra_IdVehiculo",
                table: "Compra",
                newName: "IX_Compra_VehiculoIdVehiculo");

            migrationBuilder.CreateTable(
                name: "CompraVehiculo",
                columns: table => new
                {
                    IdCompraVehiculo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCompra = table.Column<int>(type: "int", nullable: false),
                    IdVehiculo = table.Column<int>(type: "int", nullable: false),
                    GuiaRemision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraVehiculo", x => x.IdCompraVehiculo);
                    table.ForeignKey(
                        name: "FK_CompraVehiculo_Compra",
                        column: x => x.IdCompra,
                        principalTable: "Compra",
                        principalColumn: "IdCompra",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompraVehiculo_Vehiculo",
                        column: x => x.IdVehiculo,
                        principalTable: "Vehiculo",
                        principalColumn: "IdVehiculo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrackingPersona",
                columns: table => new
                {
                    IdTracking = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPersona = table.Column<int>(type: "int", nullable: false),
                    Latitud = table.Column<string>(type: "nvarchar(max)", precision: 10, scale: 7, nullable: false),
                    Longitud = table.Column<string>(type: "nvarchar(max)", precision: 10, scale: 7, nullable: false),
                    FRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingPersona", x => x.IdTracking);
                    table.ForeignKey(
                        name: "FK_TrackingPersona_Persona",
                        column: x => x.IdPersona,
                        principalTable: "Persona",
                        principalColumn: "IdPersona",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompraVehiculo_IdCompra",
                table: "CompraVehiculo",
                column: "IdCompra");

            migrationBuilder.CreateIndex(
                name: "IX_CompraVehiculo_IdVehiculo",
                table: "CompraVehiculo",
                column: "IdVehiculo");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingPersona_IdPersona",
                table: "TrackingPersona",
                column: "IdPersona");

            migrationBuilder.AddForeignKey(
                name: "FK_Compra_Vehiculo_VehiculoIdVehiculo",
                table: "Compra",
                column: "VehiculoIdVehiculo",
                principalTable: "Vehiculo",
                principalColumn: "IdVehiculo",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compra_Vehiculo_VehiculoIdVehiculo",
                table: "Compra");

            migrationBuilder.DropTable(
                name: "CompraVehiculo");

            migrationBuilder.DropTable(
                name: "TrackingPersona");

            migrationBuilder.RenameColumn(
                name: "VehiculoIdVehiculo",
                table: "Compra",
                newName: "IdVehiculo");

            migrationBuilder.RenameIndex(
                name: "IX_Compra_VehiculoIdVehiculo",
                table: "Compra",
                newName: "IX_Compra_IdVehiculo");

            migrationBuilder.AddForeignKey(
                name: "FK_Compra_Vehiculo",
                table: "Compra",
                column: "IdVehiculo",
                principalTable: "Vehiculo",
                principalColumn: "IdVehiculo",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
