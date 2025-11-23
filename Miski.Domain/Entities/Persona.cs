namespace Miski.Domain.Entities;

public class Persona
{
    public int IdPersona { get; set; }
    public int IdTipoDocumento { get; set; }
    public string NumeroDocumento { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
    public string? Estado { get; set; }
    public DateTime FRegistro { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual TipoDocumento TipoDocumento { get; set; } = null!;
    public virtual Usuario? Usuario { get; set; }
    public virtual ICollection<PersonaCategoria> PersonaCategorias { get; set; } = new List<PersonaCategoria>();
    public virtual ICollection<PersonaUbicacion> PersonaUbicaciones { get; set; } = new List<PersonaUbicacion>();
    public virtual ICollection<TrackingPersona> TrackingPersonas { get; set; } = new List<TrackingPersona>();
    public virtual ICollection<CompraVehiculo> CompraVehiculos { get; set; } = new List<CompraVehiculo>();
    public virtual ICollection<PersonaCargo> PersonaCargos { get; set; } = new List<PersonaCargo>();
    public virtual ICollection<DispositivoPersona> DispositivoPersonas { get; set; } = new List<DispositivoPersona>();
}