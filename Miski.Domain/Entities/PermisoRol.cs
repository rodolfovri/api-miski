namespace Miski.Domain.Entities;

public class PermisoRol
{
    public int IdPermisoRol { get; set; }
    public int IdRol { get; set; }
    public int? IdModulo { get; set; }
    public int? IdSubModulo { get; set; }
    public int? IdSubModuloDetalle { get; set; }
    public bool TieneAcceso { get; set; }
    // Navegación
    public Rol? Rol { get; set; } = null;
    public Modulo? Modulo { get; set; } = null;
    public SubModulo? SubModulo { get; set; } = null;
    public SubModuloDetalle? SubModuloDetalle { get; set; } = null;
    public virtual ICollection<PermisoRolAccion> PermisoRolAcciones { get; set; } = new List<PermisoRolAccion>();
}
