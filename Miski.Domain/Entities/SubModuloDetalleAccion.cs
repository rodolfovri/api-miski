namespace Miski.Domain.Entities;

public class SubModuloDetalleAccion
{
    public int IdSubModuloDetalleAccion { get; set; }
    public int IdSubModuloDetalle { get; set; }
    public int IdAccion { get; set; }
    public bool Habilitado { get; set; }
    // Navigation properties
    public virtual SubModuloDetalle SubModuloDetalle { get; set; } = null!;
    public virtual Accion Accion { get; set; } = null!;
}
