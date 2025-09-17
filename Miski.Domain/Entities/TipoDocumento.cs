namespace Miski.Domain.Entities;

public class TipoDocumento
{
    public int IdTipoDocumento { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int? LongitudMin { get; set; }
    public int? LongitudMax { get; set; }

    // Navigation properties
    public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();
}