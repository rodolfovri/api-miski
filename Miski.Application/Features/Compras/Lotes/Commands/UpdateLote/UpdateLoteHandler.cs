using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Miski.Application.Features.Compras.Lotes.Commands.UpdateLote;

public class UpdateLoteHandler : IRequestHandler<UpdateLoteCommand, LoteDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateLoteHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LoteDto> Handle(UpdateLoteCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Lote;

        var lote = await _unitOfWork.Repository<Lote>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (lote == null)
            throw new NotFoundException("Lote", request.Id);

        // ? Validar si el lote está asignado a alguna compra (relación 1:1 inversa)
        var compras = await _unitOfWork.Repository<Compra>().GetAllAsync(cancellationToken);
        var compraAsociada = compras.FirstOrDefault(c => c.IdLote == lote.IdLote);

        if (compraAsociada != null)
        {
            // Validar el EstadoRecepcion de la compra asociada
            if (!string.IsNullOrEmpty(compraAsociada.EstadoRecepcion))
            {
                if (compraAsociada.EstadoRecepcion == "PENDIENTE")
                {
                    throw new ValidationException("No se puede editar el lote porque la compra ya está asignada a un vehículo");
                }

                if (compraAsociada.EstadoRecepcion == "RECEPCIONADO")
                {
                    throw new ValidationException("No se puede editar el lote porque la compra ya ha sido recepcionada en planta");
                }
            }
        }

        // Validar que el código no esté duplicado (excepto el mismo lote)
        if (!string.IsNullOrEmpty(dto.Codigo))
        {
            var lotes = await _unitOfWork.Repository<Lote>().GetAllAsync(cancellationToken);
            if (lotes.Any(l => l.Codigo == dto.Codigo && l.IdLote != request.Id))
            {
                throw new ValidationException($"Ya existe otro lote con el código {dto.Codigo}");
            }
        }

        // ? Actualizar solo los campos del lote (NO la relación con Compra)
        lote.Peso = dto.Peso;
        lote.Sacos = dto.Sacos;
        lote.Codigo = dto.Codigo;
        lote.Comision = dto.Comision;
        lote.Observacion = dto.Observacion;

        await _unitOfWork.Repository<Lote>().UpdateAsync(lote, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // ? Si se proporciona MontoTotal, actualizar la compra asociada
        if (dto.MontoTotal.HasValue && compraAsociada != null)
        {
            compraAsociada.MontoTotal = dto.MontoTotal.Value;
            await _unitOfWork.Repository<Compra>().UpdateAsync(compraAsociada, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return _mapper.Map<LoteDto>(lote);
    }
}
