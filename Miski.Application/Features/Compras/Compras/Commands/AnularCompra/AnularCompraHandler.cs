using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.Compras.Commands.AnularCompra;

public class AnularCompraHandler : IRequestHandler<AnularCompraCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public AnularCompraHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AnularCompraCommand request, CancellationToken cancellationToken)
    {
        // Verificar que la compra existe
        var compra = await _unitOfWork.Repository<Compra>()
            .GetByIdAsync(request.IdCompra, cancellationToken);

        if (compra == null)
            throw new NotFoundException("Compra", request.IdCompra);

        // Verificar que la compra no está ya anulada
        if (compra.Estado == "ANULADO")
        {
            throw new Shared.Exceptions.ValidationException("La compra ya está anulada");
        }

        // Verificar que el usuario existe
        var usuario = await _unitOfWork.Repository<Usuario>()
            .GetByIdAsync(request.IdUsuarioAnulacion, cancellationToken);

        if (usuario == null)
            throw new NotFoundException("Usuario", request.IdUsuarioAnulacion);

        // Validar que la compra no tenga un vehículo asignado
        var compraVehiculoDetalles = await _unitOfWork.Repository<CompraVehiculoDetalle>()
            .GetAllAsync(cancellationToken);

        var tieneVehiculoAsignado = compraVehiculoDetalles.Any(cvd => cvd.IdCompra == request.IdCompra);

        if (tieneVehiculoAsignado)
        {
            throw new Shared.Exceptions.ValidationException("No se puede anular la compra porque ya tiene un vehículo asignado");
        }

        // Anular la compra
        compra.Estado = "ANULADO";
        compra.IdUsuarioAnulacion = request.IdUsuarioAnulacion;
        compra.MotivoAnulacion = request.MotivoAnulacion;
        compra.FAnulacion = DateTime.Now;

        await _unitOfWork.Repository<Compra>().UpdateAsync(compra, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
