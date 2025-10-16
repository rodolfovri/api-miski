using MediatR;

namespace Miski.Application.Features.Personas.Commands.DeletePersona;

public record DeletePersonaCommand(int Id) : IRequest<bool>;