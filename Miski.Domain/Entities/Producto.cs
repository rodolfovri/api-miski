namespace Miski.Domain.Entities;

public class Producto
{
    public int IdProducto { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? UnidadMedida { get; set; }
    public decimal? PesoPorSaco { get; set; }
    public string? Estado { get; set; }
    public DateTime FRegistro { get; set; } = DateTime.Now;

    // Navigation properties
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    public virtual ICollection<Negociacion> Negociaciones { get; set; } = new List<Negociacion>();
    public virtual ICollection<LlegadaPlantaDetalle> LlegadaPlantaDetalles { get; set; } = new List<LlegadaPlantaDetalle>();
}