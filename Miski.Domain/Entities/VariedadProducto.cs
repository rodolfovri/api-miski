namespace Miski.Domain.Entities;

public class VariedadProducto
{
    public int IdVariedadProducto { get; set; }
    public int IdProducto { get; set; }
    public int IdUnidadMedida { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Estado { get; set; }
    public string? FichaTecnica { get; set; }
    public DateTime FRegistro { get; set; } = DateTime.Now;
    // Navigation properties
    public virtual Producto Producto { get; set; } = null!;
    public virtual UnidadMedida UnidadMedida { get; set; } = null!;
    public virtual ICollection<Negociacion> Negociaciones { get; set; } = new List<Negociacion>();

}
