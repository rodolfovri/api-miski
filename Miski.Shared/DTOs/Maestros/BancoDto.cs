namespace Miski.Shared.DTOs.Maestros;

public class BancoDto
{
    public int IdBanco { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}

public class CreateBancoDto
{
    public string Nombre { get; set; } = string.Empty;
    public string? Estado { get; set; }
}

public class UpdateBancoDto
{
    public int IdBanco { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Estado { get; set; }
}
