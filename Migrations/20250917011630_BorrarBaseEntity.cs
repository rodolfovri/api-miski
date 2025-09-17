using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class BorrarBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Vehiculo");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Vehiculo");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Vehiculo");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Vehiculo");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Vehiculo");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Vehiculo");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Ubicacion");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Ubicacion");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Ubicacion");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Ubicacion");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Ubicacion");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Ubicacion");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TipoDocumento");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "TipoDocumento");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TipoDocumento");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TipoDocumento");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "TipoDocumento");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "TipoDocumento");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Rol");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Rol");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Rol");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Rol");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Rol");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Rol");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PersonaUbicacion");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PersonaUbicacion");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PersonaUbicacion");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PersonaUbicacion");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PersonaUbicacion");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PersonaUbicacion");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PersonaCategoria");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PersonaCategoria");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PersonaCategoria");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PersonaCategoria");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PersonaCategoria");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PersonaCategoria");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Persona");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Persona");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Persona");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Persona");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Persona");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Persona");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "FechaAprobacion",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Negociacion");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Lote");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Lote");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Lote");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Lote");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Lote");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Lote");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "LlegadaPlantaDetalle");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "LlegadaPlantaDetalle");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LlegadaPlantaDetalle");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "LlegadaPlantaDetalle");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "LlegadaPlantaDetalle");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "LlegadaPlantaDetalle");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "LlegadaPlanta");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "LlegadaPlanta");

            migrationBuilder.DropColumn(
                name: "FechaLlegada",
                table: "LlegadaPlanta");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LlegadaPlanta");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "LlegadaPlanta");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "LlegadaPlanta");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Compra");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Compra");

            migrationBuilder.DropColumn(
                name: "FechaEmision",
                table: "Compra");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Compra");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Compra");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Compra");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CategoriaPersona");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CategoriaPersona");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CategoriaPersona");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CategoriaPersona");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CategoriaPersona");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "CategoriaPersona");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Negociacion",
                newName: "FAprobacion");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "Negociacion",
                newName: "FRegistro");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "LlegadaPlanta",
                newName: "FLlegada");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Compra",
                newName: "FRegistro");

            migrationBuilder.RenameColumn(
                name: "FechaRegistro",
                table: "Compra",
                newName: "FEmision");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FRegistro",
                table: "Negociacion",
                newName: "Fecha");

            migrationBuilder.RenameColumn(
                name: "FAprobacion",
                table: "Negociacion",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "FLlegada",
                table: "LlegadaPlanta",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "FRegistro",
                table: "Compra",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "FEmision",
                table: "Compra",
                newName: "FechaRegistro");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Vehiculo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Vehiculo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Vehiculo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Vehiculo",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Vehiculo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Vehiculo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Usuario",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Usuario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Usuario",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Usuario",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Ubicacion",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Ubicacion",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Ubicacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Ubicacion",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Ubicacion",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Ubicacion",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TipoDocumento",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "TipoDocumento",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TipoDocumento",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TipoDocumento",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "TipoDocumento",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "TipoDocumento",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Stock",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Stock",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Stock",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Stock",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Stock",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Stock",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Rol",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Rol",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Rol",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Rol",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Rol",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Rol",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Producto",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Producto",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Producto",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Producto",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Producto",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PersonaUbicacion",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PersonaUbicacion",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PersonaUbicacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PersonaUbicacion",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PersonaUbicacion",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "PersonaUbicacion",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PersonaCategoria",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PersonaCategoria",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PersonaCategoria",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PersonaCategoria",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PersonaCategoria",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "PersonaCategoria",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Persona",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Persona",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Persona",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Persona",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Persona",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Persona",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Negociacion",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Negociacion",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAprobacion",
                table: "Negociacion",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Negociacion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Negociacion",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Negociacion",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Lote",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Lote",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Lote",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Lote",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Lote",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Lote",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "LlegadaPlantaDetalle",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "LlegadaPlantaDetalle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "LlegadaPlantaDetalle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "LlegadaPlantaDetalle",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "LlegadaPlantaDetalle",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "LlegadaPlantaDetalle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "LlegadaPlanta",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "LlegadaPlanta",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaLlegada",
                table: "LlegadaPlanta",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "LlegadaPlanta",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "LlegadaPlanta",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "LlegadaPlanta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Compra",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Compra",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEmision",
                table: "Compra",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Compra",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Compra",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Compra",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CategoriaPersona",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CategoriaPersona",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CategoriaPersona",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CategoriaPersona",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CategoriaPersona",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "CategoriaPersona",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
