using FluentValidation;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.VariedadProducto.Commands.UpdateVariedad;

public class UpdateVariedadProductoValidator : AbstractValidator<UpdateVariedadProductoDto>
{
    public UpdateVariedadProductoValidator()
    {
        RuleFor(x => x.IdVariedadProducto)
            .GreaterThan(0).WithMessage("El ID de la variedad es requerido");

        RuleFor(x => x.IdProducto)
            .GreaterThan(0).WithMessage("Debe seleccionar un producto v�lido");

        RuleFor(x => x.IdUnidadMedida)
            .GreaterThan(0).WithMessage("Debe seleccionar una unidad de medida v�lida");

        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("El c�digo es requerido")
            .MaximumLength(50).WithMessage("El c�digo no puede exceder 50 caracteres");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Descripcion)
            .MaximumLength(150).WithMessage("La descripci�n no puede exceder 150 caracteres");

        When(x => !string.IsNullOrEmpty(x.Estado), () =>
        {
            RuleFor(x => x.Estado)
                .Must(x => x == "ACTIVO" || x == "INACTIVO")
                .WithMessage("El estado debe ser ACTIVO o INACTIVO");
        });
    }
}
