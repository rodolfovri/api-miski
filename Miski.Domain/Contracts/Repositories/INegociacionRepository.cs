using Miski.Domain.Entities;

namespace Miski.Domain.Contracts.Repositories;

public interface INegociacionRepository : IRepository<Negociacion>
{
    Task<IEnumerable<Negociacion>> GetByProveedorIdAsync(int proveedorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Negociacion>> GetByComisionistaIdAsync(int comisionistaId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Negociacion>> GetByEstadoAsync(string estado, CancellationToken cancellationToken = default);
    Task<IEnumerable<Negociacion>> GetPendientesAprobacionAsync(CancellationToken cancellationToken = default);
    Task<Negociacion?> GetWithRelacionesAsync(int id, CancellationToken cancellationToken = default);
}