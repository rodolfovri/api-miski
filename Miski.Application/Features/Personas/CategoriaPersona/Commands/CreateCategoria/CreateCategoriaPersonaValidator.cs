using FluentValidation;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.CategoriaPersona.Commands.CreateCategoria;

public class CreateCategoriaPersonaValidator : AbstractValidator<CreateCategoriaPersonaDto>
{
    public CreateCategoriaPersonaValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es requerido")
            .MaximumLength(100)
            .WithMessage("El nombre no puede exceder 100 caracteres");
    }
}