namespace Miski.Shared.DTOs.Compras;

// DTO para LlegadaPlanta
public class LlegadaPlantaDto
{
    public int IdLlegadaPlanta { get; set; }
    public int IdCompra { get; set; }
    public int IdUsuario { get; set; }
    public DateTime? FLlegada { get; set; }
    public string? Observaciones { get; set; }
    public string? Estado { get; set; }
    
    // Informaci�n adicional
    public string? CompraSerie { get; set; }
    public string? UsuarioNombre { get; set; }
    
    // Detalles de la llegada
    public List<LlegadaPlantaDetalleDto> Detalles { get; set; } = new List<LlegadaPlantaDetalleDto>();
}

// DTO para LlegadaPlantaDetalle
public class LlegadaPlantaDetalleDto
{
    public int IdLlegadaDetalle { get; set; }
    public int IdLlegadaPlanta { get; set; }
    public int IdLote { get; set; }
    public int SacosRecibidos { get; set; }
    public decimal PesoRecibido { get; set; }
    public string? Observaciones { get; set; }
    
    // Informaci�n adicional del lote
    public string? LoteCodigo { get; set; }
    public int SacosAsignados { get; set; }  // Sacos originales del lote
    public decimal PesoAsignado { get; set; }  // Peso original del lote
    public int DiferenciaSacos { get; set; }  // SacosAsignados - SacosRecibidos
    public decimal DiferenciaPeso { get; set; }  // PesoAsignado - PesoRecibido
}

// DTO para crear LlegadaPlanta
public class CreateLlegadaPlantaDto
{
    public int IdCompra { get; set; }
    public int IdUsuario { get; set; }
    public DateTime? FLlegada { get; set; }
    public string? Observaciones { get; set; }
    
    // Detalles de lotes recibidos
    public List<CreateLlegadaPlantaDetalleDto> Detalles { get; set; } = new List<CreateLlegadaPlantaDetalleDto>();
}

// DTO para detalle de creaci�n
public class CreateLlegadaPlantaDetalleDto
{
    public int IdLote { get; set; }
    public int SacosRecibidos { get; set; }
    public decimal PesoRecibido { get; set; }
    public string? Observaciones { get; set; }
}

// DTO para compras con lotes por CompraVehiculo
public class CompraVehiculoConLotesDto
{
    public int IdCompraVehiculo { get; set; }
    public int IdVehiculo { get; set; }
    public string GuiaRemision { get; set; } = string.Empty;
    public DateTime FRegistro { get; set; }
    
    // Informaci�n del veh�culo
    public string? VehiculoPlaca { get; set; }
    public string? VehiculoMarca { get; set; }
    public string? VehiculoModelo { get; set; }
    
    // Compras con sus lotes
    public List<CompraConLotesDto> Compras { get; set; } = new List<CompraConLotesDto>();
}

// DTO para compra con lotes
public class CompraConLotesDto
{
    public int IdCompra { get; set; }
    public string? Serie { get; set; }
    public DateTime? FRegistro { get; set; }
    
    // Lotes de esta compra
    public List<LoteConRecepcionDto> Lotes { get; set; } = new List<LoteConRecepcionDto>();
}

// DTO para lote con informaci�n de recepci�n
public class LoteConRecepcionDto
{
    public int IdLote { get; set; }
    public string? Codigo { get; set; }
    public int SacosAsignados { get; set; }
    public decimal PesoAsignado { get; set; }
    
    // Informaci�n de recepci�n (si ya fue recibido)
    public int? IdLlegadaDetalle { get; set; }
    public int? SacosRecibidos { get; set; }
    public decimal? PesoRecibido { get; set; }
    public string? Observaciones { get; set; }
    public bool YaRecibido { get; set; }  // true si ya tiene registro en LlegadaPlantaDetalle
}
