namespace Miski.Domain.Entities;

public class Modulo
{
    public int IdModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Orden { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string TipoPlataforma { get; set; } = string.Empty;
    // Navigation properties
    public virtual ICollection<SubModulo> SubModulos { get; set; } = new List<SubModulo>();
    public virtual ICollection<PermisoRol> PermisoRoles { get; set; } = new List<PermisoRol>();
}
