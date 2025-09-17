using Miski.Domain.Entities.Base;

namespace Miski.Domain.Entities;

public class Rol : BaseEntity
{
    public int IdRol { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    // Navigation properties
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}