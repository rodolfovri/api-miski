using FluentValidation;

namespace Miski.Application.Features.Usuarios.Commands.CreateUsuario;

public class CreateUsuarioValidator : AbstractValidator<CreateUsuarioCommand>
{
    public CreateUsuarioValidator()
    {
        RuleFor(x => x.Usuario.Username)
            .NotEmpty()
            .WithMessage("El nombre de usuario es requerido")
            .MinimumLength(3)
            .WithMessage("El nombre de usuario debe tener al menos 3 caracteres")
            .MaximumLength(50)
            .WithMessage("El nombre de usuario no puede exceder 50 caracteres");

        RuleFor(x => x.Usuario.Password)
            .NotEmpty()
            .WithMessage("La contraseña es requerida")
            .MinimumLength(6)
            .WithMessage("La contraseña debe tener al menos 6 caracteres")
            .MaximumLength(100)
            .WithMessage("La contraseña no puede exceder 100 caracteres");

        When(x => x.Usuario.IdPersona.HasValue, () =>
        {
            RuleFor(x => x.Usuario.IdPersona!.Value)
                .GreaterThan(0)
                .WithMessage("Debe seleccionar una persona válida");
        });

        When(x => !string.IsNullOrEmpty(x.Usuario.Estado), () =>
        {
            RuleFor(x => x.Usuario.Estado)
                .Must(estado => estado == "ACTIVO" || estado == "INACTIVO")
                .WithMessage("El estado debe ser 'ACTIVO' o 'INACTIVO'");
        });
    }
}
