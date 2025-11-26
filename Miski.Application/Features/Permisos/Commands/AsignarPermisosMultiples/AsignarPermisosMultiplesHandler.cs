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

        // PASO 1: Eliminar TODOS los permisos existentes del rol
        await EliminarPermisosExistentesAsync(request.Data.IdRol, cancellationToken);

        var resultados = new List<PermisoRolDto>();

        // PASO 2: Crear los nuevos permisos que vienen del frontend
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

    /// <summary>
    /// Elimina todos los permisos y acciones existentes del rol
    /// Orden: Primero PermisoRolAccion (hija), luego PermisoRol (padre)
    /// </summary>
    private async Task EliminarPermisosExistentesAsync(int idRol, CancellationToken cancellationToken)
    {
        // Obtener todos los permisos del rol
        var permisosExistentes = await _unitOfWork.Repository<PermisoRol>().GetAllAsync(cancellationToken);
        var permisosDelRol = permisosExistentes.Where(p => p.IdRol == idRol).ToList();

        if (!permisosDelRol.Any())
            return; // No hay permisos que eliminar

        // Obtener todos los IDs de permisos del rol
        var idsPermisosRol = permisosDelRol.Select(p => p.IdPermisoRol).ToHashSet();

        // 1. Eliminar TODAS las acciones asociadas (tabla hija PermisoRolAccion)
        var permisosAccion = await _unitOfWork.Repository<PermisoRolAccion>().GetAllAsync(cancellationToken);
        var accionesAEliminar = permisosAccion
            .Where(pa => idsPermisosRol.Contains(pa.IdPermisoRol))
            .ToList();

        foreach (var accion in accionesAEliminar)
        {
            await _unitOfWork.Repository<PermisoRolAccion>().DeleteAsync(accion);
        }

        // 2. Eliminar TODOS los permisos del rol (tabla padre PermisoRol)
        foreach (var permiso in permisosDelRol)
        {
            await _unitOfWork.Repository<PermisoRol>().DeleteAsync(permiso);
        }

        // Guardar los cambios de eliminación antes de crear nuevos
        await _unitOfWork.SaveChangesAsync(cancellationToken);
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

        // 3. Crear nuevo permiso (ya no existe porque se eliminó todo antes)
        var permiso = new PermisoRol
        {
            IdRol = idRol,
            IdModulo = permisoItem.IdModulo,
            IdSubModulo = permisoItem.IdSubModulo,
            IdSubModuloDetalle = permisoItem.IdSubModuloDetalle,
            TieneAcceso = permisoItem.TieneAcceso
        };

        await _unitOfWork.Repository<PermisoRol>().AddAsync(permiso, cancellationToken);
        
        // Guardar para obtener el IdPermisoRol generado
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 4. Crear las nuevas acciones si se especificaron
        if (permisoItem.IdAcciones != null && permisoItem.IdAcciones.Any())
        {
            await CrearAccionesAsync(permiso.IdPermisoRol, permisoItem.IdAcciones, cancellationToken);
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

    /// <summary>
    /// Crea las acciones para un permiso (ya no necesita eliminar porque se eliminó todo antes)
    /// </summary>
    private async Task CrearAccionesAsync(int idPermisoRol, List<int> idAcciones, CancellationToken cancellationToken)
    {
        foreach (var idAccion in idAcciones)
        {
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
