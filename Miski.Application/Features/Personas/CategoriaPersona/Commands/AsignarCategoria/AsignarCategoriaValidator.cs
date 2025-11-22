using FluentValidation;

namespace Miski.Application.Features.Personas.CategoriaPersona.Commands.AsignarCategoria;

public class AsignarCategoriaValidator : AbstractValidator<AsignarCategoriaCommand>
{
    public AsignarCategoriaValidator()
    {
        RuleFor(x => x.Data.IdPersona)
            .GreaterThan(0)
            .WithMessage("El ID de la persona debe ser mayor a 0");

        RuleFor(x => x.Data.IdCategoria)
            .GreaterThan(0)
            .WithMessage("El ID de la categoría debe ser mayor a 0");
    }
}
