using FluentValidation;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.Commands.UpdatePersona;

public class UpdatePersonaValidator : AbstractValidator<UpdatePersonaDto>
{
    public UpdatePersonaValidator()
    {
        RuleFor(x => x.IdPersona)
            .GreaterThan(0)
            .WithMessage("El ID de la persona es requerido");

        RuleFor(x => x.IdTipoDocumento)
            .GreaterThan(0)
            .WithMessage("El tipo de documento es requerido");

        RuleFor(x => x.NumeroDocumento)
            .NotEmpty()
            .WithMessage("El número de documento es requerido")
            .MinimumLength(8)
            .WithMessage("El número de documento debe tener al menos 8 caracteres")
            .MaximumLength(20)
            .WithMessage("El número de documento no puede exceder 20 caracteres");

        RuleFor(x => x.Nombres)
            .NotEmpty()
            .WithMessage("Los nombres son requeridos")
            .MaximumLength(100)
            .WithMessage("Los nombres no pueden exceder 100 caracteres");

        RuleFor(x => x.Apellidos)
            .NotEmpty()
            .WithMessage("Los apellidos son requeridos")
            .MaximumLength(100)
            .WithMessage("Los apellidos no pueden exceder 100 caracteres");

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("El email no es válido");

        RuleFor(x => x.Telefono)
            .MaximumLength(15)
            .When(x => !string.IsNullOrEmpty(x.Telefono))
            .WithMessage("El teléfono no puede exceder 15 caracteres");
    }
}