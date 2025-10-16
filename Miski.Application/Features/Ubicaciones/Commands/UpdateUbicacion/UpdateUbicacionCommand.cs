using MediatR;
using Miski.Shared.DTOs.Ubicaciones;

namespace Miski.Application.Features.Ubicaciones.Commands.UpdateUbicacion;

public record UpdateUbicacionCommand(int Id, UpdateUbicacionDto Ubicacion) : IRequest<UbicacionDto>;