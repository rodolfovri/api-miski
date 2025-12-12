using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Compras.Queries.GetCompras;

public record GetComprasQuery(
    DateTime? FechaDesde = null,
    DateTime? FechaHasta = null
) : IRequest<List<CompraDto>>;
