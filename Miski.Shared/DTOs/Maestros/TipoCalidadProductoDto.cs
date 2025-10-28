namespace Miski.Shared.DTOs.Maestros;

public class TipoCalidadProductoDto
{
    public int IdTipoCalidadProducto { get; set; }
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Estado { get; set; }
    
    // Información adicional
    public string? ProductoNombre { get; set; }
}

public class CreateTipoCalidadProductoDto
{
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Estado { get; set; }  // AGREGADO como opcional
}

public class UpdateTipoCalidadProductoDto
{
    public int IdTipoCalidadProducto { get; set; }
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Estado { get; set; }
}
