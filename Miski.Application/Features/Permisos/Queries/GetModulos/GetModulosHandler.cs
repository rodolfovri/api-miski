using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Permisos;

namespace Miski.Application.Features.Permisos.Queries.GetModulos;

public class GetModulosHandler : IRequestHandler<GetModulosQuery, List<ModuloDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetModulosHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<ModuloDto>> Handle(GetModulosQuery request, CancellationToken cancellationToken)
    {
        var modulos = await _unitOfWork.Repository<Modulo>().GetAllAsync(cancellationToken);
        var subModulos = await _unitOfWork.Repository<SubModulo>().GetAllAsync(cancellationToken);
        var detalles = await _unitOfWork.Repository<SubModuloDetalle>().GetAllAsync(cancellationToken);
        var acciones = await _unitOfWork.Repository<Accion>().GetAllAsync(cancellationToken);
        var subModuloAcciones = await _unitOfWork.Repository<SubModuloAccion>().GetAllAsync(cancellationToken);
        var subModuloDetalleAcciones = await _unitOfWork.Repository<SubModuloDetalleAccion>().GetAllAsync(cancellationToken);

        // Filtrar por tipo de plataforma si se especifica
        if (!string.IsNullOrEmpty(request.TipoPlataforma))
        {
            modulos = modulos.Where(m => m.TipoPlataforma == request.TipoPlataforma).ToList();
        }

        var accionesActivas = acciones.Where(a => a.Estado == "ACTIVO").ToList();

        var resultado = modulos
            .Where(m => m.Estado == "ACTIVO")
            .OrderBy(m => m.Orden)
            .Select(modulo => new ModuloDto
            {
                IdModulo = modulo.IdModulo,
                Nombre = modulo.Nombre,
                Ruta = modulo.Ruta,
                Icono = modulo.Icono,
                Orden = modulo.Orden,
                Estado = modulo.Estado,
                TipoPlataforma = modulo.TipoPlataforma,
                SubModulos = subModulos
                    .Where(sm => sm.IdModulo == modulo.IdModulo && sm.Estado == "ACTIVO")
                    .OrderBy(sm => sm.Orden)
                    .Select(subModulo => BuildSubModuloDto(subModulo, modulo.Nombre, detalles, accionesActivas, subModuloAcciones, subModuloDetalleAcciones))
                    .ToList()
            })
            .ToList();

        return resultado;
    }

    private SubModuloDto BuildSubModuloDto(
        SubModulo subModulo,
        string moduloNombre,
        IEnumerable<SubModuloDetalle> detalles,
        List<Accion> acciones,
        IEnumerable<SubModuloAccion> subModuloAcciones,
        IEnumerable<SubModuloDetalleAccion> subModuloDetalleAcciones)
    {
        var dto = new SubModuloDto
        {
            IdSubModulo = subModulo.IdSubModulo,
            IdModulo = subModulo.IdModulo,
            Nombre = subModulo.Nombre,
            Ruta = subModulo.Ruta,
            Icono = subModulo.Icono,
            Orden = subModulo.Orden,
            Estado = subModulo.Estado,
            TieneDetalles = subModulo.TieneDetalles,
            ModuloNombre = moduloNombre
        };

        if (subModulo.TieneDetalles)
        {
            // SubMódulo tiene detalles
            dto.SubModuloDetalles = detalles
                .Where(d => d.IdSubModulo == subModulo.IdSubModulo && d.Estado == "ACTIVO")
                .OrderBy(d => d.Orden)
                .Select(detalle => BuildSubModuloDetalleDto(detalle, subModulo.Nombre, acciones, subModuloDetalleAcciones))
                .ToList();
        }
        else
        {
            // SubMódulo tiene acciones directas - obtener las acciones disponibles
            var accionesDisponibles = subModuloAcciones
                .Where(sma => sma.IdSubModulo == subModulo.IdSubModulo && sma.Habilitado)
                .Select(sma => sma.IdAccion)
                .ToHashSet();

            dto.Acciones = acciones
                .Where(a => accionesDisponibles.Contains(a.IdAccion))
                .OrderBy(a => a.Orden)
                .Select(a => new AccionDto
                {
                    IdAccion = a.IdAccion,
                    Nombre = a.Nombre,
                    Codigo = a.Codigo,
                    Icono = a.Icono,
                    Orden = a.Orden,
                    Estado = a.Estado
                })
                .ToList();
        }

        return dto;
    }

    private SubModuloDetalleDto BuildSubModuloDetalleDto(
        SubModuloDetalle detalle,
        string subModuloNombre,
        List<Accion> acciones,
        IEnumerable<SubModuloDetalleAccion> subModuloDetalleAcciones)
    {
        // Obtener las acciones disponibles para este detalle
        var accionesDisponibles = subModuloDetalleAcciones
            .Where(smda => smda.IdSubModuloDetalle == detalle.IdSubModuloDetalle && smda.Habilitado)
            .Select(smda => smda.IdAccion)
            .ToHashSet();

        return new SubModuloDetalleDto
        {
            IdSubModuloDetalle = detalle.IdSubModuloDetalle,
            IdSubModulo = detalle.IdSubModulo,
            Nombre = detalle.Nombre,
            Ruta = detalle.Ruta,
            Icono = detalle.Icono,
            Orden = detalle.Orden,
            Estado = detalle.Estado,
            SubModuloNombre = subModuloNombre,
            Acciones = acciones
                .Where(a => accionesDisponibles.Contains(a.IdAccion))
                .OrderBy(a => a.Orden)
                .Select(a => new AccionDto
                {
                    IdAccion = a.IdAccion,
                    Nombre = a.Nombre,
                    Codigo = a.Codigo,
                    Icono = a.Icono,
                    Orden = a.Orden,
                    Estado = a.Estado
                })
                .ToList()
        };
    }
}