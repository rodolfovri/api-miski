using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.Compras.Commands.AsignarLote;

public class AsignarLoteACompraHandler : IRequestHandler<AsignarLoteACompraCommand, CompraDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AsignarLoteACompraHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CompraDto> Handle(AsignarLoteACompraCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar que la compra existe
        var compra = await _unitOfWork.Repository<Compra>()
            .GetByIdAsync(request.IdCompra, cancellationToken);

        if (compra == null)
            throw new NotFoundException("Compra", request.IdCompra);

        // 2. Validar que el lote existe
        var lote = await _unitOfWork.Repository<Lote>()
            .GetByIdAsync(request.IdLote, cancellationToken);

        if (lote == null)
            throw new NotFoundException("Lote", request.IdLote);

        // 3. Validar que la compra NO tenga un lote ya asignado
        if (compra.IdLote.HasValue)
        {
            throw new ValidationException("La compra ya tiene un lote asignado. Una compra solo puede tener un lote (relación 1:1)");
        }

        // 4. Validar que el lote NO esté asignado a otra compra
        var compras = await _unitOfWork.Repository<Compra>().GetAllAsync(cancellationToken);
        var otraCompraConLote = compras.FirstOrDefault(c => c.IdLote == request.IdLote);

        if (otraCompraConLote != null)
        {
            throw new ValidationException($"El lote ya está asignado a la compra #{otraCompraConLote.IdCompra}. Un lote solo puede estar asignado a una compra");
        }

        // 5. Validar que la compra esté ACTIVA
        if (compra.Estado != "ACTIVO")
        {
            throw new ValidationException($"Solo se pueden asignar lotes a compras con estado ACTIVO. Estado actual: {compra.Estado}");
        }

        // 6. Asignar el lote a la compra y actualizar el monto total
        compra.IdLote = request.IdLote;
        compra.MontoTotal = request.MontoTotal;

        await _unitOfWork.Repository<Compra>().UpdateAsync(compra, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 7. Cargar relaciones para el DTO
        compra.Lote = lote;
        var negociacion = await _unitOfWork.Repository<Negociacion>()
            .GetByIdAsync(compra.IdNegociacion, cancellationToken);
        compra.Negociacion = negociacion!;

        return _mapper.Map<CompraDto>(compra);
    }
}
