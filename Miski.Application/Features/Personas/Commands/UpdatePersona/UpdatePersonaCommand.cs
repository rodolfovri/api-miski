using MediatR;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.Commands.UpdatePersona;

public record UpdatePersonaCommand(int Id, UpdatePersonaDto Persona) : IRequest<PersonaDto>;