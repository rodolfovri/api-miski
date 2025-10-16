namespace Miski.Domain.Entities;

public class CategoriaProducto
{
    public int IdCategoriaProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Estado { get; set; }
    public DateTime FRegistro { get; set; } = DateTime.Now;
    // Navigation properties
    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
