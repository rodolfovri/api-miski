using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.ConfiguracionGlobal.Queries.GetConfiguracionByClave;

public class GetConfiguracionByClaveQuery : IRequest<ConfiguracionGlobalDto>
{
    public string Clave { get; set; } = string.Empty;
}
