using System.Collections.ObjectModel;

namespace Miski.Domain.Entities;

public class Usuario
{
    public int IdUsuario { get; set; }
    public int? IdPersona { get; set; }
    public string Username { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public string? Estado { get; set; }
    public DateTime FRegistro { get; set; } = DateTime.Now;

    // Navigation properties
    public virtual Persona? Persona { get; set; }
    public virtual Collection<TipoCambio> TipoCambios { get; set; } = new Collection<TipoCambio>();
    public virtual Collection<UsuarioRol> UsuarioRoles { get; set; } = new Collection<UsuarioRol>();
}