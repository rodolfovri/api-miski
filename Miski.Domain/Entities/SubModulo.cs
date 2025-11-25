namespace Miski.Domain.Entities;

public class SubModulo
{
    public int IdSubModulo { get; set; }
    public int IdModulo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Ruta { get; set; } // "/almacen/control-productos" - Opcional
    public string? Icono { get; set; } 
    public int Orden { get; set; }
    public string Estado { get; set; } = string.Empty;
    public bool TieneDetalles { get; set; } // true si tiene SubModuloDetalles, false si tiene acciones directamente

    // Navigation properties
    public virtual Modulo Modulo { get; set; } = null!;
    public virtual ICollection<SubModuloDetalle> SubModuloDetalles { get; set; } = new List<SubModuloDetalle>();
    public virtual ICollection<PermisoRol> PermisoRoles { get; set; } = new List<PermisoRol>();
}
