using FluentValidation;

namespace Miski.Application.Features.Maestros.Banco.Commands.CreateBanco;

public class CreateBancoValidator : AbstractValidator<CreateBancoCommand>
{
    public CreateBancoValidator()
    {
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
