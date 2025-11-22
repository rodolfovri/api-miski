namespace Miski.Domain.Entities;

public class PersonaCargo
{
    public int IdPersonaCargo { get; set; }
    public int IdPersona { get; set; }
    public int IdCargo { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; } // NULL = cargo actual
    public bool EsActual { get; set; } // true para el cargo vigente
    public string? ObservacionAsignacion { get; set; }
    public string? MotivoRevocacion { get; set; }

    // Navigation properties
    public virtual Persona Persona { get; set; } = null!;
    public virtual Cargo Cargo { get; set; } = null!;
}
