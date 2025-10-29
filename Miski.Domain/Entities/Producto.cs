namespace Miski.Domain.Entities;

public class Producto
{
    public int IdProducto { get; set; }
    public int IdCategoriaProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Estado { get; set; }
    public string? Imagen { get; set; }
    public string? FichaTecnica { get; set; }
    public DateTime FRegistro { get; set; } = DateTime.Now;

    // Navigation properties
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    public virtual CategoriaProducto CategoriaProducto { get; set; } = null!;
    public virtual ICollection<VariedadProducto> VariedadProductos { get; set; } = new List<VariedadProducto>();
    public virtual ICollection<TipoCalidadProducto> TipoCalidadProductos { get; set; } = new List<TipoCalidadProducto>();
}