using Miski.Domain.Entities;

namespace Miski.Domain.Contracts.Repositories;

public interface IProductoRepository : IRepository<Producto>
{
    Task<IEnumerable<Producto>> GetActivosAsync(CancellationToken cancellationToken = default);
    Task<Producto?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<IEnumerable<Producto>> GetByEstadoAsync(string estado, CancellationToken cancellationToken = default);
}