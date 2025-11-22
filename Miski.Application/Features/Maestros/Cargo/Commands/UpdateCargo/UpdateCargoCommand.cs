using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Cargo.Commands.UpdateCargo;

public record UpdateCargoCommand(int Id, UpdateCargoDto Cargo) : IRequest<CargoDto>;
