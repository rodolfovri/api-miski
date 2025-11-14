using FluentValidation;

namespace Miski.Application.Features.Usuarios.Commands.UpdatePassword;

public class UpdatePasswordValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordValidator()
    {
        RuleFor(x => x.Password.IdUsuario)
            .GreaterThan(0)
            .WithMessage("El ID del usuario debe ser mayor que 0");

        RuleFor(x => x.Password.NewPassword)
            .NotEmpty()
            .WithMessage("La nueva contraseña es requerida")
            .MinimumLength(6)
            .WithMessage("La contraseña debe tener al menos 6 caracteres")
            .MaximumLength(100)
            .WithMessage("La contraseña no puede exceder 100 caracteres");
    }
}
