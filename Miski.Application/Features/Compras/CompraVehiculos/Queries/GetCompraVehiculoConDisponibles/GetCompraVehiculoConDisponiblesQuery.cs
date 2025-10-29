using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.CompraVehiculos.Queries.GetCompraVehiculoConDisponibles;

public record GetCompraVehiculoConDisponiblesQuery(int Id) : IRequest<CompraVehiculoConDisponiblesDto>;
