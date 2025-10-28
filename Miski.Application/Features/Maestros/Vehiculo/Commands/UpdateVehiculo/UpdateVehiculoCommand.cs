using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Vehiculo.Commands.UpdateVehiculo;

public record UpdateVehiculoCommand(int Id, UpdateVehiculoDto Vehiculo) : IRequest<VehiculoDto>;
