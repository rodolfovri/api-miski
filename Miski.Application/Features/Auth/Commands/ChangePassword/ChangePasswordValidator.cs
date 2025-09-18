using FluentValidation;

namespace Miski.Application.Features.Auth.Commands.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("ID de usuario inv�lido");

        RuleFor(x => x.ChangePasswordData.CurrentPassword)
            .NotEmpty()
            .WithMessage("La contrase�a actual es requerida");

        RuleFor(x => x.ChangePasswordData.NewPassword)
            .NotEmpty()
            .WithMessage("La nueva contrase�a es requerida")
            .MinimumLength(8)
            .WithMessage("La nueva contrase�a debe tener al menos 8 caracteres")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
            .WithMessage("La nueva contrase�a debe contener al menos: 1 min�scula, 1 may�scula, 1 n�mero y 1 car�cter especial");

        RuleFor(x => x.ChangePasswordData.ConfirmNewPassword)
            .NotEmpty()
            .WithMessage("La confirmaci�n de la nueva contrase�a es requerida")
            .Equal(x => x.ChangePasswordData.NewPassword)
            .WithMessage("Las contrase�as nuevas no coinciden");

        RuleFor(x => x.ChangePasswordData)
            .Must(x => x.CurrentPassword != x.NewPassword)
            .WithMessage("La nueva contrase�a debe ser diferente a la actual");
    }
}