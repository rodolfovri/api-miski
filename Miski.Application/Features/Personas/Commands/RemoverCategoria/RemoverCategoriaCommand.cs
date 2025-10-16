using MediatR;

namespace Miski.Application.Features.Personas.Commands.RemoverCategoria;

public record RemoverCategoriaCommand(int PersonaId, int CategoriaId) : IRequest<bool>;