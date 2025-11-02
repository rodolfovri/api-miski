using FluentValidation;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Lotes.Commands.CreateLote;

public class CreateLoteValidator : AbstractValidator<CreateLoteCommand>
{
    public CreateLoteValidator()
    {
        RuleFor(x => x.Lote.IdCompra)
            .GreaterThan(0)
            .WithMessage("Debe seleccionar una compra v�lida");

        RuleFor(x => x.Lote.Peso)
            .GreaterThan(0)
            .WithMessage("El peso debe ser mayor que 0")
            .LessThanOrEqualTo(10000)
            .WithMessage("El peso no puede exceder 10,000 kg");

        RuleFor(x => x.Lote.Sacos)
            .GreaterThan(0)
            .WithMessage("El n�mero de sacos debe ser mayor que 0")
            .LessThanOrEqualTo(500)
            .WithMessage("El n�mero de sacos no puede exceder 500 unidades");

        RuleFor(x => x.Lote.Codigo)
            .MaximumLength(50)
            .WithMessage("El c�digo no puede exceder 50 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Lote.Codigo));

        RuleFor(x => x.Lote.MontoTotal)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El monto total debe ser mayor o igual a 0");

        RuleFor(x => x.Lote.Comision)
            .GreaterThanOrEqualTo(0)
            .WithMessage("La comisi�n debe ser mayor o igual a 0")
            .When(x => x.Lote.Comision.HasValue);

        RuleFor(x => x.Lote.Observacion)
            .MaximumLength(200)
            .WithMessage("La observaci�n no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Lote.Observacion));
    }
}
