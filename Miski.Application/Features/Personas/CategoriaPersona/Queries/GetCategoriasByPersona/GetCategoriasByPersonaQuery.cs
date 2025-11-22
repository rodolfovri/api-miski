using MediatR;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.CategoriaPersona.Queries.GetCategoriasByPersona;

public record GetCategoriasByPersonaQuery(int IdPersona) : IRequest<List<PersonaCategoriaDto>>;
