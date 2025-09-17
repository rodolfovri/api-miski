using Miski.Shared.DTOs.Base;

namespace Miski.Shared.DTOs;

public class NegociacionDto : BaseDto
{
    public int IdNegociacion { get; set; }
    public int? IdProveedor { get; set; }
    public string? ProveedorNombre { get; set; }
    public int IdComisionista { get; set; }
    public string? ComisionistaNombre { get; set; }
    public int? IdProducto { get; set; }
    public string? ProductoNombre { get; set; }
    public DateTime Fecha { get; set; }
    public decimal PesoTotal { get; set; }
    public int SacosTotales { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal MontoTotal { get; set; }
    public string NroCuentaRuc { get; set; } = string.Empty;
    public string FotoCalidadProducto { get; set; } = string.Empty;
    public string FotoDniFrontal { get; set; } = string.Empty;
    public string FotoDniPosterior { get; set; } = string.Empty;
    public string? EstadoAprobado { get; set; }
    public int? AprobadaPor { get; set; }
    public string? AprobadaPorNombre { get; set; }
    public DateTime? FechaAprobacion { get; set; }
    public string? Observacion { get; set; }
    public string? Estado { get; set; }
}