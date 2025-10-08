namespace Miski.Domain.Entities;

public class Lote
{
    public int IdLote { get; set; }
    public int IdCompra { get; set; }
    public int NumeroLote { get; set; }
    public decimal Peso { get; set; }
    public int Sacos { get; set; }
    public string? Codigo { get; set; }
    public string? Grado { get; set; }

    // Navigation properties
    public virtual Compra Compra { get; set; } = null!;
    public virtual ICollection<LlegadaPlantaDetalle> LlegadaPlantaDetalles { get; set; } = new List<LlegadaPlantaDetalle>();
}