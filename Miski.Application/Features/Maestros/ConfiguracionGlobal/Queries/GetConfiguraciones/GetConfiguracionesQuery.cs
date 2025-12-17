using MediatR;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.ConfiguracionGlobal.Queries.GetConfiguraciones;

public class GetConfiguracionesQuery : IRequest<List<ConfiguracionGlobalDto>>
{
}
