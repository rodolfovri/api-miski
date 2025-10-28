namespace Miski.Shared.DTOs.Maestros;

public class MonedaDto
{
    public int IdMoneda { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Simbolo { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
}

// DTO para crear moneda
public class CreateMonedaDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Simbolo { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
}

// DTO para actualizar moneda
public class UpdateMonedaDto
{
    public int IdMoneda { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Simbolo { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
}
