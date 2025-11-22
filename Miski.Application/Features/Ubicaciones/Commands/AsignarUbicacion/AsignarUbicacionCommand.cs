using MediatR;
using Miski.Shared.DTOs.Ubicaciones;

namespace Miski.Application.Features.Ubicaciones.Commands.AsignarUbicacion;

public record AsignarUbicacionCommand(AsignarUbicacionDto Data) : IRequest<PersonaUbicacionDto>;
