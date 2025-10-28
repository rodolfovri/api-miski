namespace Miski.Domain.Entities;

public class UnidadMedida
{
    public int IdUnidadMedida { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Abreviatura { get; set; } = string.Empty;
    // Navigation properties
    public virtual ICollection<VariedadProducto> VariedadProductos { get; set; } = new List<VariedadProducto>();
}
