using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class ModificarPermisosRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icono",
                table: "SubModuloDetalle",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ruta",
                table: "SubModuloDetalle",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icono",
                table: "SubModulo",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ruta",
                table: "SubModulo",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TieneDetalles",
                table: "SubModulo",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Icono",
                table: "Modulo",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ruta",
                table: "Modulo",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Accion",
                columns: table => new
                {
                    IdAccion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Icono = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accion", x => x.IdAccion);
                });

            migrationBuilder.CreateTable(
                name: "PermisoRolAccion",
                columns: table => new
                {
                    IdPermisoRolAccion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPermisoRol = table.Column<int>(type: "integer", nullable: false),
                    IdAccion = table.Column<int>(type: "integer", nullable: false),
                    Habilitado = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermisoRolAccion", x => x.IdPermisoRolAccion);
                    table.ForeignKey(
                        name: "FK_PermisoRolAccion_Accion",
                        column: x => x.IdAccion,
                        principalTable: "Accion",
                        principalColumn: "IdAccion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermisoRolAccion_PermisoRol",
                        column: x => x.IdPermisoRol,
                        principalTable: "PermisoRol",
                        principalColumn: "IdPermisoRol",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermisoRolAccion_IdAccion",
                table: "PermisoRolAccion",
                column: "IdAccion");

            migrationBuilder.CreateIndex(
                name: "IX_PermisoRolAccion_IdPermisoRol",
                table: "PermisoRolAccion",
                column: "IdPermisoRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermisoRolAccion");

            migrationBuilder.DropTable(
                name: "Accion");

            migrationBuilder.DropColumn(
                name: "Icono",
                table: "SubModuloDetalle");

            migrationBuilder.DropColumn(
                name: "Ruta",
                table: "SubModuloDetalle");

            migrationBuilder.DropColumn(
                name: "Icono",
                table: "SubModulo");

            migrationBuilder.DropColumn(
                name: "Ruta",
                table: "SubModulo");

            migrationBuilder.DropColumn(
                name: "TieneDetalles",
                table: "SubModulo");

            migrationBuilder.DropColumn(
                name: "Icono",
                table: "Modulo");

            migrationBuilder.DropColumn(
                name: "Ruta",
                table: "Modulo");
        }
    }
}
