namespace Miski.Shared.DTOs.Tracking;

/// <summary>
/// DTO para registrar ubicación desde dispositivo SIN autenticación (background tracking)
/// Usado cuando la app está cerrada o el usuario no tiene sesión activa
/// </summary>
public class UbicacionDispositivoDto
{
    /// <summary>
    /// Identificador único del dispositivo (UUID)
    /// </summary>
    public string DeviceId { get; set; } = string.Empty;
    
    /// <summary>
    /// ID de la persona asociada al dispositivo
    /// </summary>
    public int IdPersona { get; set; }
    
    /// <summary>
    /// Latitud en formato string (para precisión)
    /// </summary>
    public string Latitud { get; set; } = string.Empty;
    
    /// <summary>
    /// Longitud en formato string (para precisión)
    /// </summary>
    public string Longitud { get; set; } = string.Empty;
    
    /// <summary>
    /// Precisión del GPS en metros (opcional)
    /// </summary>
    public decimal? Precision { get; set; }
    
    /// <summary>
    /// Velocidad en km/h (opcional)
    /// </summary>
    public decimal? Velocidad { get; set; }
}
