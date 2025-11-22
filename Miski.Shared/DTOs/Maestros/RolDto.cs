namespace Miski.Shared.DTOs.Maestros;

public class RolMaestroDto
{
    public int IdRol { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? TipoPlataforma { get; set; }
    public string Estado { get; set; } = "ACTIVO";
}

public class CreateRolDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? TipoPlataforma { get; set; }
    public string? Estado { get; set; }
}

public class UpdateRolDto
{
    public int IdRol { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? TipoPlataforma { get; set; }
    public string? Estado { get; set; }
}
