using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablasMovimientoAlmacen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovimientoAlmacen",
                columns: table => new
                {
                    IdMovimientoAlmacen = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdTipoMovimiento = table.Column<int>(type: "integer", nullable: false),
                    IdUbicacion = table.Column<int>(type: "integer", nullable: false),
                    TipoStock = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    IdLlegadaPlanta = table.Column<int>(type: "integer", nullable: true),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    FRegistro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Observaciones = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IdUsuarioAnulacion = table.Column<int>(type: "integer", nullable: true),
                    FAnulacion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MotivoAnulacion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientoAlmacen", x => x.IdMovimientoAlmacen);
                    table.ForeignKey(
                        name: "FK_MovimientoAlmacen_LlegadaPlanta",
                        column: x => x.IdLlegadaPlanta,
                        principalTable: "LlegadaPlanta",
                        principalColumn: "IdLlegadaPlanta",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientoAlmacen_TipoMovimiento",
                        column: x => x.IdTipoMovimiento,
                        principalTable: "TipoMovimiento",
                        principalColumn: "IdTipoMovimiento",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientoAlmacen_Ubicacion",
                        column: x => x.IdUbicacion,
                        principalTable: "Ubicacion",
                        principalColumn: "IdUbicacion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientoAlmacen_Usuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientoAlmacen_UsuarioAnulacion",
                        column: x => x.IdUsuarioAnulacion,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DetalleMovimientoAlmacen",
                columns: table => new
                {
                    IdDetalleMovimientoAlmacen = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdMovimientoAlmacen = table.Column<int>(type: "integer", nullable: false),
                    IdVariedadProducto = table.Column<int>(type: "integer", nullable: false),
                    IdLote = table.Column<int>(type: "integer", nullable: true),
                    Cantidad = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    NumeroSacos = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleMovimientoAlmacen", x => x.IdDetalleMovimientoAlmacen);
                    table.ForeignKey(
                        name: "FK_DetalleMovimientoAlmacen_Lote",
                        column: x => x.IdLote,
                        principalTable: "Lote",
                        principalColumn: "IdLote",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetalleMovimientoAlmacen_MovimientoAlmacen",
                        column: x => x.IdMovimientoAlmacen,
                        principalTable: "MovimientoAlmacen",
                        principalColumn: "IdMovimientoAlmacen",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetalleMovimientoAlmacen_VariedadProducto",
                        column: x => x.IdVariedadProducto,
                        principalTable: "VariedadProducto",
                        principalColumn: "IdVariedadProducto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetalleMovimientoAlmacen_IdLote",
                table: "DetalleMovimientoAlmacen",
                column: "IdLote");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleMovimientoAlmacen_IdMovimientoAlmacen",
                table: "DetalleMovimientoAlmacen",
                column: "IdMovimientoAlmacen");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleMovimientoAlmacen_IdVariedadProducto",
                table: "DetalleMovimientoAlmacen",
                column: "IdVariedadProducto");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoAlmacen_IdLlegadaPlanta",
                table: "MovimientoAlmacen",
                column: "IdLlegadaPlanta");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoAlmacen_IdTipoMovimiento",
                table: "MovimientoAlmacen",
                column: "IdTipoMovimiento");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoAlmacen_IdUbicacion",
                table: "MovimientoAlmacen",
                column: "IdUbicacion");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoAlmacen_IdUsuario",
                table: "MovimientoAlmacen",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoAlmacen_IdUsuarioAnulacion",
                table: "MovimientoAlmacen",
                column: "IdUsuarioAnulacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetalleMovimientoAlmacen");

            migrationBuilder.DropTable(
                name: "MovimientoAlmacen");
        }
    }
}
