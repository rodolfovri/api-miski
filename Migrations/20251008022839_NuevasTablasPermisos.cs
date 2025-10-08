using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class NuevasTablasPermisos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Modulo",
                columns: table => new
                {
                    IdModulo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TipoPlataforma = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modulo", x => x.IdModulo);
                });

            migrationBuilder.CreateTable(
                name: "SubModulo",
                columns: table => new
                {
                    IdSubModulo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdModulo = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubModulo", x => x.IdSubModulo);
                    table.ForeignKey(
                        name: "FK_SubModulo_Modulo",
                        column: x => x.IdModulo,
                        principalTable: "Modulo",
                        principalColumn: "IdModulo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubModuloDetalle",
                columns: table => new
                {
                    IdSubModuloDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSubModulo = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubModuloDetalle", x => x.IdSubModuloDetalle);
                    table.ForeignKey(
                        name: "FK_SubModuloDetalle_SubModulo",
                        column: x => x.IdSubModulo,
                        principalTable: "SubModulo",
                        principalColumn: "IdSubModulo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermisoRol",
                columns: table => new
                {
                    IdPermisoRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRol = table.Column<int>(type: "int", nullable: false),
                    IdModulo = table.Column<int>(type: "int", nullable: true),
                    IdSubModulo = table.Column<int>(type: "int", nullable: true),
                    IdSubModuloDetalle = table.Column<int>(type: "int", nullable: true),
                    TieneAcceso = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermisoRol", x => x.IdPermisoRol);
                    table.ForeignKey(
                        name: "FK_PermisoRol_Modulo",
                        column: x => x.IdModulo,
                        principalTable: "Modulo",
                        principalColumn: "IdModulo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermisoRol_Rol",
                        column: x => x.IdRol,
                        principalTable: "Rol",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermisoRol_SubModulo",
                        column: x => x.IdSubModulo,
                        principalTable: "SubModulo",
                        principalColumn: "IdSubModulo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermisoRol_SubModuloDetalle",
                        column: x => x.IdSubModuloDetalle,
                        principalTable: "SubModuloDetalle",
                        principalColumn: "IdSubModuloDetalle",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermisoRol_IdModulo",
                table: "PermisoRol",
                column: "IdModulo");

            migrationBuilder.CreateIndex(
                name: "IX_PermisoRol_IdRol",
                table: "PermisoRol",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_PermisoRol_IdSubModulo",
                table: "PermisoRol",
                column: "IdSubModulo");

            migrationBuilder.CreateIndex(
                name: "IX_PermisoRol_IdSubModuloDetalle",
                table: "PermisoRol",
                column: "IdSubModuloDetalle");

            migrationBuilder.CreateIndex(
                name: "IX_SubModulo_IdModulo",
                table: "SubModulo",
                column: "IdModulo");

            migrationBuilder.CreateIndex(
                name: "IX_SubModuloDetalle_IdSubModulo",
                table: "SubModuloDetalle",
                column: "IdSubModulo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermisoRol");

            migrationBuilder.DropTable(
                name: "SubModuloDetalle");

            migrationBuilder.DropTable(
                name: "SubModulo");

            migrationBuilder.DropTable(
                name: "Modulo");
        }
    }
}
