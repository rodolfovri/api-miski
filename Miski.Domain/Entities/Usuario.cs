using Miski.Domain.Entities.Base;

namespace Miski.Domain.Entities;

public class Usuario : BaseEntity
{
    public int IdUsuario { get; set; }
    public int? IdPersona { get; set; }
    public string Username { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public int IdRol { get; set; }
    public string? Estado { get; set; }
    public DateTime FRegistro { get; set; } = DateTime.Now;

    // Navigation properties
    public virtual Persona? Persona { get; set; }
    public virtual Rol Rol { get; set; } = null!;
}