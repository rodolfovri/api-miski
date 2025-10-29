using Microsoft.EntityFrameworkCore;
using Miski.Domain.Entities;

namespace Miski.Infrastructure.Data;

public class MiskiDbContext : DbContext
{
    public MiskiDbContext(DbContextOptions<MiskiDbContext> options) : base(options)
    {
    }

    // DbSets for all entities
    public DbSet<TipoDocumento> TipoDocumentos { get; set; }
    public DbSet<Vehiculo> Vehiculos { get; set; }
    public DbSet<Persona> Personas { get; set; }
    public DbSet<CategoriaPersona> CategoriaPersonas { get; set; }
    public DbSet<PersonaCategoria> PersonaCategorias { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<UsuarioRol> UsuarioRoles { get; set; }
    public DbSet<Ubicacion> Ubicaciones { get; set; }
    public DbSet<PersonaUbicacion> PersonaUbicaciones { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<VariedadProducto> VariedadProductos { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Negociacion> Negociaciones { get; set; }
    public DbSet<Compra> Compras { get; set; }
    public DbSet<Lote> Lotes { get; set; }
    public DbSet<LlegadaPlanta> LlegadasPlanta { get; set; }
    public DbSet<TrackingPersona> TrackingPersonas { get; set; }
    public DbSet<CompraVehiculo> CompraVehiculos { get; set; }
    public DbSet<Modulo> Modulos { get; set; }
    public DbSet<SubModulo> SubModulos { get; set; }
    public DbSet<SubModuloDetalle> SubModuloDetalles { get; set; }
    public DbSet<PermisoRol> PermisoRoles { get; set; }
    public DbSet<UnidadMedida> UnidadMedidas { get; set; }
    public DbSet<CategoriaProducto> CategoriaProductos { get; set; }
    public DbSet<CompraVehiculoDetalle> CompraVehiculoDetalles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Disable cascade delete to avoid multiple cascade paths
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        // TipoDocumento configuration
        modelBuilder.Entity<TipoDocumento>(entity =>
        {
            entity.HasKey(e => e.IdTipoDocumento);
            entity.Property(e => e.Nombre).HasMaxLength(50).IsRequired();
            entity.ToTable("TipoDocumento");
        });

        // Vehiculo configuration
        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(e => e.IdVehiculo);
            entity.Property(e => e.Placa).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Marca).HasMaxLength(50);
            entity.Property(e => e.Modelo).HasMaxLength(50);
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.ToTable("Vehiculo");
        });

        // Persona configuration
        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.IdPersona);
            entity.Property(e => e.NumeroDocumento).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Nombres).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Apellidos).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Telefono).HasMaxLength(15);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Direccion).HasMaxLength(100);
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.FRegistro).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.TipoDocumento)
                .WithMany(p => p.Personas)
                .HasForeignKey(d => d.IdTipoDocumento)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Persona_TipoDocumento");

            entity.ToTable("Persona");
        });

        // CategoriaPersona configuration
        modelBuilder.Entity<CategoriaPersona>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaPersona);
            entity.Property(e => e.Nombre).HasMaxLength(50).IsRequired();
            entity.ToTable("CategoriaPersona");
        });

        // PersonaCategoria configuration
        modelBuilder.Entity<PersonaCategoria>(entity =>
        {
            entity.HasKey(e => e.IdPersonaCategoria);

            entity.HasOne(d => d.Persona)
                .WithMany(p => p.PersonaCategorias)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PersonaCategoria_Persona");

            entity.HasOne(d => d.Categoria)
                .WithMany(p => p.PersonaCategorias)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PersonaCategoria_Categoria");

            entity.ToTable("PersonaCategoria");
        });

        // Rol configuration
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol);
            entity.Property(e => e.Nombre).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(100);
            entity.Property(e => e.TipoPlataforma).HasMaxLength(20);
            entity.ToTable("Rol");
        });

        // Usuario configuration
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);
            entity.Property(e => e.Username).HasMaxLength(50).IsRequired();
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.PasswordHash).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.FRegistro).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.Persona)
                .WithOne(p => p.Usuario)
                .HasForeignKey<Usuario>(d => d.IdPersona)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Usuario_Persona");

            entity.ToTable("Usuario");
        });

        // UsuarioRol configuration
        modelBuilder.Entity<UsuarioRol>(entity =>
        {
            entity.HasKey(e => e.IdUsuarioRol);
            entity.Property(e => e.FRegistro).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.Usuario)
                .WithMany(p => p.UsuarioRoles)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_UsuarioRol_Usuario");

            entity.HasOne(d => d.Rol)
                .WithMany(p => p.UsuarioRoles)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_UsuarioRol_Rol");

            entity.ToTable("UsuarioRol");
        });

        // Ubicacion configuration
        modelBuilder.Entity<Ubicacion>(entity =>
        {
            entity.HasKey(e => e.IdUbicacion);
            entity.Property(e => e.CodigoSenasa).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(e => e.RazonSocial).HasMaxLength(150).IsRequired();
            entity.Property(e => e.NumeroRuc).HasMaxLength(20);
            entity.Property(e => e.Direccion).HasMaxLength(100).IsRequired();
            entity.Property(e => e.DomicilioLegal).HasMaxLength(100);
            entity.Property(e => e.GiroEstablecimiento).HasMaxLength(100);
            entity.Property(e => e.ComprobantePdf).HasMaxLength(255);
            entity.Property(e => e.Tipo).HasMaxLength(50);
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.FRegistro).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.Usuario)
                .WithMany()
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Ubicacion_Usuario");

            entity.ToTable("Ubicacion");
        });

        // PersonaUbicacion configuration
        modelBuilder.Entity<PersonaUbicacion>(entity =>
        {
            entity.HasKey(e => e.IdPersonaUbicacion);
            entity.Property(e => e.FRegistro).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.Estado).HasMaxLength(20);

            entity.HasOne(d => d.Persona)
                .WithMany(p => p.PersonaUbicaciones)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TU_Persona");

            entity.HasOne(d => d.Ubicacion)
                .WithMany(p => p.PersonaUbicaciones)
                .HasForeignKey(d => d.IdUbicacion)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TU_Ubicacion");

            entity.ToTable("PersonaUbicacion");
        });

        // UnidadMedida configuration
        modelBuilder.Entity<UnidadMedida>(entity =>
        {
            entity.HasKey(e => e.IdUnidadMedida);
            entity.Property(e => e.Nombre).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Abreviatura).HasMaxLength(10).IsRequired();
            entity.ToTable("UnidadMedida");
        });

        //CategoriaProducto configuration
        modelBuilder.Entity<CategoriaProducto>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaProducto);
            entity.Property(e => e.Nombre).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.FRegistro).HasDefaultValueSql("GETDATE()");
            entity.ToTable("CategoriaProducto");
        });

        // Producto configuration
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto);
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.FRegistro).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.Imagen).HasMaxLength(255);
            entity.Property(e => e.FichaTecnica).HasMaxLength(255);

            entity.HasOne(d => d.CategoriaProducto)
                .WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoriaProducto)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Producto_CategoriaProducto");

            entity.ToTable("Producto");
        });

        // TipoCalidadProducto configuration
        modelBuilder.Entity<TipoCalidadProducto>(entity =>
        {
            entity.HasKey(e => e.IdTipoCalidadProducto);
            entity.Property(e => e.Nombre).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.HasOne(d => d.Producto)
                .WithMany(p => p.TipoCalidadProductos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TipoCalidadProducto_Producto");
            entity.ToTable("TipoCalidadProducto");
        });

        // VariedadProducto configuration
        modelBuilder.Entity<VariedadProducto>(entity =>
        {
            entity.HasKey(e => e.IdVariedadProducto);
            entity.Property(e => e.Codigo).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(150);
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.FRegistro).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.FichaTecnica).HasMaxLength(255);
            entity.HasOne(d => d.Producto)
                .WithMany(p => p.VariedadProductos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_VariedadProducto_Producto");
            entity.HasOne(d => d.UnidadMedida)
                .WithMany(p => p.VariedadProductos)
                .HasForeignKey(d => d.IdUnidadMedida)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_VariedadProducto_UnidadMedida");
            entity.ToTable("VariedadProducto");
        });


        // Stock configuration
        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.IdStock);
            entity.Property(e => e.CantidadKg).HasPrecision(18, 2);

            entity.HasOne(d => d.Producto)
                .WithMany(p => p.Stocks)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Stock_Producto");

            entity.HasOne(d => d.Planta)
                .WithMany(p => p.Stocks)
                .HasForeignKey(d => d.IdPlanta)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Stock_Ubicacion");

            entity.ToTable("Stock");
        });

        // Negociacion configuration
        modelBuilder.Entity<Negociacion>(entity =>
        {
            entity.HasKey(e => e.IdNegociacion);
            entity.Property(e => e.NroDocumentoProveedor).HasMaxLength(20);
            entity.Property(e => e.TipoCalidad).HasMaxLength(20);
            entity.Property(e => e.PrecioUnitario).HasPrecision(18, 2).IsRequired();
            entity.Property(e => e.PesoPorSaco).HasPrecision(18, 2);
            entity.Property(e => e.PesoTotal).HasPrecision(18, 2);
            entity.Property(e => e.MontoAdelanto).HasPrecision(18, 2);
            entity.Property(e => e.MontoTotalPago).HasPrecision(18, 2);
            entity.Property(e => e.NroCuentaBancaria).HasMaxLength(20);
            entity.Property(e => e.PrimeraEvidenciaFoto).HasMaxLength(255);
            entity.Property(e => e.SegundaEvidenciaFoto).HasMaxLength(255);
            entity.Property(e => e.TerceraEvidenciaFoto).HasMaxLength(255);
            entity.Property(e => e.EvidenciaVideo).HasMaxLength(255);
            entity.Property(e => e.FotoDniFrontal).HasMaxLength(255);
            entity.Property(e => e.FotoDniPosterior).HasMaxLength(255);
            entity.Property(e => e.EstadoAprobacionIngeniero).HasMaxLength(20);
            entity.Property(e => e.EstadoAprobacionContadora).HasMaxLength(20);
            entity.Property(e => e.Observacion).HasMaxLength(255);
            entity.Property(e => e.Estado).HasMaxLength(20);

            entity.HasOne(d => d.Comisionista)
                .WithMany(p => p.NegociacionesComisionista)
                .HasForeignKey(d => d.IdComisionista)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Negociacion_Comisionista");

            entity.HasOne(d => d.AprobadaPorUsuarioIngeniero)
                .WithMany()
                .HasForeignKey(d => d.AprobadaPorIngeniero)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Negociacion_AprobadaPorIngeniero");

            entity.HasOne(d => d.AprobadaPorUsuarioContadora)
                .WithMany()
                .HasForeignKey(d => d.AprobadaPorContadora)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Negociacion_AprobadaPorContadora");

            entity.HasOne(d => d.RechazadoPorUsuarioIngeniero)
                .WithMany()
                .HasForeignKey(d => d.RechazadoPorIngeniero)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Negociacion_RechazadoPorIngeniero");

            entity.HasOne(d => d.RechazadoPorUsuarioContadora)
                .WithMany()
                .HasForeignKey(d => d.RechazadoPorContadora)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Negociacion_RechazadoPorContadora");

            entity.HasOne(d => d.VariedadProducto)
                .WithMany(p => p.Negociaciones)
                .HasForeignKey(d => d.IdVariedadProducto)  
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Negociacion_VariedadProducto");

            entity.HasOne(d => d.TipoDocumento)
                .WithMany(p => p.Negociaciones)
                .HasForeignKey(d => d.IdTipoDocumento)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Negociacion_TipoDocumento");

            entity.HasOne(d => d.Banco)
                .WithMany(p => p.Negociaciones)
                .HasForeignKey(d => d.IdBanco)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Negociacion_Banco");

            entity.ToTable("Negociacion");
        });

        // Compra configuration
        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra);
            entity.Property(e => e.Serie).HasMaxLength(20);
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.MontoTotal).HasPrecision(18, 2);
            entity.Property(e => e.IGV).HasPrecision(18, 2);
            entity.Property(e => e.Observacion).HasMaxLength(200);

            entity.HasOne(d => d.Negociacion)
                .WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdNegociacion)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Compra_Negociacion");

            entity.HasOne(d => d.Moneda)
                .WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdMoneda)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Compra_Moneda");
            entity.HasOne(d => d.TipoCambio);

            entity.HasOne(d => d.TipoCambio)
                .WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdTipoCambio)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Compra_TipoCambio");

            entity.ToTable("Compra");
        });

        // CompraVehiculo configuration
        modelBuilder.Entity<CompraVehiculo>(entity =>
        {
            entity.HasKey(e => e.IdCompraVehiculo);
            entity.Property(e => e.GuiaRemision).HasMaxLength(50).IsRequired();
            entity.Property(e => e.FRegistro).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.Vehiculo)
                .WithMany(p => p.CompraVehiculos)
                .HasForeignKey(d => d.IdVehiculo)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CompraVehiculo_Vehiculo");
            entity.ToTable("CompraVehiculo");
        });

        // CompraVehiculoDetalle configuration
        modelBuilder.Entity<CompraVehiculoDetalle>(entity =>
        {
            entity.HasKey(e => e.IdCompraVehiculoDetalle);
            entity.HasOne(d => d.CompraVehiculo)
                .WithMany(p => p.CompraVehiculoDetalles)
                .HasForeignKey(d => d.IdCompraVehiculo)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CompraVehiculoDetalle_CompraVehiculo");
            entity.HasOne(d => d.Compra)
                .WithMany(p => p.CompraVehiculoDetalles)
                .HasForeignKey(d => d.IdCompra)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CompraVehiculoDetalle_Compra");
            entity.ToTable("CompraVehiculoDetalle");
        });

        // Lote configuration
        modelBuilder.Entity<Lote>(entity =>
        {
            entity.HasKey(e => e.IdLote);
            entity.Property(e => e.Peso).HasPrecision(18, 2).IsRequired();
            entity.Property(e => e.Sacos).IsRequired();
            entity.Property(e => e.Codigo).HasMaxLength(50);

            entity.HasOne(d => d.Compra)
                .WithMany(p => p.Lotes)
                .HasForeignKey(d => d.IdCompra)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Lote_Compra");

            entity.ToTable("Lote");
        });

        // LlegadaPlanta configuration
        modelBuilder.Entity<LlegadaPlanta>(entity =>
        {
            entity.HasKey(e => e.IdLlegadaPlanta);
            entity.Property(e => e.Observaciones).HasMaxLength(255);
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.FLlegada).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.SacosRecibidos).HasPrecision(18, 2).IsRequired();
            entity.Property(e => e.PesoRecibido).HasPrecision(18, 2).IsRequired();

            entity.HasOne(d => d.Compra)
                .WithMany(p => p.LlegadasPlanta)
                .HasForeignKey(d => d.IdCompra)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_LlegadaPlanta_Compra");

            entity.HasOne(d => d.Usuario)
                .WithMany(p => p.LlegadasPlanta)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_LlegadaPlanta_Ingeniero");

            entity.HasOne(d => d.Lote)
                .WithMany(p => p.LlegadasPlanta)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_LlegadaPlanta_Lote");

            entity.ToTable("LlegadaPlanta");
        });

        // TrackingPersona configuration
        modelBuilder.Entity<TrackingPersona>(entity =>
        {
            entity.HasKey(e => e.IdTracking);
            entity.Property(e => e.Latitud).HasPrecision(10, 7).IsRequired();
            entity.Property(e => e.Longitud).HasPrecision(10, 7).IsRequired();
            entity.Property(e => e.FRegistro).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.Persona)
                .WithMany(p => p.TrackingPersonas)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TrackingPersona_Persona");
            entity.ToTable("TrackingPersona");
        });

        // Modulo configuration
        modelBuilder.Entity<Modulo>(entity =>
        {
            entity.HasKey(e => e.IdModulo);
            entity.Property(e => e.Nombre).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Orden).IsRequired();
            entity.Property(e => e.Estado).HasMaxLength(20).IsRequired();
            entity.Property(e => e.TipoPlataforma).HasMaxLength(10).IsRequired();
            entity.ToTable("Modulo");
        });

        // SubModulo configuration
        modelBuilder.Entity<SubModulo>(entity =>
        {
            entity.HasKey(e => e.IdSubModulo);
            entity.Property(e => e.Nombre).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Orden).IsRequired();
            entity.Property(e => e.Estado).HasMaxLength(20).IsRequired();
            entity.HasOne(d => d.Modulo)
                .WithMany(p => p.SubModulos)
                .HasForeignKey(d => d.IdModulo)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_SubModulo_Modulo");
            entity.ToTable("SubModulo");
        });

        // SubModuloDetalle configuration
        modelBuilder.Entity<SubModuloDetalle>(entity =>
        {
            entity.HasKey(e => e.IdSubModuloDetalle);
            entity.Property(e => e.Nombre).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Orden).IsRequired();
            entity.Property(e => e.Estado).HasMaxLength(20).IsRequired();
            entity.HasOne(d => d.SubModulo)
                .WithMany(p => p.SubModuloDetalles)
                .HasForeignKey(d => d.IdSubModulo)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_SubModuloDetalle_SubModulo");
            entity.ToTable("SubModuloDetalle");
        });

        // PermisoRol configuration
        modelBuilder.Entity<PermisoRol>(entity =>
        {
            entity.HasKey(e => e.IdPermisoRol);
            entity.Property(e => e.TieneAcceso);
            entity.HasOne(d => d.Rol)
                .WithMany(p => p.PermisoRoles)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PermisoRol_Rol");
            entity.HasOne(d => d.Modulo)
                .WithMany(p => p.PermisoRoles)
                .HasForeignKey(d => d.IdModulo)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PermisoRol_Modulo");
            entity.HasOne(d => d.SubModulo)
                .WithMany(p => p.PermisoRoles)
                .HasForeignKey(d => d.IdSubModulo)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PermisoRol_SubModulo");
            entity.HasOne(d => d.SubModuloDetalle)
                .WithMany(p => p.PermisoRoles)
                .HasForeignKey(d => d.IdSubModuloDetalle)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PermisoRol_SubModuloDetalle");
            entity.ToTable("PermisoRol");
        });

        // Banco configuration
        modelBuilder.Entity<Banco>(entity =>
        {
            entity.HasKey(e => e.IdBanco);
            entity.Property(e => e.Nombre).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.ToTable("Banco");
        });

        // Moneda configuration
        modelBuilder.Entity<Moneda>(entity =>
        {
            entity.HasKey(e => e.IdMoneda);
            entity.Property(e => e.Nombre).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Simbolo).HasMaxLength(10).IsRequired();
            entity.Property(e => e.Codigo).HasMaxLength(10).IsRequired();
            entity.ToTable("Moneda");
        });

        // TipoCambio configuration
        modelBuilder.Entity<TipoCambio>(entity =>
        {
            entity.HasKey(e => e.IdTipoCambio);
            entity.Property(e => e.ValorCompra).HasPrecision(18, 4).IsRequired();
            entity.Property(e => e.ValorVenta).HasPrecision(18, 4).IsRequired();
            entity.Property(e => e.FRegistro).HasDefaultValueSql("GETDATE()");
            entity.HasOne(d => d.Moneda)
                .WithMany(p => p.TipoCambios)
                .HasForeignKey(d => d.IdMoneda)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TipoCambio_Moneda");
            entity.HasOne(d => d.Usuario)
                .WithMany(p => p.TipoCambios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_TipoCambio_Usuario");
            entity.ToTable("TipoCambio");
        });
    }
}