using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Permisos;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Permisos.Commands.AsignarAccionSubModuloDetalle;

public class AsignarAccionSubModuloDetalleHandler : IRequestHandler<AsignarAccionSubModuloDetalleCommand, SubModuloDetalleAccionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AsignarAccionSubModuloDetalleHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SubModuloDetalleAccionDto> Handle(AsignarAccionSubModuloDetalleCommand request, CancellationToken cancellationToken)
    {
        // Validar que el SubMóduloDetalle existe
        var detalle = await _unitOfWork.Repository<SubModuloDetalle>().GetByIdAsync(request.IdSubModuloDetalle, cancellationToken);
        if (detalle == null)
            throw new NotFoundException("SubMóduloDetalle", request.IdSubModuloDetalle);

        // Validar que la Acción existe
        var accion = await _unitOfWork.Repository<Accion>().GetByIdAsync(request.IdAccion, cancellationToken);
        if (accion == null)
            throw new NotFoundException("Acción", request.IdAccion);

        // Verificar si ya existe la asignación
        var subModuloDetalleAcciones = await _unitOfWork.Repository<SubModuloDetalleAccion>().GetAllAsync(cancellationToken);
        var asignacionExistente = subModuloDetalleAcciones.FirstOrDefault(smda =>
            smda.IdSubModuloDetalle == request.IdSubModuloDetalle &&
            smda.IdAccion == request.IdAccion);

        SubModuloDetalleAccion subModuloDetalleAccion;

        if (asignacionExistente != null)
        {
            // Actualizar la asignación existente
            asignacionExistente.Habilitado = request.Habilitado;
            await _unitOfWork.Repository<SubModuloDetalleAccion>().UpdateAsync(asignacionExistente);
            subModuloDetalleAccion = asignacionExistente;
        }
        else
        {
            // Crear nueva asignación
            subModuloDetalleAccion = new SubModuloDetalleAccion
            {
                IdSubModuloDetalle = request.IdSubModuloDetalle,
                IdAccion = request.IdAccion,
                Habilitado = request.Habilitado
            };

            await _unitOfWork.Repository<SubModuloDetalleAccion>().AddAsync(subModuloDetalleAccion, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SubModuloDetalleAccionDto>(subModuloDetalleAccion);
    }
}
