using AutoMapper;
using Miski.Domain.Entities;
using Miski.Shared.DTOs;
using Miski.Shared.DTOs.Auth;
using Miski.Shared.DTOs.Permisos;
using Miski.Shared.DTOs.Personas;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.DTOs.Ubicaciones;
using Miski.Shared.DTOs.Almacen;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.DTOs.Usuarios;

namespace Miski.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Producto, ProductoDto>()
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado ?? "ACTIVO"));

        CreateMap<Persona, PersonaDto>()
            .ForMember(dest => dest.TipoDocumentoNombre, opt => opt.MapFrom(src => src.TipoDocumento.Nombre))
            .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => $"{src.Nombres} {src.Apellidos}"));

        // Mapeos para Auth
        CreateMap<Persona, AuthPersonaDto>()
            .ForMember(dest => dest.TipoDocumentoNombre, opt => opt.MapFrom(src =>
                src.TipoDocumento != null ? src.TipoDocumento.Nombre : string.Empty))
            .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => $"{src.Nombres} {src.Apellidos}"));

        CreateMap<Rol, Miski.Shared.DTOs.Maestros.RolMaestroDto>()
            .ForMember(dest => dest.TipoPlataforma, opt => opt.MapFrom(src => src.TipoPlataforma));

        CreateMap<Usuario, AuthResponseDto>()
            .ForMember(dest => dest.Token, opt => opt.Ignore())
            .ForMember(dest => dest.Expiration, opt => opt.Ignore())
            .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Se manejará manualmente

        // Mapeos para Permisos
        CreateMap<Modulo, ModuloDto>();
        CreateMap<SubModulo, SubModuloDto>()
            .ForMember(dest => dest.ModuloNombre, opt => opt.MapFrom(src => src.Modulo.Nombre));
        CreateMap<SubModuloDetalle, SubModuloDetalleDto>()
            .ForMember(dest => dest.SubModuloNombre, opt => opt.MapFrom(src => src.SubModulo.Nombre));

        // Mapeo para Accion
        CreateMap<Accion, AccionDto>();

        CreateMap<PermisoRol, PermisoRolDto>()
            .ForMember(dest => dest.RolNombre, opt => opt.MapFrom(src => src.Rol != null ? src.Rol.Nombre : null))
            .ForMember(dest => dest.ModuloNombre, opt => opt.MapFrom(src => src.Modulo != null ? src.Modulo.Nombre : null))
            .ForMember(dest => dest.SubModuloNombre, opt => opt.MapFrom(src => src.SubModulo != null ? src.SubModulo.Nombre : null))
            .ForMember(dest => dest.SubModuloDetalleNombre, opt => opt.MapFrom(src => src.SubModuloDetalle != null ? src.SubModuloDetalle.Nombre : null))
            .ForMember(dest => dest.Acciones, opt => opt.Ignore()); // Se maneja manualmente en el handler

        // Mapeo para PermisoRolAccion a AccionPermisoDto
        CreateMap<PermisoRolAccion, AccionPermisoDto>()
            .ForMember(dest => dest.IdAccion, opt => opt.MapFrom(src => src.Accion.IdAccion))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Accion.Nombre))
            .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Accion.Codigo))
            .ForMember(dest => dest.Icono, opt => opt.MapFrom(src => src.Accion.Icono))
            .ForMember(dest => dest.Habilitado, opt => opt.MapFrom(src => src.Habilitado));

        // Mapeos para SubModuloAccion
        CreateMap<SubModuloAccion, SubModuloAccionDto>()
            .ForMember(dest => dest.SubModuloNombre, opt => opt.MapFrom(src => 
                src.SubModulo != null ? src.SubModulo.Nombre : null))
            .ForMember(dest => dest.AccionNombre, opt => opt.MapFrom(src => 
                src.Accion != null ? src.Accion.Nombre : null))
            .ForMember(dest => dest.AccionCodigo, opt => opt.MapFrom(src => 
                src.Accion != null ? src.Accion.Codigo : null));

        // Mapeos para SubModuloDetalleAccion
        CreateMap<SubModuloDetalleAccion, SubModuloDetalleAccionDto>()
            .ForMember(dest => dest.SubModuloDetalleNombre, opt => opt.MapFrom(src => 
                src.SubModuloDetalle != null ? src.SubModuloDetalle.Nombre : null))
            .ForMember(dest => dest.AccionNombre, opt => opt.MapFrom(src => 
                src.Accion != null ? src.Accion.Nombre : null))
            .ForMember(dest => dest.AccionCodigo, opt => opt.MapFrom(src => 
                src.Accion != null ? src.Accion.Codigo : null));

        // Mapeos para Personas
        CreateMap<Persona, Miski.Shared.DTOs.Personas.PersonaDto>()
            .ForMember(dest => dest.TipoDocumentoNombre, opt => opt.MapFrom(src =>
                src.TipoDocumento != null ? src.TipoDocumento.Nombre : string.Empty))
            .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => $"{src.Nombres} {src.Apellidos}"))
            .ForMember(dest => dest.Categorias, opt => opt.Ignore()); // Se manejará manualmente si es necesario

        CreateMap<CategoriaPersona, CategoriaPersonaDto>();

        // Mapeos para Maestros - TipoDocumento
        CreateMap<TipoDocumento, TipoDocumentoDto>();

        // Mapeos para Maestros - UnidadMedida
        CreateMap<UnidadMedida, UnidadMedidaDto>();

        // Mapeos para Maestros - CategoriaProducto
        CreateMap<CategoriaProducto, CategoriaProductoDto>();

        // Mapeos para Maestros - VariedadProducto
        CreateMap<Domain.Entities.VariedadProducto, VariedadProductoDto>()
            .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src =>
                src.Producto != null ? src.Producto.Nombre : string.Empty))
            .ForMember(dest => dest.UnidadMedidaNombre, opt => opt.MapFrom(src =>
                src.UnidadMedida != null ? src.UnidadMedida.Nombre : string.Empty))
            .ForMember(dest => dest.FichaTecnica, opt => opt.MapFrom(src => src.FichaTecnica));

        // Mapeos para Maestros - Vehiculo
        CreateMap<Domain.Entities.Vehiculo, VehiculoDto>();

        // Mapeos para Maestros - TipoCalidadProducto
        CreateMap<Domain.Entities.TipoCalidadProducto, TipoCalidadProductoDto>()
            .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src =>
                src.Producto != null ? src.Producto.Nombre : string.Empty));

        // Mapeos para Maestros - Banco
        CreateMap<Domain.Entities.Banco, BancoDto>();

        // Mapeos para Maestros - Cargo
        CreateMap<Domain.Entities.Cargo, CargoDto>();

        // Mapeos para Maestros - PersonaCargo
        CreateMap<Domain.Entities.PersonaCargo, PersonaCargoDto>()
            .ForMember(dest => dest.PersonaNombre, opt => opt.MapFrom(src =>
                src.Persona != null ? $"{src.Persona.Nombres} {src.Persona.Apellidos}" : string.Empty))
            .ForMember(dest => dest.CargoNombre, opt => opt.MapFrom(src =>
                src.Cargo != null ? src.Cargo.Nombre : string.Empty))
            .ForMember(dest => dest.ObservacionAsignacion, opt => opt.MapFrom(src => src.ObservacionAsignacion))
            .ForMember(dest => dest.MotivoRevocacion, opt => opt.MapFrom(src => src.MotivoRevocacion));

        // Mapeos para Maestros - Moneda
        CreateMap<Domain.Entities.Moneda, MonedaDto>();

        // Mapeos para Maestros - TipoCambio
        CreateMap<Domain.Entities.TipoCambio, TipoCambioDto>()
            .ForMember(dest => dest.MonedaNombre, opt => opt.MapFrom(src => 
                src.Moneda != null ? src.Moneda.Nombre : string.Empty))
            .ForMember(dest => dest.MonedaCodigo, opt => opt.MapFrom(src => 
                src.Moneda != null ? src.Moneda.Codigo : string.Empty))
            .ForMember(dest => dest.MonedaSimbolo, opt => opt.MapFrom(src => 
                src.Moneda != null ? src.Moneda.Simbolo : string.Empty))
            .ForMember(dest => dest.UsuarioNombre, opt => opt.MapFrom(src => 
                src.Usuario != null && src.Usuario.Persona != null 
                    ? $"{src.Usuario.Persona.Nombres} {src.Usuario.Persona.Apellidos}" 
                    : string.Empty));

        // Mapeos para Ubicaciones
        CreateMap<Ubicacion, UbicacionDto>();

        // Mapeos para Almacén - Productos
        CreateMap<Producto, Miski.Shared.DTOs.Almacen.ProductoDto>()
            .ForMember(dest => dest.CategoriaProductoNombre, opt => opt.MapFrom(src =>
                src.CategoriaProducto != null ? src.CategoriaProducto.Nombre : string.Empty))
            .ForMember(dest => dest.Imagen, opt => opt.MapFrom(src => src.Imagen))
            .ForMember(dest => dest.FichaTecnica, opt => opt.MapFrom(src => src.FichaTecnica));

        // Mapeos para Compras - Negociaciones
        CreateMap<Negociacion, Miski.Shared.DTOs.Compras.NegociacionDto>()
            .ForMember(dest => dest.ProveedorNombre, opt => opt.MapFrom(src => 
                src.Proveedor != null ? $"{src.Proveedor.Nombres} {src.Proveedor.Apellidos}" : string.Empty))
            .ForMember(dest => dest.ComisionistaNombre, opt => opt.MapFrom(src => 
                src.Comisionista != null && src.Comisionista.Persona != null 
                    ? $"{src.Comisionista.Persona.Nombres} {src.Comisionista.Persona.Apellidos}" 
                    : string.Empty))
            .ForMember(dest => dest.IdProducto, opt => opt.MapFrom(src => 
                src.VariedadProducto != null ? (int?)src.VariedadProducto.IdProducto : null))
            .ForMember(dest => dest.VariedadProductoNombre, opt => opt.MapFrom(src => 
                src.VariedadProducto != null ? src.VariedadProducto.Nombre : string.Empty))
            .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => 
                src.VariedadProducto != null && src.VariedadProducto.Producto != null ? src.VariedadProducto.Producto.Nombre : string.Empty))
            .ForMember(dest => dest.TipoDocumentoNombre, opt => opt.MapFrom(src => 
                src.TipoDocumento != null ? src.TipoDocumento.Nombre : string.Empty))
            .ForMember(dest => dest.BancoNombre, opt => opt.MapFrom(src => 
                src.Banco != null ? src.Banco.Nombre : string.Empty))
            .ForMember(dest => dest.AprobadaPorIngenieroNombre, opt => opt.MapFrom(src => 
                src.AprobadaPorUsuarioIngeniero != null && src.AprobadaPorUsuarioIngeniero.Persona != null 
                    ? $"{src.AprobadaPorUsuarioIngeniero.Persona.Nombres} {src.AprobadaPorUsuarioIngeniero.Persona.Apellidos}" 
                    : string.Empty))
            .ForMember(dest => dest.AprobadaPorContadoraNombre, opt => opt.MapFrom(src => 
                src.AprobadaPorUsuarioContadora != null && src.AprobadaPorUsuarioContadora.Persona != null 
                    ? $"{src.AprobadaPorUsuarioContadora.Persona.Nombres} {src.AprobadaPorUsuarioContadora.Persona.Apellidos}" 
                    : string.Empty))
            // ? Mapear IdCompra e IdLote desde la primera compra asociada (relación 1:1)
            .ForMember(dest => dest.IdCompra, opt => opt.MapFrom(src => 
                src.Compras != null && src.Compras.Any() ? (int?)src.Compras.First().IdCompra : null))
            .ForMember(dest => dest.IdLote, opt => opt.MapFrom(src => 
                src.Compras != null && src.Compras.Any() ? src.Compras.First().IdLote : null));

        // Mapeos para Compras - Lotes
        CreateMap<Lote, Miski.Shared.DTOs.Compras.LoteDto>();

        // Mapeos para Compras - CompraVehiculo
        CreateMap<CompraVehiculo, Miski.Shared.DTOs.Compras.CompraVehiculoDto>()
            .ForMember(dest => dest.PersonaNombre, opt => opt.MapFrom(src => 
                src.Persona != null ? $"{src.Persona.Nombres} {src.Persona.Apellidos}" : string.Empty))
            .ForMember(dest => dest.VehiculoPlaca, opt => opt.MapFrom(src => 
                src.Vehiculo != null ? src.Vehiculo.Placa : string.Empty))
            .ForMember(dest => dest.VehiculoMarca, opt => opt.MapFrom(src => 
                src.Vehiculo != null ? src.Vehiculo.Marca : string.Empty))
            .ForMember(dest => dest.VehiculoModelo, opt => opt.MapFrom(src => 
                src.Vehiculo != null ? src.Vehiculo.Modelo : string.Empty))
            .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.CompraVehiculoDetalles));

        CreateMap<CompraVehiculoDetalle, Miski.Shared.DTOs.Compras.CompraVehiculoDetalleDto>()
            .ForMember(dest => dest.CompraSerie, opt => opt.MapFrom(src => 
                src.Compra != null ? src.Compra.Serie : string.Empty))
            .ForMember(dest => dest.CompraFRegistro, opt => opt.MapFrom(src => 
                src.Compra != null ? src.Compra.FRegistro : null))
            .ForMember(dest => dest.CompraMontoTotal, opt => opt.MapFrom(src => 
                src.Compra != null ? src.Compra.MontoTotal : null))
            .ForMember(dest => dest.CompraNegociacionId, opt => opt.MapFrom(src => 
                src.Compra != null ? src.Compra.IdNegociacion.ToString() : string.Empty))
            // ? Relación 1:1: Una Compra tiene un Lote (no una colección)
            .ForMember(dest => dest.IdLote, opt => opt.MapFrom(src => 
                src.Compra != null && src.Compra.Lote != null ? src.Compra.Lote.IdLote : (int?)null))
            .ForMember(dest => dest.LoteCodigo, opt => opt.MapFrom(src => 
                src.Compra != null && src.Compra.Lote != null ? src.Compra.Lote.Codigo : null))
            .ForMember(dest => dest.LotePeso, opt => opt.MapFrom(src => 
                src.Compra != null && src.Compra.Lote != null ? src.Compra.Lote.Peso : (decimal?)null))
            .ForMember(dest => dest.LoteSacos, opt => opt.MapFrom(src => 
                src.Compra != null && src.Compra.Lote != null ? src.Compra.Lote.Sacos : (int?)null));

        // Mapeos para Usuarios
        CreateMap<Usuario, UsuarioDto>()
            .ForMember(dest => dest.PersonaNombre, opt => opt.MapFrom(src => 
                src.Persona != null ? src.Persona.Nombres : string.Empty))
            .ForMember(dest => dest.PersonaApellidos, opt => opt.MapFrom(src => 
                src.Persona != null ? src.Persona.Apellidos : string.Empty))
            .ForMember(dest => dest.PersonaNombreCompleto, opt => opt.MapFrom(src => 
                src.Persona != null ? $"{src.Persona.Nombres} {src.Persona.Apellidos}" : string.Empty))
            .ForMember(dest => dest.PersonaEmail, opt => opt.MapFrom(src => 
                src.Persona != null ? src.Persona.Email : string.Empty))
            .ForMember(dest => dest.PersonaTelefono, opt => opt.MapFrom(src => 
                src.Persona != null ? src.Persona.Telefono : string.Empty))
            .ForMember(dest => dest.PersonaNumeroDocumento, opt => opt.MapFrom(src => 
                src.Persona != null ? src.Persona.NumeroDocumento : string.Empty))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => 
                src.UsuarioRoles
                    .Where(ur => ur.Rol != null)
                    .Select(ur => new Miski.Shared.DTOs.Usuarios.UsuarioRolInfo 
                    { 
                        IdRol = ur.Rol!.IdRol, 
                        Nombre = ur.Rol.Nombre 
                    })
                    .ToList()));

        // Mapeos para CategoriaFAQ
        CreateMap<Domain.Entities.CategoriaFAQ, Miski.Shared.DTOs.FAQ.CategoriaFAQDto>();

        // Mapeos para FAQ
        CreateMap<Domain.Entities.FAQ, Miski.Shared.DTOs.FAQ.FAQDto>()
            .ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(src =>
                src.CategoriaFAQ != null ? src.CategoriaFAQ.Nombre : string.Empty));

        // Mapeos para CompraPago
        CreateMap<CompraPago, Miski.Shared.DTOs.Compras.CompraPagoDto>();
    }
}

// DTOs adicionales que necesitamos
public class ProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public decimal PrecioUnitario { get; set; }
    public string UnidadMedida { get; set; } = string.Empty;
    public int StockMinimo { get; set; }
    public int StockActual { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}

public class PersonaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Documento { get; set; } = string.Empty;
    public string TipoDocumento { get; set; } = string.Empty;
    public string TipoPersona { get; set; } = string.Empty;
    public string TipoDocumentoNombre { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
}