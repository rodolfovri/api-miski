using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Miski.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banco",
                columns: table => new
                {
                    IdBanco = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banco", x => x.IdBanco);
                });

            migrationBuilder.CreateTable(
                name: "Cargo",
                columns: table => new
                {
                    IdCargo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargo", x => x.IdCargo);
                });

            migrationBuilder.CreateTable(
                name: "CategoriaPersona",
                columns: table => new
                {
                    IdCategoriaPersona = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaPersona", x => x.IdCategoriaPersona);
                });

            migrationBuilder.CreateTable(
                name: "CategoriaProducto",
                columns: table => new
                {
                    IdCategoriaProducto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaProducto", x => x.IdCategoriaProducto);
                });

            migrationBuilder.CreateTable(
                name: "Lote",
                columns: table => new
                {
                    IdLote = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Peso = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Sacos = table.Column<int>(type: "integer", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Comision = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Observacion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lote", x => x.IdLote);
                });

            migrationBuilder.CreateTable(
                name: "Modulo",
                columns: table => new
                {
                    IdModulo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TipoPlataforma = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modulo", x => x.IdModulo);
                });

            migrationBuilder.CreateTable(
                name: "Moneda",
                columns: table => new
                {
                    IdMoneda = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Simbolo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Codigo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moneda", x => x.IdMoneda);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TipoPlataforma = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "TipoDocumento",
                columns: table => new
                {
                    IdTipoDocumento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LongitudMin = table.Column<int>(type: "integer", nullable: true),
                    LongitudMax = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDocumento", x => x.IdTipoDocumento);
                });

            migrationBuilder.CreateTable(
                name: "UnidadMedida",
                columns: table => new
                {
                    IdUnidadMedida = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Abreviatura = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadMedida", x => x.IdUnidadMedida);
                });

            migrationBuilder.CreateTable(
                name: "Vehiculo",
                columns: table => new
                {
                    IdVehiculo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Placa = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Marca = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Modelo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculo", x => x.IdVehiculo);
                });

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    IdProducto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCategoriaProducto = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Imagen = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FichaTecnica = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.IdProducto);
                    table.ForeignKey(
                        name: "FK_Producto_CategoriaProducto",
                        column: x => x.IdCategoriaProducto,
                        principalTable: "CategoriaProducto",
                        principalColumn: "IdCategoriaProducto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubModulo",
                columns: table => new
                {
                    IdSubModulo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdModulo = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
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
                name: "Persona",
                columns: table => new
                {
                    IdPersona = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdTipoDocumento = table.Column<int>(type: "integer", nullable: false),
                    NumeroDocumento = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Nombres = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Direccion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persona", x => x.IdPersona);
                    table.ForeignKey(
                        name: "FK_Persona_TipoDocumento",
                        column: x => x.IdTipoDocumento,
                        principalTable: "TipoDocumento",
                        principalColumn: "IdTipoDocumento",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TipoCalidadProducto",
                columns: table => new
                {
                    IdTipoCalidadProducto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdProducto = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
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

            migrationBuilder.CreateTable(
                name: "VariedadProducto",
                columns: table => new
                {
                    IdVariedadProducto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdProducto = table.Column<int>(type: "integer", nullable: false),
                    IdUnidadMedida = table.Column<int>(type: "integer", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FichaTecnica = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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

            migrationBuilder.CreateTable(
                name: "SubModuloDetalle",
                columns: table => new
                {
                    IdSubModuloDetalle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdSubModulo = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
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
                name: "CompraVehiculo",
                columns: table => new
                {
                    IdCompraVehiculo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPersona = table.Column<int>(type: "integer", nullable: false),
                    IdVehiculo = table.Column<int>(type: "integer", nullable: false),
                    GuiaRemision = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraVehiculo", x => x.IdCompraVehiculo);
                    table.ForeignKey(
                        name: "FK_CompraVehiculo_Persona",
                        column: x => x.IdPersona,
                        principalTable: "Persona",
                        principalColumn: "IdPersona",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompraVehiculo_Vehiculo",
                        column: x => x.IdVehiculo,
                        principalTable: "Vehiculo",
                        principalColumn: "IdVehiculo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonaCargo",
                columns: table => new
                {
                    IdPersonaCargo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPersona = table.Column<int>(type: "integer", nullable: false),
                    IdCargo = table.Column<int>(type: "integer", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EsActual = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ObservacionAsignacion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    MotivoRevocacion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonaCargo", x => x.IdPersonaCargo);
                    table.ForeignKey(
                        name: "FK_PersonaCargo_Cargo",
                        column: x => x.IdCargo,
                        principalTable: "Cargo",
                        principalColumn: "IdCargo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonaCargo_Persona",
                        column: x => x.IdPersona,
                        principalTable: "Persona",
                        principalColumn: "IdPersona",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonaCategoria",
                columns: table => new
                {
                    IdPersonaCategoria = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPersona = table.Column<int>(type: "integer", nullable: false),
                    IdCategoria = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonaCategoria", x => x.IdPersonaCategoria);
                    table.ForeignKey(
                        name: "FK_PersonaCategoria_Categoria",
                        column: x => x.IdCategoria,
                        principalTable: "CategoriaPersona",
                        principalColumn: "IdCategoriaPersona",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonaCategoria_Persona",
                        column: x => x.IdPersona,
                        principalTable: "Persona",
                        principalColumn: "IdPersona",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrackingPersona",
                columns: table => new
                {
                    IdTracking = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPersona = table.Column<int>(type: "integer", nullable: false),
                    Latitud = table.Column<string>(type: "text", precision: 10, scale: 7, nullable: false),
                    Longitud = table.Column<string>(type: "text", precision: 10, scale: 7, nullable: false),
                    FRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingPersona", x => x.IdTracking);
                    table.ForeignKey(
                        name: "FK_TrackingPersona_Persona",
                        column: x => x.IdPersona,
                        principalTable: "Persona",
                        principalColumn: "IdPersona",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPersona = table.Column<int>(type: "integer", nullable: true),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", maxLength: 255, nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuario_Persona",
                        column: x => x.IdPersona,
                        principalTable: "Persona",
                        principalColumn: "IdPersona",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermisoRol",
                columns: table => new
                {
                    IdPermisoRol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdRol = table.Column<int>(type: "integer", nullable: false),
                    IdModulo = table.Column<int>(type: "integer", nullable: true),
                    IdSubModulo = table.Column<int>(type: "integer", nullable: true),
                    IdSubModuloDetalle = table.Column<int>(type: "integer", nullable: true),
                    TieneAcceso = table.Column<bool>(type: "boolean", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Negociacion",
                columns: table => new
                {
                    IdNegociacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdComisionista = table.Column<int>(type: "integer", nullable: false),
                    IdTipoDocumento = table.Column<int>(type: "integer", nullable: true),
                    IdBanco = table.Column<int>(type: "integer", nullable: true),
                    NroDocumentoProveedor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IdVariedadProducto = table.Column<int>(type: "integer", nullable: true),
                    TipoCalidad = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SacosTotales = table.Column<int>(type: "integer", nullable: true),
                    PesoPorSaco = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    PesoTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MontoAdelanto = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    FAdelanto = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MontoTotalPago = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    FPagoTotal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NroCuentaBancaria = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FotoDniFrontal = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FotoDniPosterior = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PrimeraEvidenciaFoto = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SegundaEvidenciaFoto = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    TerceraEvidenciaFoto = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    EvidenciaVideo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    EstadoAprobacionIngeniero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    EstadoAprobacionContadora = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    AprobadaPorIngeniero = table.Column<int>(type: "integer", nullable: true),
                    AprobadaPorContadora = table.Column<int>(type: "integer", nullable: true),
                    RechazadoPorIngeniero = table.Column<int>(type: "integer", nullable: true),
                    RechazadoPorContadora = table.Column<int>(type: "integer", nullable: true),
                    IdUsuarioAnulacion = table.Column<int>(type: "integer", nullable: true),
                    MotivoAnulacion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    FAnulacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FAprobacionIngeniero = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FAprobacionContadora = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FRechazoIngeniero = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FRechazoContadora = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Observacion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ProveedorIdPersona = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Negociacion", x => x.IdNegociacion);
                    table.ForeignKey(
                        name: "FK_Negociacion_AprobadaPorContadora",
                        column: x => x.AprobadaPorContadora,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Negociacion_AprobadaPorIngeniero",
                        column: x => x.AprobadaPorIngeniero,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Negociacion_Banco",
                        column: x => x.IdBanco,
                        principalTable: "Banco",
                        principalColumn: "IdBanco",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Negociacion_Comisionista",
                        column: x => x.IdComisionista,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Negociacion_Persona_ProveedorIdPersona",
                        column: x => x.ProveedorIdPersona,
                        principalTable: "Persona",
                        principalColumn: "IdPersona",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Negociacion_RechazadoPorContadora",
                        column: x => x.RechazadoPorContadora,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Negociacion_RechazadoPorIngeniero",
                        column: x => x.RechazadoPorIngeniero,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Negociacion_TipoDocumento",
                        column: x => x.IdTipoDocumento,
                        principalTable: "TipoDocumento",
                        principalColumn: "IdTipoDocumento",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Negociacion_UsuarioAnulacion",
                        column: x => x.IdUsuarioAnulacion,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Negociacion_VariedadProducto",
                        column: x => x.IdVariedadProducto,
                        principalTable: "VariedadProducto",
                        principalColumn: "IdVariedadProducto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TipoCambio",
                columns: table => new
                {
                    IdTipoCambio = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdMoneda = table.Column<int>(type: "integer", nullable: false),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    ValorCompra = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    ValorVenta = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    FRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCambio", x => x.IdTipoCambio);
                    table.ForeignKey(
                        name: "FK_TipoCambio_Moneda",
                        column: x => x.IdMoneda,
                        principalTable: "Moneda",
                        principalColumn: "IdMoneda",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TipoCambio_Usuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ubicacion",
                columns: table => new
                {
                    IdUbicacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUsuario = table.Column<int>(type: "integer", nullable: true),
                    CodigoSenasa = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RazonSocial = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    NumeroRuc = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Direccion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DomicilioLegal = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    GiroEstablecimiento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ComprobantePdf = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ubicacion", x => x.IdUbicacion);
                    table.ForeignKey(
                        name: "FK_Ubicacion_Usuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRol",
                columns: table => new
                {
                    IdUsuarioRol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    IdRol = table.Column<int>(type: "integer", nullable: false),
                    FRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRol", x => x.IdUsuarioRol);
                    table.ForeignKey(
                        name: "FK_UsuarioRol_Rol",
                        column: x => x.IdRol,
                        principalTable: "Rol",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuarioRol_Usuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Compra",
                columns: table => new
                {
                    IdCompra = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdNegociacion = table.Column<int>(type: "integer", nullable: false),
                    IdLote = table.Column<int>(type: "integer", nullable: true),
                    IdMoneda = table.Column<int>(type: "integer", nullable: false),
                    IdTipoCambio = table.Column<int>(type: "integer", nullable: true),
                    Serie = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Correlativo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FEmision = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MontoTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    IGV = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    Observacion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    EstadoRecepcion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    EsParcial = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    IdUsuarioAnulacion = table.Column<int>(type: "integer", nullable: true),
                    MotivoAnulacion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    FAnulacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compra", x => x.IdCompra);
                    table.ForeignKey(
                        name: "FK_Compra_Lote",
                        column: x => x.IdLote,
                        principalTable: "Lote",
                        principalColumn: "IdLote",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compra_Moneda",
                        column: x => x.IdMoneda,
                        principalTable: "Moneda",
                        principalColumn: "IdMoneda",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compra_Negociacion",
                        column: x => x.IdNegociacion,
                        principalTable: "Negociacion",
                        principalColumn: "IdNegociacion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compra_TipoCambio",
                        column: x => x.IdTipoCambio,
                        principalTable: "TipoCambio",
                        principalColumn: "IdTipoCambio",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compra_UsuarioAnulacion",
                        column: x => x.IdUsuarioAnulacion,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonaUbicacion",
                columns: table => new
                {
                    IdPersonaUbicacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdPersona = table.Column<int>(type: "integer", nullable: false),
                    IdUbicacion = table.Column<int>(type: "integer", nullable: false),
                    FRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonaUbicacion", x => x.IdPersonaUbicacion);
                    table.ForeignKey(
                        name: "FK_TU_Persona",
                        column: x => x.IdPersona,
                        principalTable: "Persona",
                        principalColumn: "IdPersona",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TU_Ubicacion",
                        column: x => x.IdUbicacion,
                        principalTable: "Ubicacion",
                        principalColumn: "IdUbicacion",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    IdStock = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdVariedadProducto = table.Column<int>(type: "integer", nullable: false),
                    IdPlanta = table.Column<int>(type: "integer", nullable: false),
                    CantidadKg = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.IdStock);
                    table.ForeignKey(
                        name: "FK_Stock_Ubicacion",
                        column: x => x.IdPlanta,
                        principalTable: "Ubicacion",
                        principalColumn: "IdUbicacion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stock_VariedadProducto",
                        column: x => x.IdVariedadProducto,
                        principalTable: "VariedadProducto",
                        principalColumn: "IdVariedadProducto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompraVehiculoDetalle",
                columns: table => new
                {
                    IdCompraVehiculoDetalle = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCompraVehiculo = table.Column<int>(type: "integer", nullable: false),
                    IdCompra = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "LlegadaPlanta",
                columns: table => new
                {
                    IdLlegadaPlanta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCompra = table.Column<int>(type: "integer", nullable: false),
                    IdUsuario = table.Column<int>(type: "integer", nullable: false),
                    IdLote = table.Column<int>(type: "integer", nullable: false),
                    IdUbicacion = table.Column<int>(type: "integer", nullable: false),
                    SacosRecibidos = table.Column<double>(type: "double precision", precision: 18, scale: 2, nullable: false),
                    PesoRecibido = table.Column<double>(type: "double precision", precision: 18, scale: 2, nullable: false),
                    FLlegada = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Observaciones = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LlegadaPlanta", x => x.IdLlegadaPlanta);
                    table.ForeignKey(
                        name: "FK_LlegadaPlanta_Compra",
                        column: x => x.IdCompra,
                        principalTable: "Compra",
                        principalColumn: "IdCompra",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LlegadaPlanta_Ingeniero",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LlegadaPlanta_Lote",
                        column: x => x.IdLote,
                        principalTable: "Lote",
                        principalColumn: "IdLote",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LlegadaPlanta_Ubicacion",
                        column: x => x.IdUbicacion,
                        principalTable: "Ubicacion",
                        principalColumn: "IdUbicacion",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compra_IdLote",
                table: "Compra",
                column: "IdLote",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Compra_IdMoneda",
                table: "Compra",
                column: "IdMoneda");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_IdNegociacion",
                table: "Compra",
                column: "IdNegociacion");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_IdTipoCambio",
                table: "Compra",
                column: "IdTipoCambio");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_IdUsuarioAnulacion",
                table: "Compra",
                column: "IdUsuarioAnulacion");

            migrationBuilder.CreateIndex(
                name: "IX_CompraVehiculo_IdPersona",
                table: "CompraVehiculo",
                column: "IdPersona");

            migrationBuilder.CreateIndex(
                name: "IX_CompraVehiculo_IdVehiculo",
                table: "CompraVehiculo",
                column: "IdVehiculo");

            migrationBuilder.CreateIndex(
                name: "IX_CompraVehiculoDetalle_IdCompra",
                table: "CompraVehiculoDetalle",
                column: "IdCompra");

            migrationBuilder.CreateIndex(
                name: "IX_CompraVehiculoDetalle_IdCompraVehiculo",
                table: "CompraVehiculoDetalle",
                column: "IdCompraVehiculo");

            migrationBuilder.CreateIndex(
                name: "IX_LlegadaPlanta_IdCompra",
                table: "LlegadaPlanta",
                column: "IdCompra");

            migrationBuilder.CreateIndex(
                name: "IX_LlegadaPlanta_IdLote",
                table: "LlegadaPlanta",
                column: "IdLote");

            migrationBuilder.CreateIndex(
                name: "IX_LlegadaPlanta_IdUbicacion",
                table: "LlegadaPlanta",
                column: "IdUbicacion");

            migrationBuilder.CreateIndex(
                name: "IX_LlegadaPlanta_IdUsuario",
                table: "LlegadaPlanta",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_AprobadaPorContadora",
                table: "Negociacion",
                column: "AprobadaPorContadora");

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_AprobadaPorIngeniero",
                table: "Negociacion",
                column: "AprobadaPorIngeniero");

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_IdBanco",
                table: "Negociacion",
                column: "IdBanco");

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_IdComisionista",
                table: "Negociacion",
                column: "IdComisionista");

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_IdTipoDocumento",
                table: "Negociacion",
                column: "IdTipoDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_IdUsuarioAnulacion",
                table: "Negociacion",
                column: "IdUsuarioAnulacion");

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_IdVariedadProducto",
                table: "Negociacion",
                column: "IdVariedadProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_ProveedorIdPersona",
                table: "Negociacion",
                column: "ProveedorIdPersona");

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_RechazadoPorContadora",
                table: "Negociacion",
                column: "RechazadoPorContadora");

            migrationBuilder.CreateIndex(
                name: "IX_Negociacion_RechazadoPorIngeniero",
                table: "Negociacion",
                column: "RechazadoPorIngeniero");

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
                name: "IX_Persona_IdTipoDocumento",
                table: "Persona",
                column: "IdTipoDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_PersonaCargo_IdCargo",
                table: "PersonaCargo",
                column: "IdCargo");

            migrationBuilder.CreateIndex(
                name: "IX_PersonaCargo_IdPersona",
                table: "PersonaCargo",
                column: "IdPersona");

            migrationBuilder.CreateIndex(
                name: "IX_PersonaCategoria_IdCategoria",
                table: "PersonaCategoria",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_PersonaCategoria_IdPersona",
                table: "PersonaCategoria",
                column: "IdPersona");

            migrationBuilder.CreateIndex(
                name: "IX_PersonaUbicacion_IdPersona",
                table: "PersonaUbicacion",
                column: "IdPersona");

            migrationBuilder.CreateIndex(
                name: "IX_PersonaUbicacion_IdUbicacion",
                table: "PersonaUbicacion",
                column: "IdUbicacion");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_IdCategoriaProducto",
                table: "Producto",
                column: "IdCategoriaProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_IdPlanta",
                table: "Stock",
                column: "IdPlanta");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_IdVariedadProducto",
                table: "Stock",
                column: "IdVariedadProducto");

            migrationBuilder.CreateIndex(
                name: "IX_SubModulo_IdModulo",
                table: "SubModulo",
                column: "IdModulo");

            migrationBuilder.CreateIndex(
                name: "IX_SubModuloDetalle_IdSubModulo",
                table: "SubModuloDetalle",
                column: "IdSubModulo");

            migrationBuilder.CreateIndex(
                name: "IX_TipoCalidadProducto_IdProducto",
                table: "TipoCalidadProducto",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_TipoCambio_IdMoneda",
                table: "TipoCambio",
                column: "IdMoneda");

            migrationBuilder.CreateIndex(
                name: "IX_TipoCambio_IdUsuario",
                table: "TipoCambio",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingPersona_IdPersona",
                table: "TrackingPersona",
                column: "IdPersona");

            migrationBuilder.CreateIndex(
                name: "IX_Ubicacion_IdUsuario",
                table: "Ubicacion",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_IdPersona",
                table: "Usuario",
                column: "IdPersona",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Username",
                table: "Usuario",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRol_IdRol",
                table: "UsuarioRol",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRol_IdUsuario",
                table: "UsuarioRol",
                column: "IdUsuario");

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
                name: "CompraVehiculoDetalle");

            migrationBuilder.DropTable(
                name: "LlegadaPlanta");

            migrationBuilder.DropTable(
                name: "PermisoRol");

            migrationBuilder.DropTable(
                name: "PersonaCargo");

            migrationBuilder.DropTable(
                name: "PersonaCategoria");

            migrationBuilder.DropTable(
                name: "PersonaUbicacion");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "TipoCalidadProducto");

            migrationBuilder.DropTable(
                name: "TrackingPersona");

            migrationBuilder.DropTable(
                name: "UsuarioRol");

            migrationBuilder.DropTable(
                name: "CompraVehiculo");

            migrationBuilder.DropTable(
                name: "Compra");

            migrationBuilder.DropTable(
                name: "SubModuloDetalle");

            migrationBuilder.DropTable(
                name: "Cargo");

            migrationBuilder.DropTable(
                name: "CategoriaPersona");

            migrationBuilder.DropTable(
                name: "Ubicacion");

            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.DropTable(
                name: "Vehiculo");

            migrationBuilder.DropTable(
                name: "Lote");

            migrationBuilder.DropTable(
                name: "Negociacion");

            migrationBuilder.DropTable(
                name: "TipoCambio");

            migrationBuilder.DropTable(
                name: "SubModulo");

            migrationBuilder.DropTable(
                name: "Banco");

            migrationBuilder.DropTable(
                name: "VariedadProducto");

            migrationBuilder.DropTable(
                name: "Moneda");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Modulo");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "UnidadMedida");

            migrationBuilder.DropTable(
                name: "Persona");

            migrationBuilder.DropTable(
                name: "CategoriaProducto");

            migrationBuilder.DropTable(
                name: "TipoDocumento");
        }
    }
}
