using FluentValidation;
using Miski.Shared.DTOs.Auth;

namespace Miski.Application.Features.Auth.Commands.Login;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.LoginData.NumeroDocumento)
            .NotEmpty()
            .WithMessage("El n�mero de documento es requerido")
            .MinimumLength(8)
            .WithMessage("El n�mero de documento debe tener al menos 8 caracteres")
            .MaximumLength(20)
            .WithMessage("El n�mero de documento no puede exceder 20 caracteres");

        RuleFor(x => x.LoginData.Password)
            .NotEmpty()
            .WithMessage("La contrase�a es requerida")
            .MinimumLength(6)
            .WithMessage("La contrase�a debe tener al menos 6 caracteres");

        RuleFor(x => x.LoginData.TipoPlataforma)
            .NotEmpty()
            .WithMessage("El tipo de plataforma es requerido")
            .Must(tipo => tipo == "Web" || tipo == "Mobile")
            .WithMessage("El tipo de plataforma debe ser 'Web' o 'Mobile'");
    }
}