using MediatR;
using Miski.Shared.DTOs.Tracking;

namespace Miski.Application.Features.Tracking.Queries.GetUbicacionActual;

/// <summary>
/// Query para obtener la ubicación actual de una persona
/// </summary>
public class GetUbicacionActualQuery : IRequest<TrackingResponseDto?>
{
    public int IdPersona { get; set; }
}
