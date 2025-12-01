namespace Miski.Shared.DTOs.FAQ;

public class CategoriaFAQDto
{
    public int IdCategoriaFAQ { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}

public class CreateCategoriaFAQDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Estado { get; set; } = "ACTIVO";
}

public class UpdateCategoriaFAQDto
{
    public int IdCategoriaFAQ { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}
