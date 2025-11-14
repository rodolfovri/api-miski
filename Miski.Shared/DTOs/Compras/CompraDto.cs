namespace Miski.Shared.DTOs.Compras;

public class CompraDto
{
    public int IdCompra { get; set; }
    public int IdNegociacion { get; set; }
    public int? IdLote { get; set; }  // ? FK al lote (relación 1:1)
    public string? Serie { get; set; }
    public DateTime? FRegistro { get; set; }
    public DateTime? FEmision { get; set; }
    public string? Estado { get; set; }
    public string? EstadoRecepcion { get; set; }
    public string? EsParcial { get; set; }
    
    // Información de la negociación asociada
    public string? ProveedorNombre { get; set; }
    public string? ComisionistaNombre { get; set; }
    
    // Información del lote (relación 1:1)
    public decimal? PesoLote { get; set; }
    public int? SacosLote { get; set; }
    public string? CodigoLote { get; set; }
    public decimal? ComisionLote { get; set; }
    
    // Totales originales desde la Negociación
    public decimal NegociacionPesoTotal { get; set; }
    public int NegociacionSacosTotales { get; set; }
    
    public decimal PrecioUnitario { get; set; }
    public decimal? MontoTotal { get; set; }
    
    // ? Lote asociado a esta compra (1:1)
    public LoteDto? Lote { get; set; }
}

public class LoteDto
{
    public int IdLote { get; set; }
    public decimal Peso { get; set; }
    public int Sacos { get; set; }
    public string? Codigo { get; set; }
    public decimal? Comision { get; set; }
    public string? Observacion { get; set; }
    
    // ? Información de la compra asociada (desde la relación inversa)
    public int? IdCompra { get; set; }
    public string? CompraSerie { get; set; }
}

public class CreateLoteDto
{
    public decimal Peso { get; set; }
    public int Sacos { get; set; }
    public string? Codigo { get; set; }
    public decimal? Comision { get; set; }
    public string? Observacion { get; set; }
}

public class CreateLoteParaCompraDto
{
    public decimal Peso { get; set; }
    public int Sacos { get; set; }
    public string? Codigo { get; set; }
    public decimal? Comision { get; set; }
    public string? Observacion { get; set; }
    public decimal MontoTotal { get; set; }  // Monto total que se seteará en la Compra
}

public class UpdateLoteDto
{
    public int IdLote { get; set; }
    public decimal Peso { get; set; }
    public int Sacos { get; set; }
    public string? Codigo { get; set; }
    public decimal? Comision { get; set; }
    public string? Observacion { get; set; }
}

public class AsignarLoteACompraDto
{
    public int IdCompra { get; set; }
    public int IdLote { get; set; }
    public decimal MontoTotal { get; set; }  // Monto total que se seteará en la Compra
}

public class AnularCompraDto
{
    public int IdUsuarioAnulacion { get; set; }
    public string MotivoAnulacion { get; set; } = string.Empty;
}
