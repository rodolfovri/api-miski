using FluentValidation;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.UnidadMedida.Commands.CreateUnidadMedida;

public class CreateUnidadMedidaValidator : AbstractValidator<CreateUnidadMedidaDto>
{
    public CreateUnidadMedidaValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es requerido")
            .MaximumLength(20)
            .WithMessage("El nombre no puede exceder 20 caracteres");

        RuleFor(x => x.Abreviatura)
            .NotEmpty()
            .WithMessage("La abreviatura es requerida")
            .MaximumLength(10)
            .WithMessage("La abreviatura no puede exceder 10 caracteres");
    }
}