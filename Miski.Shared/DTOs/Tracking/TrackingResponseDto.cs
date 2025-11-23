namespace Miski.Shared.DTOs.Tracking;

/// <summary>
/// DTO de respuesta para ubicación de tracking
/// </summary>
public class TrackingResponseDto
{
    public int IdTracking { get; set; }
    public int IdPersona { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string NumeroDocumento { get; set; } = string.Empty;
    public string Latitud { get; set; } = string.Empty;
    public string Longitud { get; set; } = string.Empty;
    public decimal? Precision { get; set; }
    public decimal? Velocidad { get; set; }
    public DateTime FRegistro { get; set; }
    public bool EsActual { get; set; }
    public string Estado { get; set; } = string.Empty;
}
