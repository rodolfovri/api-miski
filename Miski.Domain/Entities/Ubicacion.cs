namespace Miski.Domain.Entities;

public class Ubicacion
{
    public int IdUbicacion { get; set; }
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string? Tipo { get; set; }
    public string? Estado { get; set; }
    public DateTime FRegistro { get; set; } = DateTime.Now;

    // Navigation properties
    public virtual Usuario Usuario { get; set; } = null!;   
    public virtual ICollection<PersonaUbicacion> PersonaUbicaciones { get; set; } = new List<PersonaUbicacion>();
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}