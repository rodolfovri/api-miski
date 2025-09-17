using Miski.Domain.Entities.Base;

namespace Miski.Domain.Entities;

public class Ubicacion : BaseEntity
{
    public int IdUbicacion { get; set; }
    public int IdPersona { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public string? Tipo { get; set; }
    public string? Estado { get; set; }
    public DateTime FRegistro { get; set; } = DateTime.Now;

    // Navigation properties
    public virtual Persona Persona { get; set; } = null!;
    public virtual ICollection<PersonaUbicacion> PersonaUbicaciones { get; set; } = new List<PersonaUbicacion>();
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}