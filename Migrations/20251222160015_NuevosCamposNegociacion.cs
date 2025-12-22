using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class NuevosCamposNegociacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AprobadaEvidenciasPorIngeniero",
                table: "Negociacion",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstadoAprobacionIngenieroEvidencias",
                table: "Negociacion",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FAprobacionIngenieroEvidencias",
                table: "Negociacion",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FRechazoIngenieroEvidencias",
                table: "Negociacion",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RechazadaEvidenciasPorIngeniero",
                table: "Negociacion",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_AprobadaEvidenciasPorIngeniero",
                table: "Negociacion",
                column: "AprobadaEvidenciasPorIngeniero");

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_RechazadaEvidenciasPorIngeniero",
                table: "Negociacion",
                column: "RechazadaEvidenciasPorIngeniero");

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_AprobadaEvidenciasPorIngeniero",
                table: "Negociacion",
                column: "AprobadaEvidenciasPorIngeniero",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_RechazadaEvidenciasPorIngeniero",
                table: "Negociacion",
                column: "RechazadaEvidenciasPorIngeniero",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_AprobadaEvidenciasPorIngeniero",
                table: "Negociacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_RechazadaEvidenciasPorIngeniero",
                table: "Negociacion");

            migrationBuilder.DropIndex(
                name: "IX_Negociacion_AprobadaEvidenciasPorIngeniero",
                table: "Negociacion");

            migrationBuilder.DropIndex(
                name: "IX_Negociacion_RechazadaEvidenciasPorIngeniero",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "AprobadaEvidenciasPorIngeniero",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "EstadoAprobacionIngenieroEvidencias",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "FAprobacionIngenieroEvidencias",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "FRechazoIngenieroEvidencias",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "RechazadaEvidenciasPorIngeniero",
                table: "Negociacion");
        }
    }
}
