using MediatR;

namespace Miski.Application.Features.Personas.CategoriaPersona.Commands.DeleteCategoria;

public record DeleteCategoriaPersonaCommand(int Id) : IRequest<bool>;