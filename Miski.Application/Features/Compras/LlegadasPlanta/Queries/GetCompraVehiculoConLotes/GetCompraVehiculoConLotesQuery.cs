using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.LlegadasPlanta.Queries.GetCompraVehiculoConLotes;

public record GetCompraVehiculoConLotesQuery(int IdCompraVehiculo) : IRequest<CompraVehiculoConLotesDto>;
