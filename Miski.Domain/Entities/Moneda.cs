namespace Miski.Domain.Entities;

public class Moneda
{
    public int IdMoneda { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Simbolo { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    // Navigation properties
    public virtual ICollection<TipoCambio> TipoCambios { get; set; } = new List<TipoCambio>();
    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
