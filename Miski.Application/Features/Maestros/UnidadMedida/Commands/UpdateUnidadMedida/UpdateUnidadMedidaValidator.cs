using FluentValidation;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.UnidadMedida.Commands.UpdateUnidadMedida;

public class UpdateUnidadMedidaValidator : AbstractValidator<UpdateUnidadMedidaDto>
{
    public UpdateUnidadMedidaValidator()
    {
        RuleFor(x => x.IdUnidadMedida)
            .GreaterThan(0)
            .WithMessage("El ID de la unidad de medida es requerido");

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