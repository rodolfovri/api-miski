namespace Miski.Domain.Entities;

public class TrackingPersona
{
    public int IdTracking { get; set; }
    /// FK: Referencia a la persona siendo rastreada
    /// Ejemplo: 123 (personas que tienen ubicación)
    /// Relación: Muchos registros de tracking → Una persona
    public int IdPersona { get; set; }
    /// Latitud de la ubicación
    /// Precisión: 10,7 = permite 7 decimales (preciso hasta ~1 cm)
    /// Rango: -90 a 90
    /// Ejemplo: -6.7191234
    public string Latitud { get; set; } = string.Empty;
    /// Longitud de la ubicación
    /// Precisión: 10,7 = permite 7 decimales (preciso hasta ~1 cm)
    /// Rango: -180 a 180
    /// Ejemplo: -79.9065123
    public string Longitud { get; set; } = string.Empty;
    /// Timestamp cuando se registró la ubicación
    /// Se establece automáticamente a la hora actual del servidor (UTC)
    /// IMPORTANTE: Siempre usar DateTime.UtcNow en código
    /// Ejemplo: 2025-11-23 14:30:45.123
    public DateTime FRegistro { get; set; }
    /// Última vez que se actualizó este registro
    /// Se usa cuando se corrige una ubicación o se actualiza el estado
    /// Ejemplo: 2025-11-23 14:35:12.456
    public DateTime? FActualizacion { get; set; }
    /// Estado de la ubicación
    /// Valores: "ACTIVO", "INACTIVO", etc
    /// Máximo 20 caracteres
    /// Uso: Filtrar ubicaciones válidas
    /// Ejemplo: "ACTIVO"
    public string Estado { get; set; } = string.Empty;
    /// Flag que indica si esta es la ubicación más reciente
    /// Valores: true (ubicación actual) o false (histórico)
    /// Uso: Consultas rápidas sin necesidad de ORDER BY
    /// Ejemplo: true = esta es la ubicación donde está ahora la persona
    /// IMPORTANTE: Solo UN registro por persona debe tener EsActual = true
    public bool EsActual { get; set; }
    /// Precisión del GPS en metros
    /// Viene del dispositivo móvil
    /// Valores típicos: 5-50 metros
    /// Ejemplo: 15.5 (significa ±15.5 metros)
    /// Uso: Evaluar confiabilidad de la lectura
    public decimal? Precision { get; set; }
    /// Velocidad actual del usuario en km/h
    /// Viene del dispositivo móvil (si está en movimiento)
    /// Ejemplo: 0 (parado), 45.5 (en movimiento)
    /// Uso: Detectar si está en auto, a pie, etc.
    public decimal? Velocidad { get; set; }


    // Navigation properties
    public virtual Persona Persona { get; set; } = null!;
}
