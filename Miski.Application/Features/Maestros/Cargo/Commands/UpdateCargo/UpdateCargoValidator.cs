using FluentValidation;

namespace Miski.Application.Features.Maestros.Cargo.Commands.UpdateCargo;

public class UpdateCargoValidator : AbstractValidator<UpdateCargoCommand>
{
    public UpdateCargoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El ID del cargo debe ser mayor a 0");

        RuleFor(x => x.Cargo.IdCargo)
            .GreaterThan(0).WithMessage("El ID del cargo debe ser mayor a 0");

        RuleFor(x => x.Cargo.Nombre)
            .NotEmpty().WithMessage("El nombre del cargo es requerido")
            .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres");

        RuleFor(x => x.Cargo.Descripcion)
            .MaximumLength(100).WithMessage("La descripción no puede exceder 100 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Cargo.Descripcion));

        RuleFor(x => x.Cargo.Estado)
            .MaximumLength(20).WithMessage("El estado no puede exceder 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Cargo.Estado));
    }
}
