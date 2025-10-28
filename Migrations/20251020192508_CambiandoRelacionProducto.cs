using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class CambiandoRelacionProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producto_UnidadMedida",
                table: "Producto");

            migrationBuilder.DropIndex(
                name: "IX_Producto_IdUnidadMedida",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "IdUnidadMedida",
                table: "Producto");

            migrationBuilder.CreateTable(
                name: "VariedadProducto",
                columns: table => new
                {
                    IdVariedadProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    IdUnidadMedida = table.Column<int>(type: "int", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariedadProducto", x => x.IdVariedadProducto);
                    table.ForeignKey(
                        name: "FK_VariedadProducto_Producto",
                        column: x => x.IdProducto,
                        principalTable: "Producto",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VariedadProducto_UnidadMedida",
                        column: x => x.IdUnidadMedida,
                        principalTable: "UnidadMedida",
                        principalColumn: "IdUnidadMedida",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VariedadProducto_IdProducto",
                table: "VariedadProducto",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_VariedadProducto_IdUnidadMedida",
                table: "VariedadProducto",
                column: "IdUnidadMedida");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VariedadProducto");

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "Producto",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IdUnidadMedida",
                table: "Producto",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Producto_IdUnidadMedida",
                table: "Producto",
                column: "IdUnidadMedida");

            migrationBuilder.AddForeignKey(
                name: "FK_Producto_UnidadMedida",
                table: "Producto",
                column: "IdUnidadMedida",
                principalTable: "UnidadMedida",
                principalColumn: "IdUnidadMedida",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
