using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.CompraVehiculos.Queries.GetCompraVehiculoById;

public record GetCompraVehiculoByIdQuery(int Id) : IRequest<CompraVehiculoDto>;
