namespace Miski.Shared.DTOs.Tracking;

/// <summary>
/// DTO para registrar ubicación con usuario autenticado
/// Usado cuando el usuario está activo en la app
/// </summary>
public class RegistrarUbicacionDto
{
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
