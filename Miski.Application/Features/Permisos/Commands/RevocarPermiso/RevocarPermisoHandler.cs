using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Permisos.Commands.RevocarPermiso;

public class RevocarPermisoHandler : IRequestHandler<RevocarPermisoCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public RevocarPermisoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(RevocarPermisoCommand request, CancellationToken cancellationToken)
    {
        // Validar que el rol existe
        var rol = await _unitOfWork.Repository<Rol>().GetByIdAsync(request.IdRol, cancellationToken);
        if (rol == null)
            throw new NotFoundException("Rol", request.IdRol);

        // Buscar el permiso existente
        var permisosExistentes = await _unitOfWork.Repository<PermisoRol>().GetAllAsync(cancellationToken);
        var permisoExistente = permisosExistentes.FirstOrDefault(p =>
            p.IdRol == request.IdRol &&
            p.IdModulo == request.IdModulo &&
            p.IdSubModulo == request.IdSubModulo &&
            p.IdSubModuloDetalle == request.IdSubModuloDetalle);

        if (permisoExistente == null)
        {
            // No existe el permiso, no hay nada que revocar
            return false;
        }

        // Eliminar las acciones asociadas primero
        var permisosAccion = await _unitOfWork.Repository<PermisoRolAccion>().GetAllAsync(cancellationToken);
        var accionesDelPermiso = permisosAccion.Where(pa => pa.IdPermisoRol == permisoExistente.IdPermisoRol).ToList();

        foreach (var accion in accionesDelPermiso)
        {
            await _unitOfWork.Repository<PermisoRolAccion>().DeleteAsync(accion);
        }

        // Marcar el permiso como sin acceso (o eliminarlo completamente)
        // Opción 1: Actualizar TieneAcceso a false
        permisoExistente.TieneAcceso = false;
        await _unitOfWork.Repository<PermisoRol>().UpdateAsync(permisoExistente);

        // Opción 2: Eliminar el registro completamente (descomentar si prefieres esta opción)
        // await _unitOfWork.Repository<PermisoRol>().DeleteAsync(permisoExistente);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
