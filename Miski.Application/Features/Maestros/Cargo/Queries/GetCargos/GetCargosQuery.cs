using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Cargo.Queries.GetCargos;

public record GetCargosQuery(string? Estado = null) : IRequest<List<CargoDto>>;
