using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.ConfiguracionGlobal.Commands.CreateConfiguracion;

public class CreateConfiguracionCommand : IRequest<ConfiguracionGlobalDto>
{
    public CreateConfiguracionGlobalDto ConfiguracionData { get; set; } = null!;
}
