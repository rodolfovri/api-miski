using MediatR;

namespace Miski.Application.Features.Maestros.Cargo.Commands.DeleteCargo;

public record DeleteCargoCommand(int Id) : IRequest;
