using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.ConfiguracionGlobal.Queries.GetConfiguracionById;

public class GetConfiguracionByIdQuery : IRequest<ConfiguracionGlobalDto>
{
    public int IdConfiguracionGlobal { get; set; }
}
