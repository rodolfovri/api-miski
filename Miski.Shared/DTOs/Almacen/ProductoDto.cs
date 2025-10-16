namespace Miski.Shared.DTOs.Almacen;

public class ProductoDto
{
    public int IdProducto { get; set; }
    public int IdCategoriaProducto { get; set; }
    public int IdUnidadMedida { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Estado { get; set; }
    public DateTime FRegistro { get; set; }
    
    // Información adicional
    public string? CategoriaProductoNombre { get; set; }
    public string? UnidadMedidaNombre { get; set; }
}

public class CreateProductoDto
{
    public int IdCategoriaProducto { get; set; }
    public int IdUnidadMedida { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Estado { get; set; } = "ACTIVO";
}

public class UpdateProductoDto
{
    public int IdProducto { get; set; }
    public int IdCategoriaProducto { get; set; }
    public int IdUnidadMedida { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Estado { get; set; }
}
