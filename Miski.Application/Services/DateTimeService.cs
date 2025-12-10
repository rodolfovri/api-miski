using Microsoft.Extensions.Configuration;

namespace Miski.Application.Services;

/// <summary>
/// Implementación del servicio de conversión de fecha/hora
/// Maneja la conversión entre UTC y la zona horaria local configurada
/// </summary>
public class DateTimeService : IDateTimeService
{
    private readonly TimeZoneInfo _timeZone;
    private readonly string _timeZoneId;

    public DateTimeService(IConfiguration configuration)
    {
        // Obtener el ID de zona horaria desde configuración o usar Perú por defecto
        _timeZoneId = configuration["TimeZone:Id"] ?? "SA Pacific Standard Time";
        
        try
        {
            _timeZone = TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId);
        }
        catch (TimeZoneNotFoundException)
        {
            // Si falla, intentar con el formato alternativo (Linux)
            try
            {
                _timeZoneId = configuration["TimeZone:IanaId"] ?? "America/Lima";
                _timeZone = TimeZoneInfo.FindSystemTimeZoneById(_timeZoneId);
            }
            catch
            {
                // Fallback a UTC si todo falla
                _timeZone = TimeZoneInfo.Utc;
                _timeZoneId = "UTC";
            }
        }
    }

    /// <summary>
    /// Convierte una fecha UTC a la zona horaria local configurada
    /// </summary>
    public DateTime ConvertToLocalTime(DateTime utcDateTime)
    {
        // Si la fecha no tiene Kind especificado, asumimos que es UTC
        if (utcDateTime.Kind == DateTimeKind.Unspecified)
        {
            utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
        }

        // Si ya es UTC, convertir
        if (utcDateTime.Kind == DateTimeKind.Utc)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, _timeZone);
        }

        // Si ya es local, retornar como está
        return utcDateTime;
    }

    /// <summary>
    /// Convierte una fecha UTC nullable a la zona horaria local configurada
    /// </summary>
    public DateTime? ConvertToLocalTime(DateTime? utcDateTime)
    {
        if (!utcDateTime.HasValue)
            return null;

        return ConvertToLocalTime(utcDateTime.Value);
    }

    /// <summary>
    /// Convierte una fecha local a UTC
    /// </summary>
    public DateTime ConvertToUtc(DateTime localDateTime)
    {
        // Si ya es UTC, retornar como está
        if (localDateTime.Kind == DateTimeKind.Utc)
        {
            return localDateTime;
        }

        // Si es Unspecified, asumimos que es hora local
        if (localDateTime.Kind == DateTimeKind.Unspecified)
        {
            localDateTime = DateTime.SpecifyKind(localDateTime, DateTimeKind.Local);
        }

        return TimeZoneInfo.ConvertTimeToUtc(localDateTime, _timeZone);
    }

    /// <summary>
    /// Obtiene la fecha/hora actual en la zona horaria local
    /// </summary>
    public DateTime GetLocalNow()
    {
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone);
    }

    /// <summary>
    /// Obtiene el nombre de la zona horaria configurada
    /// </summary>
    public string GetTimeZoneId()
    {
        return _timeZoneId;
    }
}
