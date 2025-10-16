using FluentValidation;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.CategoriaProducto.Commands.CreateCategoria;

public class CreateCategoriaProductoValidator : AbstractValidator<CreateCategoriaProductoDto>
{
    public CreateCategoriaProductoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es requerido")
            .MaximumLength(50)
            .WithMessage("El nombre no puede exceder 50 caracteres");

        RuleFor(x => x.Descripcion)
            .MaximumLength(255)
            .When(x => !string.IsNullOrEmpty(x.Descripcion))
            .WithMessage("La descripción no puede exceder 255 caracteres");

        RuleFor(x => x.Estado)
            .Must(estado => estado == "ACTIVO" || estado == "INACTIVO")
            .WithMessage("El estado debe ser ACTIVO o INACTIVO");
    }
}