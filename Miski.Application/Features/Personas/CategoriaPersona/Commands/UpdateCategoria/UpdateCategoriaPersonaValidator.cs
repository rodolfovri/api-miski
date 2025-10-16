using FluentValidation;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.CategoriaPersona.Commands.UpdateCategoria;

public class UpdateCategoriaPersonaValidator : AbstractValidator<UpdateCategoriaPersonaDto>
{
    public UpdateCategoriaPersonaValidator()
    {
        RuleFor(x => x.IdCategoriaPersona)
            .GreaterThan(0)
            .WithMessage("El ID de la categoría es requerido");

        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es requerido")
            .MaximumLength(100)
            .WithMessage("El nombre no puede exceder 100 caracteres");
    }
}