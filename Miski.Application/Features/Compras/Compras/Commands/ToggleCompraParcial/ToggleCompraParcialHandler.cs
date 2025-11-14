using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.Compras.Commands.ToggleCompraParcial;

public class ToggleCompraParcialHandler : IRequestHandler<ToggleCompraParcialCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public ToggleCompraParcialHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(ToggleCompraParcialCommand request, CancellationToken cancellationToken)
    {
        // 1. Verificar que la compra existe
        var compra = await _unitOfWork.Repository<Compra>()
            .GetByIdAsync(request.IdCompra, cancellationToken);

        if (compra == null)
            throw new NotFoundException("Compra", request.IdCompra);

        // 2. Validar que la compra NO está ANULADA
        if (compra.Estado == "ANULADO")
        {
            throw new ValidationException("No se puede marcar/desmarcar como parcial una compra que está anulada");
        }

        // 3. Validar que la compra NO está asignada a un vehículo
        var compraVehiculoDetalles = await _unitOfWork.Repository<CompraVehiculoDetalle>()
            .GetAllAsync(cancellationToken);

        var estaAsignada = compraVehiculoDetalles.Any(cvd => cvd.IdCompra == request.IdCompra);

        if (estaAsignada)
        {
            throw new ValidationException("No se puede marcar/desmarcar como parcial una compra que ya está asignada a un vehículo");
        }

        // 4. Toggle del campo EsParcial
        // Si es "SI" lo cambia a "NO", si es "NO" o null lo cambia a "SI"
        if (compra.EsParcial == "SI")
        {
            compra.EsParcial = "NO";
        }
        else
        {
            compra.EsParcial = "SI";
        }

        // 5. Actualizar la compra
        await _unitOfWork.Repository<Compra>().UpdateAsync(compra, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
