using FluentValidation;
using Miski.Shared.DTOs;
using System.IO;

namespace Miski.Application.Features.Negociaciones.Commands.CreateNegociacion;

public class CreateNegociacionValidator : AbstractValidator<CreateNegociacionCommand>
{
    public CreateNegociacionValidator()
    {
        RuleFor(x => x.Negociacion.IdComisionista)
            .GreaterThan(0)
            .WithMessage("IdComisionista debe ser mayor que 0");

        RuleFor(x => x.Negociacion.Fecha)
            .NotEmpty()
            .WithMessage("Fecha es requerida")
            .LessThanOrEqualTo(DateTime.Now.AddDays(1))
            .WithMessage("La fecha no puede ser futura")
            .GreaterThanOrEqualTo(DateTime.Now.AddDays(-30))
            .WithMessage("La fecha no puede ser mayor a 30 días en el pasado");

        RuleFor(x => x.Negociacion.PesoTotal)
            .GreaterThan(0)
            .WithMessage("Peso total debe ser mayor que 0")
            .LessThanOrEqualTo(50000) // 50 toneladas máximo
            .WithMessage("Peso total no puede exceder 50,000 kg");

        RuleFor(x => x.Negociacion.SacosTotales)
            .GreaterThan(0)
            .WithMessage("Sacos totales debe ser mayor que 0")
            .LessThanOrEqualTo(2000) // máximo 2000 sacos
            .WithMessage("Sacos totales no puede exceder 2,000 unidades");

        RuleFor(x => x.Negociacion.PrecioUnitario)
            .GreaterThan(0)
            .WithMessage("Precio unitario debe ser mayor que 0")
            .LessThanOrEqualTo(1000) // máximo 1000 soles por kg
            .WithMessage("Precio unitario no puede exceder S/. 1,000 por kg");

        RuleFor(x => x.Negociacion.NroCuentaRuc)
            .NotEmpty()
            .WithMessage("Número de cuenta/RUC es requerido")
            .MaximumLength(20)
            .WithMessage("Número de cuenta/RUC no puede exceder 20 caracteres")
            .MinimumLength(8)
            .WithMessage("Número de cuenta/RUC debe tener al menos 8 caracteres");

        RuleFor(x => x.Negociacion.FotoCalidadProducto)
            .NotEmpty()
            .WithMessage("Foto de calidad del producto es requerida")
            .Must(BeValidImagePath)
            .WithMessage("La foto de calidad debe ser una ruta válida o URL");

        RuleFor(x => x.Negociacion.FotoDniFrontal)
            .NotEmpty()
            .WithMessage("Foto frontal del DNI es requerida")
            .Must(BeValidImagePath)
            .WithMessage("La foto frontal del DNI debe ser una ruta válida o URL");

        RuleFor(x => x.Negociacion.FotoDniPosterior)
            .NotEmpty()
            .WithMessage("Foto posterior del DNI es requerida")
            .Must(BeValidImagePath)
            .WithMessage("La foto posterior del DNI debe ser una ruta válida o URL");

        RuleFor(x => x.Negociacion.Observacion)
            .MaximumLength(500)
            .WithMessage("Las observaciones no pueden exceder 500 caracteres");

        // Validación condicional para proveedor
        When(x => x.Negociacion.IdProveedor.HasValue, () =>
        {
            RuleFor(x => x.Negociacion.IdProveedor.Value)
                .GreaterThan(0)
                .WithMessage("IdProveedor debe ser mayor que 0");
        });

        // Validación condicional para producto
        When(x => x.Negociacion.IdProducto.HasValue, () =>
        {
            RuleFor(x => x.Negociacion.IdProducto.Value)
                .GreaterThan(0)
                .WithMessage("IdProducto debe ser mayor que 0");
        });

        // Validación de lógica de negocio: coherencia entre peso y sacos
        RuleFor(x => x.Negociacion)
            .Must(HaveConsistentWeightAndBags)
            .WithMessage("La relación entre peso total y número de sacos no es coherente (peso promedio por saco debe estar entre 20-30 kg)");
    }

    private static bool BeValidImagePath(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
            return false;

        // Verificar extensiones válidas
        var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".bmp" };
        var extension = Path.GetExtension(imagePath.ToLower());
        
        return validExtensions.Contains(extension) || 
               imagePath.StartsWith("http://") || 
               imagePath.StartsWith("https://");
    }

    private static bool HaveConsistentWeightAndBags(CreateNegociacionDto negociacion)
    {
        if (negociacion.SacosTotales == 0) return false;
        
        var avgWeightPerBag = negociacion.PesoTotal / negociacion.SacosTotales;
        
        // Un saco típico de productos agrícolas pesa entre 20-30 kg
        return avgWeightPerBag >= 20 && avgWeightPerBag <= 30;
    }
}