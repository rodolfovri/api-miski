using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Vehiculo.Commands.CreateVehiculo;

public record CreateVehiculoCommand(CreateVehiculoDto Vehiculo) : IRequest<VehiculoDto>;
