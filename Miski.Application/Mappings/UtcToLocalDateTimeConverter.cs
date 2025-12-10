using AutoMapper;
using Miski.Application.Services;

namespace Miski.Application.Mappings;

/// <summary>
/// Convertidor personalizado de AutoMapper para convertir DateTime UTC a hora local
/// Se aplica automáticamente a todas las propiedades DateTime en los mapeos
/// </summary>
public class UtcToLocalDateTimeConverter : IValueConverter<DateTime, DateTime>, IValueConverter<DateTime?, DateTime?>
{
    private readonly IDateTimeService _dateTimeService;

    public UtcToLocalDateTimeConverter(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    // Conversión para DateTime no nullable
    public DateTime Convert(DateTime sourceMember, ResolutionContext context)
    {
        return _dateTimeService.ConvertToLocalTime(sourceMember);
    }

    // Conversión para DateTime nullable
    public DateTime? Convert(DateTime? sourceMember, ResolutionContext context)
    {
        return _dateTimeService.ConvertToLocalTime(sourceMember);
    }
}
