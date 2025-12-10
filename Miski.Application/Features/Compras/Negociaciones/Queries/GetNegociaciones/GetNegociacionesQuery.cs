using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Queries.GetNegociaciones;

public record GetNegociacionesQuery(
    int? IdComisionista = null,
    DateTime? FechaDesde = null,
    DateTime? FechaHasta = null
) : IRequest<List<NegociacionDto>>;
