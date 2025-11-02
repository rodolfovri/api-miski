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

        // Filtrar compras sin asignar (que NO están en CompraVehiculoDetalle)
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
            
            // Cargar lotes asociados a esta compra
            var lotesCompra = lotes.Where(l => l.IdCompra == compra.IdCompra).ToList();
            
            // Calcular PesoTotal y SacosTotales desde los lotes
            var pesoTotal = lotesCompra.Sum(l => l.Peso);
            var sacosTotales = lotesCompra.Sum(l => l.Sacos);
            
            var compraDto = new CompraDto
            {
                IdCompra = compra.IdCompra,
                IdNegociacion = compra.IdNegociacion,
                Serie = compra.Serie,
                FRegistro = compra.FRegistro,
                FEmision = compra.FEmision,
                Estado = compra.Estado,
                EstadoRecepcion = compra.EstadoRecepcion,
                MontoTotal = compra.MontoTotal ?? 0, // MontoTotal de Compra
                PesoTotal = pesoTotal, // PesoTotal desde Lotes
                SacosTotales = sacosTotales, // SacosTotales desde Lotes
                PrecioUnitario = negociacion?.PrecioUnitario ?? 0 // PrecioUnitario desde Negociacion
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

            // Cargar lotes en el DTO
            compraDto.Lotes = lotesCompra.Select(l => _mapper.Map<LoteDto>(l)).ToList();

            comprasDto.Add(compraDto);
        }

        return comprasDto.OrderByDescending(c => c.FRegistro).ToList();
    }
}
