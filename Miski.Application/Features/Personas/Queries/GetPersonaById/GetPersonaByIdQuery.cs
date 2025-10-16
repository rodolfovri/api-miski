using MediatR;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.Queries.GetPersonaById;

public record GetPersonaByIdQuery(int Id) : IRequest<PersonaDto>;