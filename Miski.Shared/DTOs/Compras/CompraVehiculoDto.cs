namespace Miski.Shared.DTOs.Compras;

public class CompraVehiculoDto
{
    public int IdCompraVehiculo { get; set; }
    public int IdVehiculo { get; set; }
    public string GuiaRemision { get; set; } = string.Empty;
    public DateTime FRegistro { get; set; }
    
    // Informaci�n adicional
    public string? VehiculoPlaca { get; set; }
    public string? VehiculoMarca { get; set; }
    public string? VehiculoModelo { get; set; }
    
    // Lista de compras asignadas
    public List<CompraVehiculoDetalleDto> Detalles { get; set; } = new List<CompraVehiculoDetalleDto>();
}

public class CompraVehiculoDetalleDto
{
    public int IdCompraVehiculoDetalle { get; set; }
    public int IdCompraVehiculo { get; set; }
    public int IdCompra { get; set; }
    
    // Informaci�n adicional de la compra
    public string? CompraSerie { get; set; }
    public DateTime? CompraFRegistro { get; set; }
    public decimal? CompraMontoTotal { get; set; }
    public string? CompraNegociacionId { get; set; }
    
    // Indica si la compra est� asignada al CompraVehiculo (true) o est� disponible (false)
    public bool Asignado { get; set; }
}

// DTO extendido que incluye compras asignadas y disponibles
public class CompraVehiculoConDisponiblesDto
{
    public int IdCompraVehiculo { get; set; }
    public int IdVehiculo { get; set; }
    public string GuiaRemision { get; set; } = string.Empty;
    public DateTime FRegistro { get; set; }
    
    // Informaci�n adicional
    public string? VehiculoPlaca { get; set; }
    public string? VehiculoMarca { get; set; }
    public string? VehiculoModelo { get; set; }
    
    // Lista de compras (asignadas y disponibles)
    public List<CompraVehiculoDetalleDto> Detalles { get; set; } = new List<CompraVehiculoDetalleDto>();
}

// DTO para crear asignaci�n de compras a veh�culo
public class CreateCompraVehiculoDto
{
    public int IdVehiculo { get; set; }
    public string GuiaRemision { get; set; } = string.Empty;
    public List<int> IdCompras { get; set; } = new List<int>();
}

// DTO para actualizar asignaci�n de compras a veh�culo
public class UpdateCompraVehiculoDto
{
    public int IdCompraVehiculo { get; set; }
    public int IdVehiculo { get; set; }
    public string GuiaRemision { get; set; } = string.Empty;
    public List<int> IdCompras { get; set; } = new List<int>();
}
