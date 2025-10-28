using MediatR;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.CompraVehiculos.Commands.CreateCompraVehiculo;

public record CreateCompraVehiculoCommand(CreateCompraVehiculoDto CompraVehiculo) : IRequest<CompraVehiculoDto>;
