using FluentValidation;

namespace Miski.Application.Features.Maestros.ConfiguracionGlobal.Commands.CreateConfiguracion;

public class CreateConfiguracionValidator : AbstractValidator<CreateConfiguracionCommand>
{
    public CreateConfiguracionValidator()
    {
        RuleFor(x => x.ConfiguracionData.Clave)
            .NotEmpty().WithMessage("La clave es requerida")
            .MaximumLength(100).WithMessage("La clave no puede exceder 100 caracteres");

        RuleFor(x => x.ConfiguracionData.Valor)
            .NotEmpty().WithMessage("El valor es requerido")
            .MaximumLength(255).WithMessage("El valor no puede exceder 255 caracteres");

        RuleFor(x => x.ConfiguracionData.TipoDato)
            .NotEmpty().WithMessage("El tipo de dato es requerido")
            .Must(tipo => new[] { "string", "int", "decimal", "bool" }.Contains(tipo.ToLower()))
            .WithMessage("El tipo de dato debe ser: string, int, decimal o bool");

        RuleFor(x => x.ConfiguracionData.Descripcion)
            .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres");
    }
}
