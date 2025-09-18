namespace Miski.Shared.DTOs.Auth;

public class RegisterDto
{
    public int IdPersona { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public int IdRol { get; set; }
    public string? Estado { get; set; }
}