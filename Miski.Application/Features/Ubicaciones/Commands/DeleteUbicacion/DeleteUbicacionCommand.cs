using MediatR;

namespace Miski.Application.Features.Ubicaciones.Commands.DeleteUbicacion;

public record DeleteUbicacionCommand(int Id) : IRequest<bool>;