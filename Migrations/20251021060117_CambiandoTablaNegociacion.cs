using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class CambiandoTablaNegociacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LlegadaPlanta_Usuario_UsuarioIdUsuario",
                table: "LlegadaPlanta");

            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_AprobadaPor",
                table: "Negociacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_Proveedor",
                table: "Negociacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_Usuario_UsuarioIdUsuario",
                table: "Negociacion");

            migrationBuilder.DropIndex(
                name: "IX_LlegadaPlanta_UsuarioIdUsuario",
                table: "LlegadaPlanta");

            migrationBuilder.DropColumn(
                name: "EstadoAprobado",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "UsuarioIdUsuario",
                table: "LlegadaPlanta");

            migrationBuilder.RenameColumn(
                name: "UsuarioIdUsuario",
                table: "Negociacion",
                newName: "RechazadoPorIngeniero");

            migrationBuilder.RenameColumn(
                name: "NroCuentaRuc",
                table: "Negociacion",
                newName: "TipoCalidad");

            migrationBuilder.RenameColumn(
                name: "IdProveedor",
                table: "Negociacion",
                newName: "RechazadoPorContadora");

            migrationBuilder.RenameColumn(
                name: "FotoCalidadProducto",
                table: "Negociacion",
                newName: "TerceraEvindenciaFoto");

            migrationBuilder.RenameColumn(
                name: "FAprobacion",
                table: "Negociacion",
                newName: "FRechazoIngeniero");

            migrationBuilder.RenameColumn(
                name: "AprobadaPor",
                table: "Negociacion",
                newName: "ProveedorIdPersona");

            migrationBuilder.RenameIndex(
                name: "IX_Negociacion_UsuarioIdUsuario",
                table: "Negociacion",
                newName: "IX_Negociacion_RechazadoPorIngeniero");

            migrationBuilder.RenameIndex(
                name: "IX_Negociacion_IdProveedor",
                table: "Negociacion",
                newName: "IX_Negociacion_RechazadoPorContadora");

            migrationBuilder.RenameIndex(
                name: "IX_Negociacion_AprobadaPor",
                table: "Negociacion",
                newName: "IX_Negociacion_ProveedorIdPersona");

            migrationBuilder.AlterColumn<int>(
                name: "SacosTotales",
                table: "Negociacion",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AprobadaPorContadora",
                table: "Negociacion",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AprobadaPorIngeniero",
                table: "Negociacion",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstadoAprobacionContadora",
                table: "Negociacion",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstadoAprobacionIngeniero",
                table: "Negociacion",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EvidenciaVideo",
                table: "Negociacion",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FAdelanto",
                table: "Negociacion",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FAprobacionContadora",
                table: "Negociacion",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FAprobacionIngeniero",
                table: "Negociacion",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FPagoTotal",
                table: "Negociacion",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FRechazoContadora",
                table: "Negociacion",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MontoAdelanto",
                table: "Negociacion",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MontoTotalPago",
                table: "Negociacion",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NroCuentaBancaria",
                table: "Negociacion",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NroDocumentoProveedor",
                table: "Negociacion",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PesoPorSaco",
                table: "Negociacion",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimeraEvindenciaFoto",
                table: "Negociacion",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SegundaEvindenciaFoto",
                table: "Negociacion",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_AprobadaPorContadora",
                table: "Negociacion",
                column: "AprobadaPorContadora");

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_AprobadaPorIngeniero",
                table: "Negociacion",
                column: "AprobadaPorIngeniero");

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_AprobadaPorContadora",
                table: "Negociacion",
                column: "AprobadaPorContadora",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_AprobadaPorIngeniero",
                table: "Negociacion",
                column: "AprobadaPorIngeniero",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_Persona_ProveedorIdPersona",
                table: "Negociacion",
                column: "ProveedorIdPersona",
                principalTable: "Persona",
                principalColumn: "IdPersona");

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_RechazadoPorContadora",
                table: "Negociacion",
                column: "RechazadoPorContadora",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_RechazadoPorIngeniero",
                table: "Negociacion",
                column: "RechazadoPorIngeniero",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_AprobadaPorContadora",
                table: "Negociacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_AprobadaPorIngeniero",
                table: "Negociacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_Persona_ProveedorIdPersona",
                table: "Negociacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_RechazadoPorContadora",
                table: "Negociacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Negociacion_RechazadoPorIngeniero",
                table: "Negociacion");

            migrationBuilder.DropIndex(
                name: "IX_Negociacion_AprobadaPorContadora",
                table: "Negociacion");

            migrationBuilder.DropIndex(
                name: "IX_Negociacion_AprobadaPorIngeniero",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "AprobadaPorContadora",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "AprobadaPorIngeniero",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "EstadoAprobacionContadora",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "EstadoAprobacionIngeniero",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "EvidenciaVideo",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "FAdelanto",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "FAprobacionContadora",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "FAprobacionIngeniero",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "FPagoTotal",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "FRechazoContadora",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "MontoAdelanto",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "MontoTotalPago",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "NroCuentaBancaria",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "NroDocumentoProveedor",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "PesoPorSaco",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "PrimeraEvindenciaFoto",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "SegundaEvindenciaFoto",
                table: "Negociacion");

            migrationBuilder.RenameColumn(
                name: "TipoCalidad",
                table: "Negociacion",
                newName: "NroCuentaRuc");

            migrationBuilder.RenameColumn(
                name: "TerceraEvindenciaFoto",
                table: "Negociacion",
                newName: "FotoCalidadProducto");

            migrationBuilder.RenameColumn(
                name: "RechazadoPorIngeniero",
                table: "Negociacion",
                newName: "UsuarioIdUsuario");

            migrationBuilder.RenameColumn(
                name: "RechazadoPorContadora",
                table: "Negociacion",
                newName: "IdProveedor");

            migrationBuilder.RenameColumn(
                name: "ProveedorIdPersona",
                table: "Negociacion",
                newName: "AprobadaPor");

            migrationBuilder.RenameColumn(
                name: "FRechazoIngeniero",
                table: "Negociacion",
                newName: "FAprobacion");

            migrationBuilder.RenameIndex(
                name: "IX_Negociacion_RechazadoPorIngeniero",
                table: "Negociacion",
                newName: "IX_Negociacion_UsuarioIdUsuario");

            migrationBuilder.RenameIndex(
                name: "IX_Negociacion_RechazadoPorContadora",
                table: "Negociacion",
                newName: "IX_Negociacion_IdProveedor");

            migrationBuilder.RenameIndex(
                name: "IX_Negociacion_ProveedorIdPersona",
                table: "Negociacion",
                newName: "IX_Negociacion_AprobadaPor");

            migrationBuilder.AlterColumn<int>(
                name: "SacosTotales",
                table: "Negociacion",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstadoAprobado",
                table: "Negociacion",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioIdUsuario",
                table: "LlegadaPlanta",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LlegadaPlanta_UsuarioIdUsuario",
                table: "LlegadaPlanta",
                column: "UsuarioIdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_LlegadaPlanta_Usuario_UsuarioIdUsuario",
                table: "LlegadaPlanta",
                column: "UsuarioIdUsuario",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_AprobadaPor",
                table: "Negociacion",
                column: "AprobadaPor",
                principalTable: "Usuario",
                principalColumn: "IdUsuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_Proveedor",
                table: "Negociacion",
                column: "IdProveedor",
                principalTable: "Persona",
                principalColumn: "IdPersona",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Negociacion_Usuario_UsuarioIdUsuario",
                table: "Negociacion",
                column: "UsuarioIdUsuario",
                principalTable: "Usuario",
                principalColumn: "IdUsuario");
        }
    }
}
