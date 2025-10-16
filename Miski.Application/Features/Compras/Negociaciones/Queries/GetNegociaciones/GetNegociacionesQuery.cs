using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Queries.GetNegociaciones;

public record GetNegociacionesQuery(
    int? IdProveedor = null,
    int? IdComisionista = null,
    int? IdProducto = null,
    string? EstadoAprobado = null,
    string? Estado = null
) : IRequest<List<NegociacionDto>>;
