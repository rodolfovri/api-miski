using MediatR;
using Miski.Shared.DTOs.Ubicaciones;

namespace Miski.Application.Features.Ubicaciones.Commands.RevocarUbicacion;

public record RevocarUbicacionCommand(RevocarUbicacionDto Data) : IRequest<Unit>;
