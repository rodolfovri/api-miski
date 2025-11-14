using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.Compras.Commands.DesasignarLote;

public class DesasignarLoteDeCompraHandler : IRequestHandler<DesasignarLoteDeCompraCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DesasignarLoteDeCompraHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DesasignarLoteDeCompraCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar que la compra existe
        var compra = await _unitOfWork.Repository<Compra>()
            .GetByIdAsync(request.IdCompra, cancellationToken);

        if (compra == null)
            throw new NotFoundException("Compra", request.IdCompra);

        // 2. Validar que la compra tenga un lote asignado
        if (!compra.IdLote.HasValue)
        {
            throw new ValidationException("La compra no tiene ningún lote asignado");
        }

        // 3. Validar que la compra NO tenga llegadas de planta
        var llegadasPlanta = await _unitOfWork.Repository<LlegadaPlanta>().GetAllAsync(cancellationToken);
        var tieneLlegadas = llegadasPlanta.Any(lp => lp.IdCompra == compra.IdCompra);

        if (tieneLlegadas)
        {
            throw new ValidationException("No se puede desasignar el lote porque la compra ya tiene llegadas de planta registradas");
        }

        // 4. Validar que la compra NO esté asignada a un vehículo
        var compraVehiculoDetalles = await _unitOfWork.Repository<CompraVehiculoDetalle>().GetAllAsync(cancellationToken);
        var estaEnVehiculo = compraVehiculoDetalles.Any(cvd => cvd.IdCompra == compra.IdCompra);

        if (estaEnVehiculo)
        {
            throw new ValidationException("No se puede desasignar el lote porque la compra está asignada a un vehículo");
        }

        // 5. Desasignar el lote
        compra.IdLote = null;
        compra.MontoTotal = null;

        await _unitOfWork.Repository<Compra>().UpdateAsync(compra, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
