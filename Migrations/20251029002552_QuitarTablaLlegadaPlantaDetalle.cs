using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class QuitarTablaLlegadaPlantaDetalle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LlegadaPlantaDetalle");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FLlegada",
                table: "LlegadaPlanta",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdLote",
                table: "LlegadaPlanta",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "PesoRecibido",
                table: "LlegadaPlanta",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SacosRecibidos",
                table: "LlegadaPlanta",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_LlegadaPlanta_IdLote",
                table: "LlegadaPlanta",
                column: "IdLote");

            migrationBuilder.AddForeignKey(
                name: "FK_LlegadaPlanta_Lote",
                table: "LlegadaPlanta",
                column: "IdLote",
                principalTable: "Lote",
                principalColumn: "IdLote",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LlegadaPlanta_Lote",
                table: "LlegadaPlanta");

            migrationBuilder.DropIndex(
                name: "IX_LlegadaPlanta_IdLote",
                table: "LlegadaPlanta");

            migrationBuilder.DropColumn(
                name: "IdLote",
                table: "LlegadaPlanta");

            migrationBuilder.DropColumn(
                name: "PesoRecibido",
                table: "LlegadaPlanta");

            migrationBuilder.DropColumn(
                name: "SacosRecibidos",
                table: "LlegadaPlanta");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FLlegada",
                table: "LlegadaPlanta",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.CreateTable(
                name: "LlegadaPlantaDetalle",
                columns: table => new
                {
                    IdLlegadaDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdLlegadaPlanta = table.Column<int>(type: "int", nullable: false),
                    IdLote = table.Column<int>(type: "int", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PesoRecibido = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SacosRecibidos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LlegadaPlantaDetalle", x => x.IdLlegadaDetalle);
                    table.ForeignKey(
                        name: "FK_LlegadaDetalle_Llegada",
                        column: x => x.IdLlegadaPlanta,
                        principalTable: "LlegadaPlanta",
                        principalColumn: "IdLlegadaPlanta",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LlegadaDetalle_Lote",
                        column: x => x.IdLote,
                        principalTable: "Lote",
                        principalColumn: "IdLote",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LlegadaPlantaDetalle_IdLlegadaPlanta",
                table: "LlegadaPlantaDetalle",
                column: "IdLlegadaPlanta");

            migrationBuilder.CreateIndex(
                name: "IX_LlegadaPlantaDetalle_IdLote",
                table: "LlegadaPlantaDetalle",
                column: "IdLote");
        }
    }
}
