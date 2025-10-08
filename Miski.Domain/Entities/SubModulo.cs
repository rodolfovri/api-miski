namespace Miski.Domain.Entities;

public class SubModulo
{
    public int IdSubModulo { get; set; }
    public int IdModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Orden { get; set; }
    public string Estado { get; set; } = string.Empty;
    // Navigation properties
    public virtual Modulo Modulo { get; set; } = null!;
    public virtual ICollection<SubModuloDetalle> SubModuloDetalles { get; set; } = new List<SubModuloDetalle>();
    public virtual ICollection<PermisoRol> PermisoRoles { get; set; } = new List<PermisoRol>();
}
