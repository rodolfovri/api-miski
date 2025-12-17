using MediatR;

namespace Miski.Application.Features.Maestros.ConfiguracionGlobal.Commands.DeleteConfiguracion;

public class DeleteConfiguracionCommand : IRequest<bool>
{
    public int IdConfiguracionGlobal { get; set; }
}
