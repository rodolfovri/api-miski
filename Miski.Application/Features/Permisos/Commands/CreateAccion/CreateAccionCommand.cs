using MediatR;
using Miski.Shared.DTOs.Permisos;

namespace Miski.Application.Features.Permisos.Commands.CreateAccion;

public class CreateAccionCommand : IRequest<AccionDto>
{
    public CreateAccionDto Data { get; set; } = null!;
}
