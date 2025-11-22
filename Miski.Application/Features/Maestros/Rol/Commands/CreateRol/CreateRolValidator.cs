using FluentValidation;

namespace Miski.Application.Features.Maestros.Rol.Commands.CreateRol;

public class CreateRolValidator : AbstractValidator<CreateRolCommand>
{
    public CreateRolValidator()
    {
        RuleFor(x => x.Rol.Nombre)
            .NotEmpty()
            .WithMessage("El nombre del rol es requerido")
            .MaximumLength(100)
            .WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Rol.Descripcion)
            .MaximumLength(500)
            .WithMessage("La descripción no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Rol.Descripcion));

        RuleFor(x => x.Rol.TipoPlataforma)
            .MaximumLength(50)
            .WithMessage("El tipo de plataforma no puede exceder 50 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Rol.TipoPlataforma));

        RuleFor(x => x.Rol.Estado)
            .Must(estado => estado == null || estado == "ACTIVO" || estado == "INACTIVO")
            .WithMessage("El estado debe ser ACTIVO o INACTIVO")
            .When(x => !string.IsNullOrEmpty(x.Rol.Estado));
    }
}
