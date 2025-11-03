using FluentValidation;

namespace Miski.Application.Features.Compras.LlegadasPlanta.Commands.AnularLlegadaPlanta;

public class AnularLlegadaPlantaValidator : AbstractValidator<AnularLlegadaPlantaCommand>
{
    public AnularLlegadaPlantaValidator()
    {
        RuleFor(x => x.IdLlegadaPlanta)
            .GreaterThan(0)
            .WithMessage("El ID de la llegada a planta es obligatorio");

        RuleFor(x => x.IdUsuarioAnulacion)
            .GreaterThan(0)
            .WithMessage("El ID del usuario que anula es obligatorio");

        RuleFor(x => x.MotivoAnulacion)
            .NotEmpty()
            .WithMessage("El motivo de anulación es obligatorio")
            .MaximumLength(500)
            .WithMessage("El motivo de anulación no puede exceder 500 caracteres");
    }
}
