using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Permisos;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Permisos.Commands.AsignarPermisosMultiples;

public class AsignarPermisosMultiplesHandler : IRequestHandler<AsignarPermisosMultiplesCommand, List<PermisoRolDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AsignarPermisosMultiplesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<PermisoRolDto>> Handle(AsignarPermisosMultiplesCommand request, CancellationToken cancellationToken)
    {
        // Validar que el rol existe
        var rol = await _unitOfWork.Repository<Rol>().GetByIdAsync(request.Data.IdRol, cancellationToken);
        if (rol == null)
            throw new NotFoundException("Rol", request.Data.IdRol);

        var resultados = new List<PermisoRolDto>();

        // Procesar cada permiso en una transacción
        foreach (var permisoItem in request.Data.Permisos)
        {
            var permisoDto = await ProcesarPermisoAsync(
                request.Data.IdRol,
                permisoItem,
                cancellationToken);

            resultados.Add(permisoDto);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return resultados;
    }

    private async Task<PermisoRolDto> ProcesarPermisoAsync(
        int idRol,
        PermisoItemDto permisoItem,
        CancellationToken cancellationToken)
    {
        // Validar entidades si se especifican
        if (permisoItem.IdModulo.HasValue)
        {
            var modulo = await _unitOfWork.Repository<Modulo>().GetByIdAsync(permisoItem.IdModulo.Value, cancellationToken);
            if (modulo == null)
                throw new NotFoundException("Módulo", permisoItem.IdModulo.Value);
        }

        SubModulo? subModulo = null;
        if (permisoItem.IdSubModulo.HasValue)
        {
            subModulo = await _unitOfWork.Repository<SubModulo>().GetByIdAsync(permisoItem.IdSubModulo.Value, cancellationToken);
            if (subModulo == null)
                throw new NotFoundException("SubMódulo", permisoItem.IdSubModulo.Value);
        }

        if (permisoItem.IdSubModuloDetalle.HasValue)
        {
            var detalle = await _unitOfWork.Repository<SubModuloDetalle>().GetByIdAsync(permisoItem.IdSubModuloDetalle.Value, cancellationToken);
            if (detalle == null)
                throw new NotFoundException("SubMóduloDetalle", permisoItem.IdSubModuloDetalle.Value);
        }

        // Validar acciones si se especificaron
        if (permisoItem.IdAcciones != null && permisoItem.IdAcciones.Any())
        {
            await ValidarAccionesDisponiblesAsync(
                permisoItem.IdSubModulo,
                permisoItem.IdSubModuloDetalle,
                permisoItem.IdAcciones,
                subModulo,
                cancellationToken);
        }

        // Verificar si ya existe el permiso
        var permisosExistentes = await _unitOfWork.Repository<PermisoRol>().GetAllAsync(cancellationToken);
        var permisoExistente = permisosExistentes.FirstOrDefault(p =>
            p.IdRol == idRol &&
            p.IdModulo == permisoItem.IdModulo &&
            p.IdSubModulo == permisoItem.IdSubModulo &&
            p.IdSubModuloDetalle == permisoItem.IdSubModuloDetalle);

        PermisoRol permiso;

        if (permisoExistente != null)
        {
            // Actualizar el permiso existente
            permisoExistente.TieneAcceso = permisoItem.TieneAcceso;
            await _unitOfWork.Repository<PermisoRol>().UpdateAsync(permisoExistente);
            permiso = permisoExistente;
        }
        else
        {
            // Crear nuevo permiso
            permiso = new PermisoRol
            {
                IdRol = idRol,
                IdModulo = permisoItem.IdModulo,
                IdSubModulo = permisoItem.IdSubModulo,
                IdSubModuloDetalle = permisoItem.IdSubModuloDetalle,
                TieneAcceso = permisoItem.TieneAcceso
            };

            await _unitOfWork.Repository<PermisoRol>().AddAsync(permiso, cancellationToken);
            
            // Necesitamos guardar para obtener el IdPermisoRol
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Gestionar PermisoRolAccion si se especificaron acciones
        if (permisoItem.IdAcciones != null && permisoItem.IdAcciones.Any())
        {
            await GestionarAccionesAsync(permiso.IdPermisoRol, permisoItem.IdAcciones, cancellationToken);
        }

        return _mapper.Map<PermisoRolDto>(permiso);
    }

    private async Task ValidarAccionesDisponiblesAsync(
        int? idSubModulo,
        int? idSubModuloDetalle,
        List<int> idAcciones,
        SubModulo? subModulo,
        CancellationToken cancellationToken)
    {
        HashSet<int> accionesDisponibles;

        if (idSubModuloDetalle.HasValue)
        {
            // Validar acciones disponibles para SubMóduloDetalle
            var subModuloDetalleAcciones = await _unitOfWork.Repository<SubModuloDetalleAccion>().GetAllAsync(cancellationToken);
            accionesDisponibles = subModuloDetalleAcciones
                .Where(smda => smda.IdSubModuloDetalle == idSubModuloDetalle.Value && smda.Habilitado)
                .Select(smda => smda.IdAccion)
                .ToHashSet();

            var accionesNoDisponibles = idAcciones.Where(id => !accionesDisponibles.Contains(id)).ToList();
            if (accionesNoDisponibles.Any())
            {
                throw new ValidationException(
                    $"Las siguientes acciones no están disponibles para este SubMóduloDetalle: {string.Join(", ", accionesNoDisponibles)}");
            }
        }
        else if (idSubModulo.HasValue)
        {
            // Validar que el SubMódulo NO tiene detalles (TieneDetalles = false)
            if (subModulo != null && subModulo.TieneDetalles)
            {
                throw new ValidationException(
                    "No se pueden asignar acciones directamente a un SubMódulo que tiene detalles. " +
                    "Asigne las acciones a los SubMóduloDetalles individuales.");
            }

            // Validar acciones disponibles para SubMódulo
            var subModuloAcciones = await _unitOfWork.Repository<SubModuloAccion>().GetAllAsync(cancellationToken);
            accionesDisponibles = subModuloAcciones
                .Where(sma => sma.IdSubModulo == idSubModulo.Value && sma.Habilitado)
                .Select(sma => sma.IdAccion)
                .ToHashSet();

            var accionesNoDisponibles = idAcciones.Where(id => !accionesDisponibles.Contains(id)).ToList();
            if (accionesNoDisponibles.Any())
            {
                throw new ValidationException(
                    $"Las siguientes acciones no están disponibles para este SubMódulo: {string.Join(", ", accionesNoDisponibles)}");
            }
        }
        else
        {
            // No se pueden asignar acciones a nivel de Módulo
            throw new ValidationException(
                "No se pueden asignar acciones a nivel de Módulo. " +
                "Las acciones solo se pueden asignar a SubMódulos (sin detalles) o SubMóduloDetalles.");
        }
    }

    private async Task GestionarAccionesAsync(int idPermisoRol, List<int> idAcciones, CancellationToken cancellationToken)
    {
        // Obtener permisos de acciones existentes
        var permisosAccionExistentes = await _unitOfWork.Repository<PermisoRolAccion>().GetAllAsync(cancellationToken);
        var permisosActuales = permisosAccionExistentes.Where(pra => pra.IdPermisoRol == idPermisoRol).ToList();

        // Eliminar permisos que ya no están en la lista
        var accionesAEliminar = permisosActuales.Where(pa => !idAcciones.Contains(pa.IdAccion)).ToList();
        foreach (var accionEliminar in accionesAEliminar)
        {
            await _unitOfWork.Repository<PermisoRolAccion>().DeleteAsync(accionEliminar);
        }

        // Agregar o actualizar permisos
        foreach (var idAccion in idAcciones)
        {
            var permisoAccionExistente = permisosActuales.FirstOrDefault(pa => pa.IdAccion == idAccion);

            if (permisoAccionExistente != null)
            {
                // Actualizar habilitado
                permisoAccionExistente.Habilitado = true;
                await _unitOfWork.Repository<PermisoRolAccion>().UpdateAsync(permisoAccionExistente);
            }
            else
            {
                // Crear nuevo permiso de acción
                var nuevoPermisoAccion = new PermisoRolAccion
                {
                    IdPermisoRol = idPermisoRol,
                    IdAccion = idAccion,
                    Habilitado = true
                };

                await _unitOfWork.Repository<PermisoRolAccion>().AddAsync(nuevoPermisoAccion, cancellationToken);
            }
        }
    }
}
