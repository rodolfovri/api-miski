using Microsoft.AspNetCore.Http;

namespace Miski.Shared.DTOs.Maestros;

public class VariedadProductoDto
{
    public int IdVariedadProducto { get; set; }
    public int IdProducto { get; set; }
    public int IdUnidadMedida { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Estado { get; set; }
    public string? FichaTecnica { get; set; }  // ? URL del PDF
    public DateTime FRegistro { get; set; }
    
    // Información adicional
    public string? ProductoNombre { get; set; }
    public string? UnidadMedidaNombre { get; set; }

    // Materia Prima
    public decimal StockMateriaPrimaKg { get; set; }
    public int StockMateriaPrimaSacos { get; set; }

    // Producto Terminado
    public decimal StockProductoTerminadoKg { get; set; }
    public int StockProductoTerminadoSacos { get; set; }
}

public class CreateVariedadProductoDto
{
    public int IdProducto { get; set; }
    public int IdUnidadMedida { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Estado { get; set; } = "ACTIVO";
    public IFormFile? FichaTecnica { get; set; }  // ? Archivo PDF (opcional)
}

public class UpdateVariedadProductoDto
{
    public int IdVariedadProducto { get; set; }
    public int IdProducto { get; set; }
    public int IdUnidadMedida { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Estado { get; set; }
    public IFormFile? FichaTecnica { get; set; }  // ? Archivo PDF (opcional para actualizar)
}
