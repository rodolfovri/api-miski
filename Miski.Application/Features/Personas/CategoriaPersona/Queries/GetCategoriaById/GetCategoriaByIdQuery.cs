using MediatR;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.CategoriaPersona.Queries.GetCategoriaById;

public record GetCategoriaByIdQuery(int Id) : IRequest<CategoriaPersonaDto>;