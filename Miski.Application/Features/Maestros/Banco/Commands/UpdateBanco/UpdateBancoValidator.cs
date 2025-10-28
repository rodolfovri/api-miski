using FluentValidation;

namespace Miski.Application.Features.Maestros.Banco.Commands.UpdateBanco;

public class UpdateBancoValidator : AbstractValidator<UpdateBancoCommand>
{
    public UpdateBancoValidator()
    {
        RuleFor(x => x.Banco.IdBanco)
            .GreaterThan(0)
            .WithMessage("El ID del banco es requerido");

        RuleFor(x => x.Banco.Nombre)
            .NotEmpty()
            .WithMessage("El nombre del banco es requerido")
            .MaximumLength(20)
            .WithMessage("El nombre no puede exceder 20 caracteres");

        RuleFor(x => x.Banco.Estado)
            .MaximumLength(20)
            .WithMessage("El estado no puede exceder 20 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Banco.Estado));
    }
}
