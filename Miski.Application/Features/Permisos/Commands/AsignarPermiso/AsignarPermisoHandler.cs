using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Permisos;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Permisos.Commands.AsignarPermiso;

public class AsignarPermisoHandler : IRequestHandler<AsignarPermisoCommand, PermisoRolDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AsignarPermisoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PermisoRolDto> Handle(AsignarPermisoCommand request, CancellationToken cancellationToken)
    {
        // Validar que el rol existe
        var rol = await _unitOfWork.Repository<Rol>().GetByIdAsync(request.Permiso.IdRol, cancellationToken);
        if (rol == null)
            throw new NotFoundException("Rol", request.Permiso.IdRol);

        // Validar que el módulo existe si se especifica
        if (request.Permiso.IdModulo.HasValue)
        {
            var modulo = await _unitOfWork.Repository<Modulo>().GetByIdAsync(request.Permiso.IdModulo.Value, cancellationToken);
            if (modulo == null)
                throw new NotFoundException("Módulo", request.Permiso.IdModulo.Value);
        }

        // Validar que el submódulo existe si se especifica
        if (request.Permiso.IdSubModulo.HasValue)
        {
            var subModulo = await _unitOfWork.Repository<SubModulo>().GetByIdAsync(request.Permiso.IdSubModulo.Value, cancellationToken);
            if (subModulo == null)
                throw new NotFoundException("SubMódulo", request.Permiso.IdSubModulo.Value);
        }

        // Validar que el detalle existe si se especifica
        if (request.Permiso.IdSubModuloDetalle.HasValue)
        {
            var detalle = await _unitOfWork.Repository<SubModuloDetalle>().GetByIdAsync(request.Permiso.IdSubModuloDetalle.Value, cancellationToken);
            if (detalle == null)
                throw new NotFoundException("SubMóduloDetalle", request.Permiso.IdSubModuloDetalle.Value);
        }

        // Verificar si ya existe el permiso
        var permisosExistentes = await _unitOfWork.Repository<PermisoRol>().GetAllAsync(cancellationToken);
        var permisoExistente = permisosExistentes.FirstOrDefault(p =>
            p.IdRol == request.Permiso.IdRol &&
            p.IdModulo == request.Permiso.IdModulo &&
            p.IdSubModulo == request.Permiso.IdSubModulo &&
            p.IdSubModuloDetalle == request.Permiso.IdSubModuloDetalle);

        if (permisoExistente != null)
        {
            // Actualizar el permiso existente
            permisoExistente.TieneAcceso = request.Permiso.TieneAcceso;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<PermisoRolDto>(permisoExistente);
        }

        // Crear nuevo permiso
        var nuevoPermiso = new PermisoRol
        {
            IdRol = request.Permiso.IdRol,
            IdModulo = request.Permiso.IdModulo,
            IdSubModulo = request.Permiso.IdSubModulo,
            IdSubModuloDetalle = request.Permiso.IdSubModuloDetalle,
            TieneAcceso = request.Permiso.TieneAcceso
        };

        await _unitOfWork.Repository<PermisoRol>().AddAsync(nuevoPermiso, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PermisoRolDto>(nuevoPermiso);
    }
}