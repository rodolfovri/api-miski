using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Vehiculo.Queries.GetVehiculoById;

public record GetVehiculoByIdQuery(int Id) : IRequest<VehiculoDto>;
