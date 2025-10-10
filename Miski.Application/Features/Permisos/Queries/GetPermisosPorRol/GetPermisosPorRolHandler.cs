using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Permisos;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Permisos.Queries.GetPermisosPorRol;

public class GetPermisosPorRolHandler : IRequestHandler<GetPermisosPorRolQuery, PermisoRolConJerarquiaDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPermisosPorRolHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PermisoRolConJerarquiaDto> Handle(GetPermisosPorRolQuery request, CancellationToken cancellationToken)
    {
        // Validar que el rol existe
        var rol = await _unitOfWork.Repository<Rol>().GetByIdAsync(request.IdRol, cancellationToken);
        if (rol == null)
            throw new NotFoundException("Rol", request.IdRol);

        // Obtener todos los módulos
        var modulos = await _unitOfWork.Repository<Modulo>().GetAllAsync(cancellationToken);
        var subModulos = await _unitOfWork.Repository<SubModulo>().GetAllAsync(cancellationToken);
        var detalles = await _unitOfWork.Repository<SubModuloDetalle>().GetAllAsync(cancellationToken);
        var permisos = await _unitOfWork.Repository<PermisoRol>().GetAllAsync(cancellationToken);

        // Filtrar permisos del rol
        var permisosRol = permisos.Where(p => p.IdRol == request.IdRol).ToList();

        var resultado = new PermisoRolConJerarquiaDto
        {
            IdRol = rol.IdRol,
            RolNombre = rol.Nombre,
            Modulos = modulos
                .Where(m => m.Estado == "ACTIVO")
                .OrderBy(m => m.Orden)
                .Select(modulo => new ModuloPermisoDto
                {
                    IdModulo = modulo.IdModulo,
                    Nombre = modulo.Nombre,
                    Orden = modulo.Orden,
                    TieneAcceso = TieneAccesoModulo(modulo.IdModulo, permisosRol),
                    SubModulos = subModulos
                        .Where(sm => sm.IdModulo == modulo.IdModulo && sm.Estado == "ACTIVO")
                        .OrderBy(sm => sm.Orden)
                        .Select(subModulo => new SubModuloPermisoDto
                        {
                            IdSubModulo = subModulo.IdSubModulo,
                            Nombre = subModulo.Nombre,
                            Orden = subModulo.Orden,
                            TieneAcceso = TieneAccesoSubModulo(modulo.IdModulo, subModulo.IdSubModulo, permisosRol),
                            SubModuloDetalles = detalles
                                .Where(d => d.IdSubModulo == subModulo.IdSubModulo && d.Estado == "ACTIVO")
                                .OrderBy(d => d.Orden)
                                .Select(detalle => new SubModuloDetallePermisoDto
                                {
                                    IdSubModuloDetalle = detalle.IdSubModuloDetalle,
                                    Nombre = detalle.Nombre,
                                    Orden = detalle.Orden,
                                    TieneAcceso = TieneAccesoDetalle(modulo.IdModulo, subModulo.IdSubModulo, detalle.IdSubModuloDetalle, permisosRol)
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList()
        };

        return resultado;
    }

    /// <summary>
    /// Si un rol tiene acceso a un Módulo, automáticamente accede a todos sus subniveles
    /// </summary>
    private bool TieneAccesoModulo(int idModulo, List<PermisoRol> permisos)
    {
        var permisoModulo = permisos.FirstOrDefault(p => 
            p.IdModulo == idModulo && 
            !p.IdSubModulo.HasValue && 
            !p.IdSubModuloDetalle.HasValue);

        return permisoModulo?.TieneAcceso ?? false;
    }

    /// <summary>
    /// Verifica acceso a SubMódulo considerando herencia del Módulo padre
    /// </summary>
    private bool TieneAccesoSubModulo(int idModulo, int idSubModulo, List<PermisoRol> permisos)
    {
        // Primero verificar si tiene acceso al módulo completo
        if (TieneAccesoModulo(idModulo, permisos))
            return true;

        // Si no, verificar permiso específico del submódulo
        var permisoSubModulo = permisos.FirstOrDefault(p =>
            p.IdSubModulo == idSubModulo &&
            !p.IdSubModuloDetalle.HasValue);

        return permisoSubModulo?.TieneAcceso ?? false;
    }

    /// <summary>
    /// Verifica acceso a Detalle considerando herencia del SubMódulo y Módulo
    /// </summary>
    private bool TieneAccesoDetalle(int idModulo, int idSubModulo, int idDetalle, List<PermisoRol> permisos)
    {
        // Primero verificar si tiene acceso al módulo completo
        if (TieneAccesoModulo(idModulo, permisos))
            return true;

        // Luego verificar si tiene acceso al submódulo completo
        if (TieneAccesoSubModulo(idModulo, idSubModulo, permisos))
            return true;

        // Finalmente verificar permiso específico del detalle
        var permisoDetalle = permisos.FirstOrDefault(p =>
            p.IdSubModuloDetalle == idDetalle);

        return permisoDetalle?.TieneAcceso ?? false;
    }
}