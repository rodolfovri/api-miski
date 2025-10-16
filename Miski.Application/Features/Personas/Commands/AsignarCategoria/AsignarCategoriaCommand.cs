using MediatR;

namespace Miski.Application.Features.Personas.Commands.AsignarCategoria;

public record AsignarCategoriaCommand(int PersonaId, int CategoriaId) : IRequest<bool>;