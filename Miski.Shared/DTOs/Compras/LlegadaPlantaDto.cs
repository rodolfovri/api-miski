namespace Miski.Shared.DTOs.Compras;

// DTO para LlegadaPlanta
public class LlegadaPlantaDto
{
    public int IdLlegadaPlanta { get; set; }
    public int IdCompra { get; set; }
    public int IdUsuario { get; set; }
    public int IdLote { get; set; }
    public int IdUbicacion { get; set; }
    public decimal SacosRecibidos { get; set; }
    public decimal PesoRecibido { get; set; }
    public DateTime? FLlegada { get; set; }
    public string? Observaciones { get; set; }
    public string? Estado { get; set; }
    
    // Informaci�n adicional
    public string? CompraSerie { get; set; }
    public string? UsuarioNombre { get; set; }
    public string? LoteCodigo { get; set; }
    public string? UbicacionNombre { get; set; }
    
    // Datos del lote original
    public int SacosAsignados { get; set; }
    public decimal PesoAsignado { get; set; }
    public int DiferenciaSacos { get; set; }
    public decimal DiferenciaPeso { get; set; }
}

// DTO para crear LlegadaPlanta (ACTUALIZADO para recibir m�ltiples lotes)
public class CreateLlegadaPlantaDto
{
    public int IdCompraVehiculo { get; set; }
    public int IdUsuario { get; set; }
    public int IdUbicacion { get; set; }
    public List<LlegadaPlantaDetalleInputDto> Detalles { get; set; } = new List<LlegadaPlantaDetalleInputDto>();
}

// DTO para el detalle de cada lote recibido
public class LlegadaPlantaDetalleInputDto
{
    public int IdCompra { get; set; }
    public int IdLote { get; set; }
    public decimal SacosRecibidos { get; set; }
    public decimal PesoRecibido { get; set; }
    public string? Observaciones { get; set; }
}

// DTO para respuesta de creaci�n m�ltiple
public class CreateLlegadaPlantaResponseDto
{
    public int IdCompraVehiculo { get; set; }
    public int TotalLotesRecibidos { get; set; }
    public DateTime FLlegada { get; set; }
    public List<LlegadaPlantaDto> LlegadasRegistradas { get; set; } = new List<LlegadaPlantaDto>();
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
    public int? IdLlegadaPlanta { get; set; }
    public decimal? SacosRecibidos { get; set; }
    public decimal? PesoRecibido { get; set; }
    public int? DiferenciaSacos { get; set; }
    public decimal? DiferenciaPeso { get; set; }
    public string? Observaciones { get; set; }
    public bool YaRecibido { get; set; }
}

// DTO para reporte completo de veh�culos con compras y recepciones
public class VehiculoConComprasYRecepcionesDto
{
    public int IdCompraVehiculo { get; set; }
    public int IdPersona { get; set; }
    public int IdVehiculo { get; set; }
    public string GuiaRemision { get; set; } = string.Empty;
    public DateTime FRegistro { get; set; }
    public string? Estado { get; set; }
    
    // Informaci�n de la persona
    public string? PersonaNombre { get; set; }
    
    // Informaci�n del veh�culo
    public string? VehiculoPlaca { get; set; }
    public string? VehiculoMarca { get; set; }
    public string? VehiculoModelo { get; set; }
    
    // Compras asignadas a este veh�culo con detalles de recepci�n
    public List<CompraConRecepcionDetalladaDto> Compras { get; set; } = new List<CompraConRecepcionDetalladaDto>();
}

// DTO para compra con recepci�n detallada
public class CompraConRecepcionDetalladaDto
{
    public int IdCompra { get; set; }
    public string? Serie { get; set; }
    public DateTime? FRegistro { get; set; }
    public string? Estado { get; set; }
    
    // Lotes con detalles de recepci�n
    public List<LoteConRecepcionDetalladoDto> Lotes { get; set; } = new List<LoteConRecepcionDetalladoDto>();
}

// DTO para lote con recepci�n detallada
public class LoteConRecepcionDetalladoDto
{
    public int IdLote { get; set; }
    public string? Codigo { get; set; }
    public int SacosAsignados { get; set; }
    public decimal PesoAsignado { get; set; }
    
    // Datos de recepci�n en planta
    public int? IdLlegadaPlanta { get; set; }
    public decimal? SacosRecibidos { get; set; }
    public decimal? PesoRecibido { get; set; }
    public int? DiferenciaSacos { get; set; }
    public decimal? DiferenciaPeso { get; set; }
    public string? Observaciones { get; set; }
    public bool YaRecibido { get; set; }
}

// DTO NUEVO para el reporte de veh�culos con compras ACTIVAS
public class CompraVehiculoResumenDto
{
    public int IdCompraVehiculo { get; set; }
    public string? VehiculoPlaca { get; set; }
    public string GuiaRemision { get; set; } = string.Empty;
    public DateTime FRegistro { get; set; }
    
    public List<CompraDetalleDto> Detalles { get; set; } = new List<CompraDetalleDto>();
}

// DTO NUEVO para detalle de compra con lote
public class CompraDetalleDto
{
    public int IdCompra { get; set; }
    public string? CodigoLote { get; set; }
    public int SacosEnviados { get; set; }
    public decimal PesoEnviado { get; set; }
    public decimal? SacosRecibidos { get; set; }
    public decimal? PesoRecibido { get; set; }
    public string? EstadoCompra { get; set; }
}
