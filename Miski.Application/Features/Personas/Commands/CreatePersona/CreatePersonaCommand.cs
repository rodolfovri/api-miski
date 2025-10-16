using MediatR;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.Commands.CreatePersona;

public record CreatePersonaCommand(CreatePersonaDto Persona) : IRequest<PersonaDto>;