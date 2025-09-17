namespace Miski.Shared.DTOs;

public class ProductoDto
{
    public int IdProducto { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? UnidadMedida { get; set; }
    public decimal? PesoPorSaco { get; set; }
    public string? Estado { get; set; }
    public DateTime FRegistro { get; set; }
}