using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Cargo.Commands.AsignarCargo;

public record AsignarCargoCommand(AsignarCargoDto Data) : IRequest<PersonaCargoDto>;
