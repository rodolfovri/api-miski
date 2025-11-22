namespace Miski.Domain.Entities;

public class PersonaUbicacion
{
    public int IdPersonaUbicacion { get; set; }
    public int IdPersona { get; set; }
    public int IdUbicacion { get; set; }
    public DateTime FRegistro { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Persona Persona { get; set; } = null!;
    public virtual Ubicacion Ubicacion { get; set; } = null!;
}