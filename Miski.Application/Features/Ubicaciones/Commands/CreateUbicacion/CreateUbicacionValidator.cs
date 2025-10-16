using FluentValidation;
using Miski.Shared.DTOs.Ubicaciones;

namespace Miski.Application.Features.Ubicaciones.Commands.CreateUbicacion;

public class CreateUbicacionValidator : AbstractValidator<CreateUbicacionDto>
{
    public CreateUbicacionValidator()
    {
        RuleFor(x => x.IdUsuario)
            .GreaterThan(0)
            .WithMessage("El ID del usuario es requerido");

        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es requerido")
            .MaximumLength(100)
            .WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Direccion)
            .NotEmpty()
            .WithMessage("La direcci�n es requerida")
            .MaximumLength(200)
            .WithMessage("La direcci�n no puede exceder 200 caracteres");

        RuleFor(x => x.Tipo)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.Tipo))
            .WithMessage("El tipo no puede exceder 50 caracteres");

        RuleFor(x => x.Estado)
            .Must(estado => estado == "ACTIVO" || estado == "INACTIVO")
            .WithMessage("El estado debe ser ACTIVO o INACTIVO");
    }
}