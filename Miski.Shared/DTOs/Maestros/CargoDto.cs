namespace Miski.Shared.DTOs.Maestros;

public class CargoDto
{
    public int IdCargo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime FRegistro { get; set; }
}

public class CreateCargoDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Estado { get; set; }
}

public class UpdateCargoDto
{
    public int IdCargo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Estado { get; set; }
}
