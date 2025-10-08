namespace Miski.Domain.Entities;

public class TrackingPersona
{
    public int IdTracking { get; set; }
    public int IdPersona { get; set; }
    public string Latitud { get; set; } = string.Empty;
    public string Longitud { get; set; } = string.Empty;
    public DateTime FRegistro { get; set; }

    // Navigation properties
    public virtual Persona Persona { get; set; } = null!;
}
