using Microsoft.AspNetCore.Http;

namespace Miski.Shared.DTOs.Almacen;

public class ProductoDto
{
    public int IdProducto { get; set; }
    public int IdCategoriaProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Estado { get; set; }
    public string? Imagen { get; set; }  // URL de la imagen
    public string? FichaTecnica { get; set; }  // URL del PDF
    public DateTime FRegistro { get; set; }
    
    // Información adicional
    public string? CategoriaProductoNombre { get; set; }
}

public class CreateProductoDto
{
    public int IdCategoriaProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Estado { get; set; } = "ACTIVO";
    public IFormFile? Imagen { get; set; }  // Archivo de imagen (opcional)
    public IFormFile? FichaTecnica { get; set; }  // Archivo PDF (opcional)
}

public class UpdateProductoDto
{
    public int IdProducto { get; set; }
    public int IdCategoriaProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Estado { get; set; }
    public IFormFile? Imagen { get; set; }  // Archivo de imagen (opcional para actualizar)
    public IFormFile? FichaTecnica { get; set; }  // Archivo PDF (opcional para actualizar)
}
