using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.CompraVehiculos.Queries.GetCompraVehiculos;

public record GetCompraVehiculosQuery(
    DateTime? FechaDesde = null,
    DateTime? FechaHasta = null
) : IRequest<IEnumerable<CompraVehiculoDto>>;
