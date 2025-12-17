using FluentValidation;

namespace Miski.Application.Features.Maestros.TipoMovimiento.Commands.UpdateTipoMovimiento;

public class UpdateTipoMovimientoValidator : AbstractValidator<UpdateTipoMovimientoCommand>
{
    public UpdateTipoMovimientoValidator()
    {
        RuleFor(x => x.TipoMovimientoData.IdTipoMovimiento)
            .GreaterThan(0).WithMessage("El ID del tipo de movimiento debe ser mayor a 0");

        RuleFor(x => x.TipoMovimientoData.TipoOperacion)
            .NotEmpty().WithMessage("El tipo de operación es requerido")
            .MaximumLength(20).WithMessage("El tipo de operación no puede exceder 20 caracteres")
            .Must(tipo => new[] { "INGRESO", "SALIDA" }.Contains(tipo.ToUpper()))
            .WithMessage("El tipo de operación debe ser 'INGRESO' o 'SALIDA'");

        RuleFor(x => x.TipoMovimientoData.Descripcion)
            .NotEmpty().WithMessage("La descripción es requerida")
            .MaximumLength(255).WithMessage("La descripción no puede exceder 255 caracteres");

        RuleFor(x => x.TipoMovimientoData.Estado)
            .MaximumLength(20).WithMessage("El estado no puede exceder 20 caracteres");
    }
}
