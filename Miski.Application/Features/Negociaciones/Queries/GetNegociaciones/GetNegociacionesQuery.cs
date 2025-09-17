using MediatR;
using Miski.Shared.DTOs;

namespace Miski.Application.Features.Negociaciones.Queries.GetNegociaciones;

public record GetNegociacionesQuery(
    int? ProveedorId = null, 
    int? ComisionistaId = null, 
    string? Estado = null
) : IRequest<IEnumerable<NegociacionDto>>;