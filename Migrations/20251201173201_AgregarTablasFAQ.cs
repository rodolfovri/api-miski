using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablasFAQ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriaFAQ",
                columns: table => new
                {
                    IdCategoriaFAQ = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaFAQ", x => x.IdCategoriaFAQ);
                });

            migrationBuilder.CreateTable(
                name: "FAQ",
                columns: table => new
                {
                    IdFAQ = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCategoriaFAQ = table.Column<int>(type: "integer", nullable: false),
                    Pregunta = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Respuesta = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false),
                    FRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQ", x => x.IdFAQ);
                    table.ForeignKey(
                        name: "FK_FAQ_CategoriaFAQ",
                        column: x => x.IdCategoriaFAQ,
                        principalTable: "CategoriaFAQ",
                        principalColumn: "IdCategoriaFAQ",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FAQ_IdCategoriaFAQ",
                table: "FAQ",
                column: "IdCategoriaFAQ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FAQ");

            migrationBuilder.DropTable(
                name: "CategoriaFAQ");
        }
    }
}
