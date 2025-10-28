namespace Miski.Domain.Entities;

public class TipoCambio
{
    public int IdTipoCambio { get; set; }
    public int IdMoneda { get; set; }
    public int IdUsuario { get; set; }
    public decimal ValorCompra { get; set; }
    public decimal ValorVenta { get; set; }
    public DateTime FRegistro { get; set; }
    // Navigation properties
    public Moneda? Moneda { get; set; }
    public Usuario? Usuario { get; set; }
    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
