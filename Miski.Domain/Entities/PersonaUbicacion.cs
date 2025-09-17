using Miski.Domain.Entities.Base;

namespace Miski.Domain.Entities;

public class PersonaUbicacion : BaseEntity
{
    public int IdPersonaUbicacion { get; set; }
    public int IdPersona { get; set; }
    public int IdUbicacion { get; set; }
    public DateTime FRegistro { get; set; } = DateTime.Now;
    public string? Estado { get; set; }

    // Navigation properties
    public virtual Persona Persona { get; set; } = null!;
    public virtual Ubicacion Ubicacion { get; set; } = null!;
}