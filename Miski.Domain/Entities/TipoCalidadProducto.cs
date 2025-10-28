namespace Miski.Domain.Entities;

public class TipoCalidadProducto
{
    public int IdTipoCalidadProducto { get; set; }
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    // Navigation property
    public virtual Producto Producto { get; set; } = null!;
}
