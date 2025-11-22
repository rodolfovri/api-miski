using MediatR;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.CategoriaPersona.Commands.AsignarCategoria;

public record AsignarCategoriaCommand(AsignarCategoriaDto Data) : IRequest<PersonaCategoriaDto>;
