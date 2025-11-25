using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class ModificarPermisosNuevos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubModuloAccion",
                columns: table => new
                {
                    IdSubModuloAccion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdSubModulo = table.Column<int>(type: "integer", nullable: false),
                    IdAccion = table.Column<int>(type: "integer", nullable: false),
                    Habilitado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubModuloAccion", x => x.IdSubModuloAccion);
                    table.ForeignKey(
                        name: "FK_SubModuloAccion_Accion",
                        column: x => x.IdAccion,
                        principalTable: "Accion",
                        principalColumn: "IdAccion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubModuloAccion_SubModulo",
                        column: x => x.IdSubModulo,
                        principalTable: "SubModulo",
                        principalColumn: "IdSubModulo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubModuloDetalleAccion",
                columns: table => new
                {
                    IdSubModuloDetalleAccion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdSubModuloDetalle = table.Column<int>(type: "integer", nullable: false),
                    IdAccion = table.Column<int>(type: "integer", nullable: false),
                    Habilitado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubModuloDetalleAccion", x => x.IdSubModuloDetalleAccion);
                    table.ForeignKey(
                        name: "FK_SubModuloDetalleAccion_Accion",
                        column: x => x.IdAccion,
                        principalTable: "Accion",
                        principalColumn: "IdAccion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubModuloDetalleAccion_SubModuloDetalle",
                        column: x => x.IdSubModuloDetalle,
                        principalTable: "SubModuloDetalle",
                        principalColumn: "IdSubModuloDetalle",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubModuloAccion_IdAccion",
                table: "SubModuloAccion",
                column: "IdAccion");

            migrationBuilder.CreateIndex(
                name: "IX_SubModuloAccion_IdSubModulo",
                table: "SubModuloAccion",
                column: "IdSubModulo");

            migrationBuilder.CreateIndex(
                name: "IX_SubModuloDetalleAccion_IdAccion",
                table: "SubModuloDetalleAccion",
                column: "IdAccion");

            migrationBuilder.CreateIndex(
                name: "IX_SubModuloDetalleAccion_IdSubModuloDetalle",
                table: "SubModuloDetalleAccion",
                column: "IdSubModuloDetalle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubModuloAccion");

            migrationBuilder.DropTable(
                name: "SubModuloDetalleAccion");
        }
    }
}
