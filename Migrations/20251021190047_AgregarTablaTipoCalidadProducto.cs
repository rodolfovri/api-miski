using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablaTipoCalidadProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoCalidadProducto",
                columns: table => new
                {
                    IdTipoCalidadProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProducto = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCalidadProducto", x => x.IdTipoCalidadProducto);
                    table.ForeignKey(
                        name: "FK_TipoCalidadProducto_Producto",
                        column: x => x.IdProducto,
                        principalTable: "Producto",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TipoCalidadProducto_IdProducto",
                table: "TipoCalidadProducto",
                column: "IdProducto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TipoCalidadProducto");
        }
    }
}
