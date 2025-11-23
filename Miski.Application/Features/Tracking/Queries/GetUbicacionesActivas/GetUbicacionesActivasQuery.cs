using MediatR;
using Miski.Shared.DTOs.Tracking;

namespace Miski.Application.Features.Tracking.Queries.GetUbicacionesActivas;

/// <summary>
/// Query para obtener todas las ubicaciones activas (últimas posiciones de todas las personas)
/// Útil para mostrar en un mapa en tiempo real
/// </summary>
public class GetUbicacionesActivasQuery : IRequest<List<TrackingResponseDto>>
{
}
