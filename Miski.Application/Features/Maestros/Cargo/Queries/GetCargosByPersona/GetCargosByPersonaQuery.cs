using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.Cargo.Queries.GetCargosByPersona;

public record GetCargosByPersonaQuery(int IdPersona, bool? SoloActuales = null) : IRequest<List<PersonaCargoDto>>;
