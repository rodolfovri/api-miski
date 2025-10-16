using MediatR;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.CategoriaPersona.Commands.UpdateCategoria;

public record UpdateCategoriaPersonaCommand(int Id, UpdateCategoriaPersonaDto Categoria) : IRequest<CategoriaPersonaDto>;