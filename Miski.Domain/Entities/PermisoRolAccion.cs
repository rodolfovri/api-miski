namespace Miski.Domain.Entities;

public class PermisoRolAccion
{
    public int IdPermisoRolAccion { get; set; }
    public int IdPermisoRol { get; set; }
    public int IdAccion { get; set; }
    public bool Habilitado { get; set; } // true si el rol tiene permiso para esta acción

    // Navegation properties
    public virtual PermisoRol PermisoRol { get; set; } = null!;
    public virtual Accion Accion { get; set; } = null!;
}
