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
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Negociacion> Negociaciones { get; set; }
    public DbSet<Compra> Compras { get; set; }
    public DbSet<Lote> Lotes { get; set; }
    public DbSet<LlegadaPlanta> LlegadasPlanta { get; set; }
    public DbSet<LlegadaPlantaDetalle> LlegadaPlantaDetalles { get; set; }
    public DbSet<TrackingPersona> TrackingPersonas { get; set; }
    public DbSet<CompraVehiculo> CompraVehiculos { get; set; }
    public DbSet<Modulo> Modulos { get; set; }
    public DbSet<SubModulo> SubModulos { get; set; }
    public DbSet<SubModuloDetalle> SubModuloDetalles { get; set; }
    public DbSet<PermisoRol> PermisoRoles { get; set; }

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
            entity.Property(e => e.Licencia).HasMaxLength(20);
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
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Direccion).IsRequired();
            entity.Property(e => e.Latitud).HasPrecision(10, 7);
            entity.Property(e => e.Longitud).HasPrecision(10, 7);
            entity.Property(e => e.Tipo).HasMaxLength(50);
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.FRegistro).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.Persona)
                .WithMany(p => p.Ubicaciones)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Ubicacion_Persona");

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

        // Producto configuration
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto);
            entity.Property(e => e.Codigo).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.UnidadMedida).HasMaxLength(20);
            entity.Property(e => e.PesoPorSaco).HasPrecision(10, 2);
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.FRegistro).HasDefaultValueSql("GETDATE()");
            entity.ToTable("Producto");
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
            entity.Property(e => e.PesoTotal).HasPrecision(18, 2).IsRequired();
            entity.Property(e => e.PrecioUnitario).HasPrecision(18, 2).IsRequired();
            entity.Property(e => e.NroCuentaRuc).HasMaxLength(20).IsRequired();
            entity.Property(e => e.FotoCalidadProducto).HasMaxLength(255).IsRequired();
            entity.Property(e => e.FotoDniFrontal).HasMaxLength(255).IsRequired();
            entity.Property(e => e.FotoDniPosterior).HasMaxLength(255).IsRequired();
            entity.Property(e => e.EstadoAprobado).HasMaxLength(10);
            entity.Property(e => e.Observacion).HasMaxLength(255);
            entity.Property(e => e.Estado).HasMaxLength(20);

            entity.HasOne(d => d.Proveedor)
                .WithMany(p => p.NegociacionesProveedor)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Negociacion_Proveedor");

            entity.HasOne(d => d.Comisionista)
                .WithMany(p => p.NegociacionesComisionista)
                .HasForeignKey(d => d.IdComisionista)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Negociacion_Comisionista");

            entity.HasOne(d => d.AprobadaPorPersona)
                .WithMany(p => p.NegociacionesAprobadas)
                .HasForeignKey(d => d.AprobadaPor)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Negociacion_AprobadaPor");

            entity.HasOne(d => d.Producto)
                .WithMany(p => p.Negociaciones)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Negociacion_Producto");

            entity.ToTable("Negociacion");
        });

        // Compra configuration
        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra);
            entity.Property(e => e.Serie).HasMaxLength(20);
            entity.Property(e => e.GuiaRemision).HasMaxLength(100);
            entity.Property(e => e.Estado).HasMaxLength(20);

            entity.HasOne(d => d.Negociacion)
                .WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdNegociacion)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Compra_Negociacion");

            entity.ToTable("Compra");
        });

        // CompraVehiculo configuration
        modelBuilder.Entity<CompraVehiculo>(entity =>
        {
            entity.HasKey(e => e.IdCompraVehiculo);
            entity.Property(e => e.FRegistro).HasDefaultValueSql("GETDATE()");

            entity.HasOne(d => d.Compra)
                .WithMany(p => p.CompraVehiculos)
                .HasForeignKey(d => d.IdCompra)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CompraVehiculo_Compra");
            entity.HasOne(d => d.Vehiculo)
                .WithMany(p => p.CompraVehiculos)
                .HasForeignKey(d => d.IdVehiculo)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CompraVehiculo_Vehiculo");
            entity.ToTable("CompraVehiculo");
        });

        // Lote configuration
        modelBuilder.Entity<Lote>(entity =>
        {
            entity.HasKey(e => e.IdLote);
            entity.Property(e => e.Peso).HasPrecision(18, 2).IsRequired();
            entity.Property(e => e.Sacos).IsRequired();
            entity.Property(e => e.Codigo).HasMaxLength(50);
            entity.Property(e => e.Grado).HasMaxLength(20);

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

            entity.ToTable("LlegadaPlanta");
        });

        // LlegadaPlantaDetalle configuration
        modelBuilder.Entity<LlegadaPlantaDetalle>(entity =>
        {
            entity.HasKey(e => e.IdLlegadaDetalle);
            entity.Property(e => e.PesoRecibido).HasPrecision(18, 2).IsRequired();
            entity.Property(e => e.Observaciones).HasMaxLength(255);

            entity.HasOne(d => d.LlegadaPlanta)
                .WithMany(p => p.LlegadaPlantaDetalles)
                .HasForeignKey(d => d.IdLlegadaPlanta)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_LlegadaDetalle_Llegada");

            entity.HasOne(d => d.Lote)
                .WithMany(p => p.LlegadaPlantaDetalles)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_LlegadaDetalle_Lote");

            entity.HasOne(d => d.ProductoFinal)
                .WithMany(p => p.LlegadaPlantaDetalles)
                .HasForeignKey(d => d.IdProductoFinal)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_LlegadaDetalle_Producto");

            entity.ToTable("LlegadaPlantaDetalle");
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
    }
}