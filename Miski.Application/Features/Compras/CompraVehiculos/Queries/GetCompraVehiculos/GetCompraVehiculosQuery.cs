using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.CompraVehiculos.Queries.GetCompraVehiculos;

public record GetCompraVehiculosQuery(
    int? IdVehiculo = null,
    string? GuiaRemision = null
) : IRequest<IEnumerable<CompraVehiculoDto>>;
