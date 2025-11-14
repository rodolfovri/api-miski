using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Compras.Queries.GetComprasSinAsignar;

public class GetComprasSinAsignarHandler : IRequestHandler<GetComprasSinAsignarQuery, List<CompraDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetComprasSinAsignarHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CompraDto>> Handle(GetComprasSinAsignarQuery request, CancellationToken cancellationToken)
    {
        // Obtener todas las compras con estado ACTIVO
        var compras = await _unitOfWork.Repository<Compra>().GetAllAsync(cancellationToken);
        compras = compras.Where(c => c.Estado == "ACTIVO").ToList();

        // Obtener todos los detalles de compras asignadas a vehículos
        var comprasVehiculoDetalles = await _unitOfWork.Repository<CompraVehiculoDetalle>()
            .GetAllAsync(cancellationToken);

        // Obtener IDs de compras que ya están asignadas
        var idsComprasAsignadas = comprasVehiculoDetalles
            .Select(cvd => cvd.IdCompra)
            .Distinct()
            .ToList();

        // Filtrar compras sin asignar (que NO estén en CompraVehiculoDetalle)
        var comprasSinAsignar = compras
            .Where(c => !idsComprasAsignadas.Contains(c.IdCompra))
            .ToList();

        // Cargar relaciones para construir el DTO
        var negociaciones = await _unitOfWork.Repository<Negociacion>().GetAllAsync(cancellationToken);
        var personas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
        var lotes = await _unitOfWork.Repository<Lote>().GetAllAsync(cancellationToken);

        var comprasDto = new List<CompraDto>();

        foreach (var compra in comprasSinAsignar)
        {
            // Cargar negociación
            var negociacion = negociaciones.FirstOrDefault(n => n.IdNegociacion == compra.IdNegociacion);
            
            // ? Cargar lote asociado a esta compra (relación 1:1)
            Lote? loteCompra = null;
            if (compra.IdLote.HasValue)
            {
                loteCompra = lotes.FirstOrDefault(l => l.IdLote == compra.IdLote.Value);
            }
            
            var compraDto = new CompraDto
            {
                IdCompra = compra.IdCompra,
                IdNegociacion = compra.IdNegociacion,
                IdLote = compra.IdLote,  // ? FK al lote (puede ser null)
                Serie = compra.Serie,
                FRegistro = compra.FRegistro,
                FEmision = compra.FEmision,
                Estado = compra.Estado,
                EstadoRecepcion = compra.EstadoRecepcion,
                EsParcial = compra.EsParcial,
                MontoTotal = compra.MontoTotal,
                
                // ? Información del lote (si existe)
                PesoLote = loteCompra?.Peso,
                SacosLote = loteCompra?.Sacos,
                CodigoLote = loteCompra?.Codigo,
                ComisionLote = loteCompra?.Comision,
                
                // Totales originales desde la Negociación
                NegociacionPesoTotal = negociacion?.PesoTotal ?? 0,
                NegociacionSacosTotales = negociacion?.SacosTotales ?? 0,
                
                PrecioUnitario = negociacion?.PrecioUnitario ?? 0
            };

            // Si existe la negociación, cargar información
            if (negociacion != null)
            {
                // Buscar proveedor por documento si existe
                if (!string.IsNullOrEmpty(negociacion.NroDocumentoProveedor))
                {
                    var proveedor = personas.FirstOrDefault(p => p.NumeroDocumento == negociacion.NroDocumentoProveedor);
                    if (proveedor != null)
                    {
                        compraDto.ProveedorNombre = $"{proveedor.Nombres} {proveedor.Apellidos}";
                    }
                }

                // Cargar comisionista
                var comisionista = personas.FirstOrDefault(p => p.IdPersona == negociacion.IdComisionista);
                if (comisionista != null)
                {
                    compraDto.ComisionistaNombre = $"{comisionista.Nombres} {comisionista.Apellidos}";
                }
            }

            // ? Cargar el lote en el DTO (relación 1:1)
            if (loteCompra != null)
            {
                loteCompra.Compra = compra; // Cargar relación inversa
                compraDto.Lote = _mapper.Map<LoteDto>(loteCompra);
            }

            comprasDto.Add(compraDto);
        }

        return comprasDto.OrderByDescending(c => c.FRegistro).ToList();
    }
}
