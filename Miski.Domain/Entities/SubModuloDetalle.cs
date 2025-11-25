namespace Miski.Domain.Entities;

public class SubModuloDetalle
{
    public int IdSubModuloDetalle { get; set; }
    public int IdSubModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Ruta { get; set; } // "/almacen/tablas-maestras/unidad-medida" - Opcional
    public string? Icono { get; set; }
    public int Orden { get; set; }
    public string Estado { get; set; } = string.Empty;
    // Navigation properties
    public virtual SubModulo SubModulo { get; set; } = null!;
    public virtual ICollection<PermisoRol> PermisoRoles { get; set; } = new List<PermisoRol>();
    public virtual ICollection<SubModuloDetalleAccion> SubModuloDetalleAcciones { get; set; } = new List<SubModuloDetalleAccion>();
}
