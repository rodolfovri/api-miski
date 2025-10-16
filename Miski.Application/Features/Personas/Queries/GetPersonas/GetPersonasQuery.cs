using MediatR;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.Queries.GetPersonas;

public record GetPersonasQuery(
    string? NumeroDocumento = null,
    string? Nombres = null,
    string? Estado = null
) : IRequest<List<PersonaDto>>;