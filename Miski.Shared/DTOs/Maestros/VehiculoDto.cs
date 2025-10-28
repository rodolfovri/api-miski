namespace Miski.Shared.DTOs.Maestros;

public class VehiculoDto
{
    public int IdVehiculo { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public string? Estado { get; set; }
}

public class CreateVehiculoDto
{
    public string Placa { get; set; } = string.Empty;
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
}

public class UpdateVehiculoDto
{
    public int IdVehiculo { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public string? Estado { get; set; }
}
