namespace Miski.Shared.DTOs.Maestros;

public class UnidadMedidaDto
{
    public int IdUnidadMedida { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Abreviatura { get; set; } = string.Empty;
}

public class CreateUnidadMedidaDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Abreviatura { get; set; } = string.Empty;
}

public class UpdateUnidadMedidaDto
{
    public int IdUnidadMedida { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Abreviatura { get; set; } = string.Empty;
}