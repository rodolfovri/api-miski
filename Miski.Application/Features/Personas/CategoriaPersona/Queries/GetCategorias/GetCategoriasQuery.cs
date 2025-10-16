using MediatR;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.CategoriaPersona.Queries.GetCategorias;

public record GetCategoriasQuery(string? Nombre = null) : IRequest<List<CategoriaPersonaDto>>;