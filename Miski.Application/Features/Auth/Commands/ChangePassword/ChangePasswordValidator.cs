using FluentValidation;

namespace Miski.Application.Features.Auth.Commands.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("ID de usuario inválido");

        RuleFor(x => x.ChangePasswordData.CurrentPassword)
            .NotEmpty()
            .WithMessage("La contraseña actual es requerida");

        RuleFor(x => x.ChangePasswordData.NewPassword)
            .NotEmpty()
            .WithMessage("La nueva contraseña es requerida")
            .MinimumLength(8)
            .WithMessage("La nueva contraseña debe tener al menos 8 caracteres")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
            .WithMessage("La nueva contraseña debe contener al menos: 1 minúscula, 1 mayúscula, 1 número y 1 carácter especial");

        RuleFor(x => x.ChangePasswordData.ConfirmNewPassword)
            .NotEmpty()
            .WithMessage("La confirmación de la nueva contraseña es requerida")
            .Equal(x => x.ChangePasswordData.NewPassword)
            .WithMessage("Las contraseñas nuevas no coinciden");

        RuleFor(x => x.ChangePasswordData)
            .Must(x => x.CurrentPassword != x.NewPassword)
            .WithMessage("La nueva contraseña debe ser diferente a la actual");
    }
}