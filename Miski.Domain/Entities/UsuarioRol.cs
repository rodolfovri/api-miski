namespace Miski.Domain.Entities;

public class UsuarioRol
{
    public int IdUsuarioRol { get; set; }
    public int IdUsuario { get; set; }
    public int IdRol { get; set; }
    public DateTime FRegistro { get; set; } = DateTime.UtcNow;
    // Navigation properties
    public virtual Usuario Usuario { get; set; } = null!;
    public virtual Rol Rol { get; set; } = null!;
}

