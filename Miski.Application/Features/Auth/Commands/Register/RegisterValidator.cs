using FluentValidation;

namespace Miski.Application.Features.Auth.Commands.Register;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(x => x.RegisterData.IdPersona)
            .GreaterThan(0)
            .WithMessage("Debe seleccionar una persona v�lida");

        RuleFor(x => x.RegisterData.Username)
            .NotEmpty()
            .WithMessage("El nombre de usuario es requerido")
            .MinimumLength(3)
            .WithMessage("El nombre de usuario debe tener al menos 3 caracteres")
            .MaximumLength(50)
            .WithMessage("El nombre de usuario no puede exceder 50 caracteres")
            .Matches("^[a-zA-Z0-9._-]+$")
            .WithMessage("El nombre de usuario solo puede contener letras, n�meros, puntos, guiones y guiones bajos");

        RuleFor(x => x.RegisterData.Password)
            .NotEmpty()
            .WithMessage("La contrase�a es requerida")
            .MinimumLength(8)
            .WithMessage("La contrase�a debe tener al menos 8 caracteres")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
            .WithMessage("La contrase�a debe contener al menos: 1 min�scula, 1 may�scula, 1 n�mero y 1 car�cter especial");

        RuleFor(x => x.RegisterData.ConfirmPassword)
            .NotEmpty()
            .WithMessage("La confirmaci�n de contrase�a es requerida")
            .Equal(x => x.RegisterData.Password)
            .WithMessage("Las contrase�as no coinciden");

        RuleFor(x => x.RegisterData.IdRol)
            .GreaterThan(0)
            .WithMessage("Debe seleccionar un rol v�lido");
    }
}