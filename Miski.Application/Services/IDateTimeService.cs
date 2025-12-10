namespace Miski.Application.Services;

/// <summary>
/// Servicio para manejar conversiones de fecha/hora según la zona horaria configurada
/// </summary>
public interface IDateTimeService
{
    /// <summary>
    /// Convierte una fecha UTC a la zona horaria local configurada
    /// </summary>
    /// <param name="utcDateTime">Fecha en UTC</param>
    /// <returns>Fecha convertida a zona horaria local</returns>
    DateTime ConvertToLocalTime(DateTime utcDateTime);

    /// <summary>
    /// Convierte una fecha UTC nullable a la zona horaria local configurada
    /// </summary>
    /// <param name="utcDateTime">Fecha en UTC (puede ser null)</param>
    /// <returns>Fecha convertida a zona horaria local (o null)</returns>
    DateTime? ConvertToLocalTime(DateTime? utcDateTime);

    /// <summary>
    /// Convierte una fecha local a UTC
    /// </summary>
    /// <param name="localDateTime">Fecha en zona horaria local</param>
    /// <returns>Fecha en UTC</returns>
    DateTime ConvertToUtc(DateTime localDateTime);

    /// <summary>
    /// Obtiene la fecha/hora actual en la zona horaria local
    /// </summary>
    /// <returns>Fecha/hora actual en zona local</returns>
    DateTime GetLocalNow();

    /// <summary>
    /// Obtiene el nombre de la zona horaria configurada
    /// </summary>
    /// <returns>ID de la zona horaria (ejemplo: "SA Pacific Standard Time")</returns>
    string GetTimeZoneId();
}
