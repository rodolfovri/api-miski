using FluentValidation;
using Miski.Shared.DTOs.Permisos;

namespace Miski.Application.Features.Permisos.Commands.CreateAccion;

public class CreateAccionValidator : AbstractValidator<CreateAccionDto>
{
    public CreateAccionValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la acción es requerido")
            .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres");

        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("El código de la acción es requerido")
            .MaximumLength(20).WithMessage("El código no puede exceder 20 caracteres");

        RuleFor(x => x.Icono)
            .NotEmpty().WithMessage("El icono es requerido")
            .MaximumLength(50).WithMessage("El icono no puede exceder 50 caracteres");

        RuleFor(x => x.Orden)
            .GreaterThan(0).WithMessage("El orden debe ser mayor a 0");

        RuleFor(x => x.Estado)
            .NotEmpty().WithMessage("El estado es requerido")
            .Must(e => e == "ACTIVO" || e == "INACTIVO")
            .WithMessage("El estado debe ser ACTIVO o INACTIVO");
    }
}
