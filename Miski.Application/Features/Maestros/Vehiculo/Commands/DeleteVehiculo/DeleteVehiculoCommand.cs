using MediatR;

namespace Miski.Application.Features.Maestros.Vehiculo.Commands.DeleteVehiculo;

public record DeleteVehiculoCommand(int Id) : IRequest<Unit>;
