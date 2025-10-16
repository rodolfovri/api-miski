using MediatR;
using Miski.Shared.DTOs.Ubicaciones;

namespace Miski.Application.Features.Ubicaciones.Commands.CreateUbicacion;

public record CreateUbicacionCommand(CreateUbicacionDto Ubicacion) : IRequest<UbicacionDto>;