using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Vehiculo.Queries.GetVehiculos;

public record GetVehiculosQuery(string? Placa = null, string? Estado = null) : IRequest<List<VehiculoDto>>;
