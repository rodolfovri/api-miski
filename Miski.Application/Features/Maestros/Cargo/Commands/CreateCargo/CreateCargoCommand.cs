using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Cargo.Commands.CreateCargo;

public record CreateCargoCommand(CreateCargoDto Cargo) : IRequest<CargoDto>;
