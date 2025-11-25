using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Permisos;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Permisos.Commands.AsignarAccionSubModulo;

public class AsignarAccionSubModuloHandler : IRequestHandler<AsignarAccionSubModuloCommand, SubModuloAccionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AsignarAccionSubModuloHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SubModuloAccionDto> Handle(AsignarAccionSubModuloCommand request, CancellationToken cancellationToken)
    {
        // Validar que el SubMódulo existe
        var subModulo = await _unitOfWork.Repository<SubModulo>().GetByIdAsync(request.IdSubModulo, cancellationToken);
        if (subModulo == null)
            throw new NotFoundException("SubMódulo", request.IdSubModulo);

        // Validar que el SubMódulo NO tiene detalles
        if (subModulo.TieneDetalles)
        {
            throw new ValidationException(
                "No se pueden asignar acciones directamente a un SubMódulo que tiene detalles. " +
                "Asigne las acciones a los SubMóduloDetalles individuales.");
        }

        // Validar que la Acción existe
        var accion = await _unitOfWork.Repository<Accion>().GetByIdAsync(request.IdAccion, cancellationToken);
        if (accion == null)
            throw new NotFoundException("Acción", request.IdAccion);

        // Verificar si ya existe la asignación
        var subModuloAcciones = await _unitOfWork.Repository<SubModuloAccion>().GetAllAsync(cancellationToken);
        var asignacionExistente = subModuloAcciones.FirstOrDefault(sma =>
            sma.IdSubModulo == request.IdSubModulo &&
            sma.IdAccion == request.IdAccion);

        SubModuloAccion subModuloAccion;

        if (asignacionExistente != null)
        {
            // Actualizar la asignación existente
            asignacionExistente.Habilitado = request.Habilitado;
            await _unitOfWork.Repository<SubModuloAccion>().UpdateAsync(asignacionExistente);
            subModuloAccion = asignacionExistente;
        }
        else
        {
            // Crear nueva asignación
            subModuloAccion = new SubModuloAccion
            {
                IdSubModulo = request.IdSubModulo,
                IdAccion = request.IdAccion,
                Habilitado = request.Habilitado
            };

            await _unitOfWork.Repository<SubModuloAccion>().AddAsync(subModuloAccion, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SubModuloAccionDto>(subModuloAccion);
    }
}
