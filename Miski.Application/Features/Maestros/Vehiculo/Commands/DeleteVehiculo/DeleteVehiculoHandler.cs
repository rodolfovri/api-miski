using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.Vehiculo.Commands.DeleteVehiculo;

public class DeleteVehiculoHandler : IRequestHandler<DeleteVehiculoCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVehiculoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteVehiculoCommand request, CancellationToken cancellationToken)
    {
        var vehiculo = await _unitOfWork.Repository<Domain.Entities.Vehiculo>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (vehiculo == null)
            throw new NotFoundException("Vehiculo", request.Id);

        // Validar que el vehículo no tenga compras asociadas
        var compraVehiculos = await _unitOfWork.Repository<CompraVehiculo>().GetAllAsync(cancellationToken);
        if (compraVehiculos.Any(cv => cv.IdVehiculo == request.Id))
        {
            throw new ValidationException("No se puede eliminar el vehículo porque tiene compras asociadas");
        }

        // Cambiar estado a INACTIVO en lugar de eliminar físicamente
        vehiculo.Estado = "INACTIVO";
        await _unitOfWork.Repository<Domain.Entities.Vehiculo>().UpdateAsync(vehiculo, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
