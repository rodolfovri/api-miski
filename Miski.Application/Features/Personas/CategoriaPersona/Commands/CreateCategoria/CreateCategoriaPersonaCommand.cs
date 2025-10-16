using MediatR;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.CategoriaPersona.Commands.CreateCategoria;

public record CreateCategoriaPersonaCommand(CreateCategoriaPersonaDto Categoria) : IRequest<CategoriaPersonaDto>;