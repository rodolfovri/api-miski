namespace Miski.Domain.Entities;

public class Accion
{
    public int IdAccion { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Icono { get; set; } = string.Empty;
    public int Orden { get; set; }
    public string Estado { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<PermisoRolAccion> PermisoRolAcciones { get; set; } = new List<PermisoRolAccion>();
}
