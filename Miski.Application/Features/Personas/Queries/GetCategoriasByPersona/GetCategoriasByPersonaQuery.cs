using MediatR;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.Queries.GetCategoriasByPersona;

public record GetCategoriasByPersonaQuery(int PersonaId) : IRequest<List<CategoriaPersonaDto>>;