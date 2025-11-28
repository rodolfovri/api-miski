using MediatR;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.Queries.GetPersonasByCategoria;

public record GetPersonasByCategoriaQuery(
    int IdCategoria,
    string? Estado = null
) : IRequest<List<PersonaDto>>;
