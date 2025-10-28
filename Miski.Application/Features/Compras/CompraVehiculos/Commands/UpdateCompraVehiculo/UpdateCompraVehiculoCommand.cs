using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.CompraVehiculos.Commands.UpdateCompraVehiculo;

public record UpdateCompraVehiculoCommand(int Id, UpdateCompraVehiculoDto CompraVehiculo) : IRequest<CompraVehiculoDto>;
