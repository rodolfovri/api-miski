using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.ConfiguracionGlobal.Commands.UpdateConfiguracion;

public class UpdateConfiguracionCommand : IRequest<ConfiguracionGlobalDto>
{
    public UpdateConfiguracionGlobalDto ConfiguracionData { get; set; } = null!;
}
