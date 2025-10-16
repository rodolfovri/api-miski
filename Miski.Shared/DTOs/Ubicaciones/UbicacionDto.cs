namespace Miski.Shared.DTOs.Ubicaciones;

public class UbicacionDto
{
    public int IdUbicacion { get; set; }
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string? Tipo { get; set; }
    public string? Estado { get; set; }
    public DateTime FRegistro { get; set; }
}

public class CreateUbicacionDto
{
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string? Tipo { get; set; }
    public string Estado { get; set; } = "ACTIVO";
}

public class UpdateUbicacionDto
{
    public int IdUbicacion { get; set; }
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string? Tipo { get; set; }
    public string? Estado { get; set; }
}