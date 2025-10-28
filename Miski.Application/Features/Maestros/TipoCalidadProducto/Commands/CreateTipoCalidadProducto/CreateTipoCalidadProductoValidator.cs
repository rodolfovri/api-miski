using FluentValidation;

namespace Miski.Application.Features.Maestros.TipoCalidadProducto.Commands.CreateTipoCalidadProducto;

public class CreateTipoCalidadProductoValidator : AbstractValidator<CreateTipoCalidadProductoCommand>
{
    public CreateTipoCalidadProductoValidator()
    {
        RuleFor(x => x.TipoCalidadProducto.IdProducto)
            .GreaterThan(0)
            .WithMessage("Debe seleccionar un producto válido");

        RuleFor(x => x.TipoCalidadProducto.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es requerido")
            .MaximumLength(50)
            .WithMessage("El nombre no puede exceder 50 caracteres");

        RuleFor(x => x.TipoCalidadProducto.Estado)
            .MaximumLength(20)
            .WithMessage("El estado no puede exceder 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.TipoCalidadProducto.Estado));
    }
}
