using Microsoft.EntityFrameworkCore;
using Miski.Domain.Contracts.Repositories;
using Miski.Domain.Entities;
using Miski.Infrastructure.Data;

namespace Miski.Infrastructure.Repositories;

public class NegociacionRepository : Repository<Negociacion>, INegociacionRepository
{
    public NegociacionRepository(MiskiDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Negociacion>> GetByProveedorIdAsync(int proveedorId, CancellationToken cancellationToken = default)
    {
        // Buscar por documento del proveedor ya que ahora usamos NroDocumentoProveedor
        var proveedor = await _context.Set<Persona>().FindAsync(new object[] { proveedorId }, cancellationToken);
        if (proveedor == null) return new List<Negociacion>();

        return await _dbSet
            .Include(n => n.Proveedor)
            .Include(n => n.Comisionista)
            .Include(n => n.VariedadProducto)  
                .ThenInclude(v => v.Producto)   
            .Where(n => n.NroDocumentoProveedor == proveedor.NumeroDocumento)
            .OrderByDescending(n => n.FRegistro)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Negociacion>> GetByComisionistaIdAsync(int comisionistaId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(n => n.Proveedor)
            .Include(n => n.Comisionista)
            .Include(n => n.VariedadProducto) 
                .ThenInclude(v => v.Producto)  
            .Where(n => n.IdComisionista == comisionistaId)
            .OrderByDescending(n => n.FRegistro)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Negociacion>> GetByEstadoAsync(string estado, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(n => n.Proveedor)
            .Include(n => n.Comisionista)
            .Include(n => n.VariedadProducto) 
                .ThenInclude(v => v.Producto)
            .Where(n => n.Estado == estado)
            .OrderByDescending(n => n.FRegistro)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Negociacion>> GetPendientesAprobacionAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(n => n.Proveedor)
            .Include(n => n.Comisionista)
            .Include(n => n.VariedadProducto)
                .ThenInclude(v => v.Producto) 
            .Where(n => n.EstadoAprobacionIngeniero == "PENDIENTE")
            .OrderBy(n => n.FRegistro)
            .ToListAsync(cancellationToken);
    }

    public async Task<Negociacion?> GetWithRelacionesAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(n => n.Proveedor)
            .Include(n => n.Comisionista)
            .Include(n => n.VariedadProducto)
                .ThenInclude(v => v.Producto)
            .Include(n => n.AprobadaPorUsuarioIngeniero)
                .ThenInclude(u => u.Persona)
            .Include(n => n.AprobadaPorUsuarioContadora)
                .ThenInclude(u => u.Persona)
            .Include(n => n.RechazadoPorUsuarioIngeniero)
                .ThenInclude(u => u.Persona)
            .Include(n => n.RechazadoPorUsuarioContadora)
                .ThenInclude(u => u.Persona)
            .FirstOrDefaultAsync(n => n.IdNegociacion == id, cancellationToken);
    }

    public override async Task<IEnumerable<Negociacion>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(n => n.Proveedor)
            .Include(n => n.Comisionista)
            .Include(n => n.VariedadProducto)
                .ThenInclude(v => v.Producto)
            .OrderByDescending(n => n.FRegistro)
            .ToListAsync(cancellationToken);
    }
}