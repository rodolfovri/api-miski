using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.CompraVehiculos.Commands.EliminarCompraDeVehiculo;

public class EliminarCompraDeVehiculoHandler : IRequestHandler<EliminarCompraDeVehiculoCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public EliminarCompraDeVehiculoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(EliminarCompraDeVehiculoCommand request, CancellationToken cancellationToken)
    {
        // 1. Verificar que el CompraVehiculoDetalle existe
        var detalle = await _unitOfWork.Repository<CompraVehiculoDetalle>()
            .GetByIdAsync(request.IdCompraVehiculoDetalle, cancellationToken);

        if (detalle == null)
            throw new NotFoundException("CompraVehiculoDetalle", request.IdCompraVehiculoDetalle);

        // 2. Verificar que el CompraVehiculo asociado existe
        var compraVehiculo = await _unitOfWork.Repository<CompraVehiculo>()
            .GetByIdAsync(detalle.IdCompraVehiculo, cancellationToken);

        if (compraVehiculo == null)
            throw new NotFoundException("CompraVehiculo", detalle.IdCompraVehiculo);

        // 3. Validar que el CompraVehiculo está en estado ACTIVO
        if (compraVehiculo.Estado != "ACTIVO")
        {
            string mensaje = compraVehiculo.Estado == "PARCIAL" 
                ? "No se puede eliminar la asociación porque el vehículo ya inició la entrega a planta (estado PARCIAL)" 
                : "No se puede eliminar la asociación porque el vehículo ya fue entregado a planta";
            throw new Shared.Exceptions.ValidationException(mensaje);
        }

        // 4. Verificar que la Compra existe
        var compra = await _unitOfWork.Repository<Compra>()
            .GetByIdAsync(detalle.IdCompra, cancellationToken);

        if (compra == null)
            throw new NotFoundException("Compra", detalle.IdCompra);

        // 5. Validar que la Compra NO ha sido recepcionada
        if (compra.EstadoRecepcion == "RECEPCIONADO")
        {
            throw new Shared.Exceptions.ValidationException("No se puede eliminar la asociación porque la compra ya fue recepcionada en planta");
        }

        // 6. Validar que la Compra NO tiene llegadas a planta registradas
        var llegadasPlanta = await _unitOfWork.Repository<LlegadaPlanta>()
            .GetAllAsync(cancellationToken);

        var tieneLlegadas = llegadasPlanta.Any(lp => lp.IdCompra == detalle.IdCompra);

        if (tieneLlegadas)
        {
            throw new Shared.Exceptions.ValidationException("No se puede eliminar la asociación porque la compra tiene llegadas a planta registradas");
        }

        // 7. Eliminar el CompraVehiculoDetalle
        await _unitOfWork.Repository<CompraVehiculoDetalle>().DeleteAsync(detalle, cancellationToken);

        // 8. Actualizar el EstadoRecepcion de la Compra a NULL (ya no está asignada)
        compra.EstadoRecepcion = null;
        await _unitOfWork.Repository<Compra>().UpdateAsync(compra, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
