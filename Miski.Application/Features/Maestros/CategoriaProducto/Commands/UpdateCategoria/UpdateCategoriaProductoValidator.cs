using FluentValidation;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.CategoriaProducto.Commands.UpdateCategoria;

public class UpdateCategoriaProductoValidator : AbstractValidator<UpdateCategoriaProductoDto>
{
    public UpdateCategoriaProductoValidator()
    {
        RuleFor(x => x.IdCategoriaProducto)
            .GreaterThan(0)
            .WithMessage("El ID de la categoría es requerido");

        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es requerido")
            .MaximumLength(50)
            .WithMessage("El nombre no puede exceder 50 caracteres");

        RuleFor(x => x.Descripcion)
            .MaximumLength(255)
            .When(x => !string.IsNullOrEmpty(x.Descripcion))
            .WithMessage("La descripción no puede exceder 255 caracteres");
    }
}