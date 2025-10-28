namespace Miski.Shared.DTOs.Maestros;

public class TipoCambioDto
{
    public int IdTipoCambio { get; set; }
    public int IdMoneda { get; set; }
    public int IdUsuario { get; set; }
    public decimal ValorCompra { get; set; }
    public decimal ValorVenta { get; set; }
    public DateTime FRegistro { get; set; }
    
    // Información adicional
    public string? MonedaNombre { get; set; }
    public string? MonedaCodigo { get; set; }
    public string? MonedaSimbolo { get; set; }
    public string? UsuarioNombre { get; set; }
}

// DTO para crear tipo de cambio
public class CreateTipoCambioDto
{
    public int IdMoneda { get; set; }
    public int IdUsuario { get; set; }
    public decimal ValorCompra { get; set; }
    public decimal ValorVenta { get; set; }
}

// DTO para actualizar tipo de cambio
public class UpdateTipoCambioDto
{
    public int IdTipoCambio { get; set; }
    public int IdMoneda { get; set; }
    public int IdUsuario { get; set; }
    public decimal ValorCompra { get; set; }
    public decimal ValorVenta { get; set; }
}
