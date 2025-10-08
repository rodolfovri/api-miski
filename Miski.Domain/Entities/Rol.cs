namespace Miski.Domain.Entities;

public class Rol
{
    public int IdRol { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? TipoPlataforma { get; set; } // Ejemplo: "Web", "Mobile", etc.

    // Navigation properties
    public virtual ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
    public virtual ICollection<PermisoRol> PermisoRoles { get; set; } = new List<PermisoRol>();
}