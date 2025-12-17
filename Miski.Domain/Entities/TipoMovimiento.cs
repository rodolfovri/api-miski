namespace Miski.Domain.Entities;

public class TipoMovimiento
{
    public int IdTipoMovimiento { get; set; }
    public string TipoOperacion { get; set; } = string.Empty; // "INGRESO" o "SALIDA"
    public string Descripcion { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}
