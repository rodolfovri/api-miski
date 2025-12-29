namespace Miski.Shared.DTOs.Maestros;

public class KardexVariedadProductoDto
{
    public int IdVariedadProducto { get; set; }
    public string CodigoVariedad { get; set; } = string.Empty;
    public string NombreVariedad { get; set; } = string.Empty;
    public string NombreProducto { get; set; } = string.Empty;
    public DateTime FechaDesde { get; set; }
    public DateTime FechaHasta { get; set; }
    public string? TipoStock { get; set; } // Filtro aplicado (MATERIA_PRIMA, PRODUCTO_TERMINADO o null = todos)
    public decimal StockInicial { get; set; }
    public int SacosInicial { get; set; }
    public List<MovimientoKardexDto> Movimientos { get; set; } = new();
    public decimal StockFinal { get; set; }
    public int SacosFinal { get; set; }
}

public class MovimientoKardexDto
{
    public int IdMovimientoAlmacen { get; set; }
    public DateTime Fecha { get; set; }
    public string TipoOperacion { get; set; } = string.Empty; // "INGRESO" o "SALIDA"
    public string TipoStock { get; set; } = string.Empty; // "MATERIA_PRIMA" o "PRODUCTO_TERMINADO"
    public string Descripcion { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public int? IdLlegadaPlanta { get; set; }
    public string? LoteCodigo { get; set; }
    
    // Cantidades del movimiento
    public decimal CantidadIngreso { get; set; }
    public int SacosIngreso { get; set; }
    public decimal CantidadSalida { get; set; }
    public int SacosSalida { get; set; }
    
    // Saldo después del movimiento
    public decimal SaldoCantidad { get; set; }
    public int SaldoSacos { get; set; }
    
    // Usuario que realizó el movimiento
    public string UsuarioNombre { get; set; } = string.Empty;
    
    // Ubicación del movimiento
    public string UbicacionNombre { get; set; } = string.Empty;
}
