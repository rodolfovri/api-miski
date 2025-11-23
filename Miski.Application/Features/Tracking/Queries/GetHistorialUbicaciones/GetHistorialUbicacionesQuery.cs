using MediatR;
using Miski.Shared.DTOs.Tracking;

namespace Miski.Application.Features.Tracking.Queries.GetHistorialUbicaciones;

/// <summary>
/// Query para obtener el historial de ubicaciones de una persona
/// </summary>
public class GetHistorialUbicacionesQuery : IRequest<List<TrackingResponseDto>>
{
    public int IdPersona { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int? Limite { get; set; } = 100; // Máximo 100 registros por defecto
}
