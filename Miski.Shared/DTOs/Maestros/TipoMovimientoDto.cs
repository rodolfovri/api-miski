namespace Miski.Shared.DTOs.Maestros;

public class TipoMovimientoDto
{
    public int IdTipoMovimiento { get; set; }
    public string TipoOperacion { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}

public class CreateTipoMovimientoDto
{
    public string TipoOperacion { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Estado { get; set; } = "ACTIVO";
}

public class UpdateTipoMovimientoDto
{
    public int IdTipoMovimiento { get; set; }
    public string TipoOperacion { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}
