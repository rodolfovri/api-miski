namespace Miski.Shared.DTOs.FAQ;

public class FAQDto
{
    public int IdFAQ { get; set; }
    public int IdCategoriaFAQ { get; set; }
    public string Pregunta { get; set; } = string.Empty;
    public string Respuesta { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public int Orden { get; set; }
    public DateTime FRegistro { get; set; }
    
    // Información adicional
    public string? CategoriaNombre { get; set; }
}

public class CreateFAQDto
{
    public int IdCategoriaFAQ { get; set; }
    public string Pregunta { get; set; } = string.Empty;
    public string Respuesta { get; set; } = string.Empty;
    public string Estado { get; set; } = "ACTIVO";
    public int Orden { get; set; }
}

public class UpdateFAQDto
{
    public int IdFAQ { get; set; }
    public int IdCategoriaFAQ { get; set; }
    public string Pregunta { get; set; } = string.Empty;
    public string Respuesta { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public int Orden { get; set; }
}
