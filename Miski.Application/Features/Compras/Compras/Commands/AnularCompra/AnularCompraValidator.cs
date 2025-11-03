using FluentValidation;

namespace Miski.Application.Features.Compras.Compras.Commands.AnularCompra;

public class AnularCompraValidator : AbstractValidator<AnularCompraCommand>
{
    public AnularCompraValidator()
    {
        RuleFor(x => x.IdCompra)
            .GreaterThan(0)
            .WithMessage("El ID de la compra es obligatorio");

        RuleFor(x => x.IdUsuarioAnulacion)
            .GreaterThan(0)
            .WithMessage("El ID del usuario es obligatorio");

        RuleFor(x => x.MotivoAnulacion)
            .NotEmpty()
            .WithMessage("El motivo de anulación es obligatorio")
            .MaximumLength(200)
            .WithMessage("El motivo de anulación no puede exceder 200 caracteres");
    }
}
