using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCompraVehiculoDetalle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompraVehiculo_Compra",
                table: "CompraVehiculo");

            migrationBuilder.DropIndex(
                name: "IX_CompraVehiculo_IdCompra",
                table: "CompraVehiculo");

            migrationBuilder.DropColumn(
                name: "IdCompra",
                table: "CompraVehiculo");

            migrationBuilder.DropColumn(
                name: "GuiaRemision",
                table: "Compra");

            migrationBuilder.AlterColumn<string>(
                name: "GuiaRemision",
                table: "CompraVehiculo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "CompraVehiculoDetalle",
                columns: table => new
                {
                    IdCompraVehiculoDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCompraVehiculo = table.Column<int>(type: "int", nullable: false),
                    IdCompra = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraVehiculoDetalle", x => x.IdCompraVehiculoDetalle);
                    table.ForeignKey(
                        name: "FK_CompraVehiculoDetalle_Compra",
                        column: x => x.IdCompra,
                        principalTable: "Compra",
                        principalColumn: "IdCompra",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompraVehiculoDetalle_CompraVehiculo",
                        column: x => x.IdCompraVehiculo,
                        principalTable: "CompraVehiculo",
                        principalColumn: "IdCompraVehiculo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompraVehiculoDetalle_IdCompra",
                table: "CompraVehiculoDetalle",
                column: "IdCompra");

            migrationBuilder.CreateIndex(
                name: "IX_CompraVehiculoDetalle_IdCompraVehiculo",
                table: "CompraVehiculoDetalle",
                column: "IdCompraVehiculo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompraVehiculoDetalle");

            migrationBuilder.AlterColumn<string>(
                name: "GuiaRemision",
                table: "CompraVehiculo",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "IdCompra",
                table: "CompraVehiculo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GuiaRemision",
                table: "Compra",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompraVehiculo_IdCompra",
                table: "CompraVehiculo",
                column: "IdCompra");

            migrationBuilder.AddForeignKey(
                name: "FK_CompraVehiculo_Compra",
                table: "CompraVehiculo",
                column: "IdCompra",
                principalTable: "Compra",
                principalColumn: "IdCompra",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
