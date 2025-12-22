using Miski.Domain.Contracts.Repositories;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Services;

/// <summary>
/// Implementación del servicio para obtener configuraciones globales de la base de datos
/// </summary>
public class ConfiguracionService : IConfiguracionService
{
    private readonly IRepository<ConfiguracionGlobal> _repository;

    public ConfiguracionService(IRepository<ConfiguracionGlobal> repository)
    {
        _repository = repository;
    }

    public async Task<decimal> ObtenerDecimalAsync(string clave, CancellationToken cancellationToken = default)
    {
        var configuracion = await ObtenerConfiguracionAsync(clave, cancellationToken);
        
        if (!decimal.TryParse(configuracion.Valor, out decimal resultado))
            throw new InvalidOperationException($"El valor de la configuración '{clave}' no es un decimal válido. Valor actual: '{configuracion.Valor}'");
        
        return resultado;
    }

    public async Task<int> ObtenerEnteroAsync(string clave, CancellationToken cancellationToken = default)
    {
        var configuracion = await ObtenerConfiguracionAsync(clave, cancellationToken);
        
        if (!int.TryParse(configuracion.Valor, out int resultado))
            throw new InvalidOperationException($"El valor de la configuración '{clave}' no es un entero válido. Valor actual: '{configuracion.Valor}'");
        
        return resultado;
    }

    public async Task<bool> ObtenerBooleanoAsync(string clave, CancellationToken cancellationToken = default)
    {
        var configuracion = await ObtenerConfiguracionAsync(clave, cancellationToken);
        
        if (!bool.TryParse(configuracion.Valor, out bool resultado))
            throw new InvalidOperationException($"El valor de la configuración '{clave}' no es un booleano válido. Valor actual: '{configuracion.Valor}'");
        
        return resultado;
    }

    public async Task<string> ObtenerTextoAsync(string clave, CancellationToken cancellationToken = default)
    {
        var configuracion = await ObtenerConfiguracionAsync(clave, cancellationToken);
        return configuracion.Valor;
    }

    private async Task<ConfiguracionGlobal> ObtenerConfiguracionAsync(string clave, CancellationToken cancellationToken)
    {
        var configuraciones = await _repository.FindAsync(c => c.Clave == clave, cancellationToken);
        var configuracion = configuraciones.FirstOrDefault();
        
        if (configuracion == null)
            throw new NotFoundException("ConfiguracionGlobal", clave);
        
        return configuracion;
    }
}
