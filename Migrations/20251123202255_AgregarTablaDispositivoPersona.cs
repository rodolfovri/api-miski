using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablaDispositivoPersona : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsActual",
                table: "TrackingPersona",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "TrackingPersona",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "ACTIVO");

            migrationBuilder.AddColumn<DateTime>(
                name: "FActualizacion",
                table: "TrackingPersona",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Precision",
                table: "TrackingPersona",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Velocidad",
                table: "TrackingPersona",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DispositivoPersona",
                columns: table => new
                {
                    IdDispositivoPersona = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPersona = table.Column<int>(type: "integer", nullable: false),
                    DeviceId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ModeloDispositivo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SistemaOperativo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    VersionApp = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    FUltimaActividad = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispositivoPersona", x => x.IdDispositivoPersona);
                    table.ForeignKey(
                        name: "FK_DispositivoPersona_Persona",
                        column: x => x.IdPersona,
                        principalTable: "Persona",
                        principalColumn: "IdPersona",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackingPersona_FRegistro",
                table: "TrackingPersona",
                column: "FRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingPersona_IdPersona_EsActual",
                table: "TrackingPersona",
                columns: new[] { "IdPersona", "EsActual" });

            migrationBuilder.CreateIndex(
                name: "IX_DispositivoPersona_DeviceId",
                table: "DispositivoPersona",
                column: "DeviceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DispositivoPersona_IdPersona",
                table: "DispositivoPersona",
                column: "IdPersona");

            migrationBuilder.CreateIndex(
                name: "IX_DispositivoPersona_IdPersona_Activo",
                table: "DispositivoPersona",
                columns: new[] { "IdPersona", "Activo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DispositivoPersona");

            migrationBuilder.DropIndex(
                name: "IX_TrackingPersona_FRegistro",
                table: "TrackingPersona");

            migrationBuilder.DropIndex(
                name: "IX_TrackingPersona_IdPersona_EsActual",
                table: "TrackingPersona");

            migrationBuilder.DropColumn(
                name: "EsActual",
                table: "TrackingPersona");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "TrackingPersona");

            migrationBuilder.DropColumn(
                name: "FActualizacion",
                table: "TrackingPersona");

            migrationBuilder.DropColumn(
                name: "Precision",
                table: "TrackingPersona");

            migrationBuilder.DropColumn(
                name: "Velocidad",
                table: "TrackingPersona");
        }
    }
}
