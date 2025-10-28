using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Queries.GetNegociaciones;

public record GetNegociacionesQuery(
    int? IdComisionista = null,
    int? IdVariedadProducto = null,  // CAMBIADO de IdProducto
    string? Estado = null,
    string? EstadoAprobacionIngeniero = null
) : IRequest<List<NegociacionDto>>;
