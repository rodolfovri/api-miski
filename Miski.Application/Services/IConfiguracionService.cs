namespace Miski.Application.Services;

/// <summary>
/// Servicio para obtener configuraciones globales de la base de datos
/// </summary>
public interface IConfiguracionService
{
    /// <summary>
    /// Obtiene un valor decimal desde la configuración global
    /// </summary>
    /// <param name="clave">Clave de la configuración</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Valor decimal de la configuración</returns>
    /// <exception cref="Miski.Shared.Exceptions.NotFoundException">Si la configuración no existe</exception>
    /// <exception cref="System.InvalidOperationException">Si el valor no es un decimal válido</exception>
    Task<decimal> ObtenerDecimalAsync(string clave, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un valor entero desde la configuración global
    /// </summary>
    /// <param name="clave">Clave de la configuración</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Valor entero de la configuración</returns>
    /// <exception cref="Miski.Shared.Exceptions.NotFoundException">Si la configuración no existe</exception>
    /// <exception cref="System.InvalidOperationException">Si el valor no es un entero válido</exception>
    Task<int> ObtenerEnteroAsync(string clave, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un valor booleano desde la configuración global
    /// </summary>
    /// <param name="clave">Clave de la configuración</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Valor booleano de la configuración</returns>
    /// <exception cref="Miski.Shared.Exceptions.NotFoundException">Si la configuración no existe</exception>
    /// <exception cref="System.InvalidOperationException">Si el valor no es un booleano válido</exception>
    Task<bool> ObtenerBooleanoAsync(string clave, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un valor de texto desde la configuración global
    /// </summary>
    /// <param name="clave">Clave de la configuración</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Valor de texto de la configuración</returns>
    /// <exception cref="Miski.Shared.Exceptions.NotFoundException">Si la configuración no existe</exception>
    Task<string> ObtenerTextoAsync(string clave, CancellationToken cancellationToken = default);
}
