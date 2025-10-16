using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablasUnidadMedidaCategoriaProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PesoPorSaco",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "UnidadMedida",
                table: "Producto");

            migrationBuilder.AddColumn<int>(
                name: "IdCategoriaProducto",
                table: "Producto",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUnidadMedida",
                table: "Producto",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CategoriaProducto",
                columns: table => new
                {
                    IdCategoriaProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaProducto", x => x.IdCategoriaProducto);
                });

            migrationBuilder.CreateTable(
                name: "UnidadMedida",
                columns: table => new
                {
                    IdUnidadMedida = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Abreviatura = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadMedida", x => x.IdUnidadMedida);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Producto_IdCategoriaProducto",
                table: "Producto",
                column: "IdCategoriaProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_IdUnidadMedida",
                table: "Producto",
                column: "IdUnidadMedida");

            migrationBuilder.AddForeignKey(
                name: "FK_Producto_CategoriaProducto",
                table: "Producto",
                column: "IdCategoriaProducto",
                principalTable: "CategoriaProducto",
                principalColumn: "IdCategoriaProducto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Producto_UnidadMedida",
                table: "Producto",
                column: "IdUnidadMedida",
                principalTable: "UnidadMedida",
                principalColumn: "IdUnidadMedida",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producto_CategoriaProducto",
                table: "Producto");

            migrationBuilder.DropForeignKey(
                name: "FK_Producto_UnidadMedida",
                table: "Producto");

            migrationBuilder.DropTable(
                name: "CategoriaProducto");

            migrationBuilder.DropTable(
                name: "UnidadMedida");

            migrationBuilder.DropIndex(
                name: "IX_Producto_IdCategoriaProducto",
                table: "Producto");

            migrationBuilder.DropIndex(
                name: "IX_Producto_IdUnidadMedida",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "IdCategoriaProducto",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "IdUnidadMedida",
                table: "Producto");

            migrationBuilder.AddColumn<decimal>(
                name: "PesoPorSaco",
                table: "Producto",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnidadMedida",
                table: "Producto",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
