using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Cargo.Commands.RevocarCargo;

public record RevocarCargoCommand(RevocarCargoDto Data) : IRequest<PersonaCargoDto>;
