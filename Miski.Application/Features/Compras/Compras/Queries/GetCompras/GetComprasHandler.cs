using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Compras.Queries.GetCompras;

public class GetComprasHandler : IRequestHandler<GetComprasQuery, List<CompraDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetComprasHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CompraDto>> Handle(GetComprasQuery request, CancellationToken cancellationToken)
    {
        var compras = await _unitOfWork.Repository<Compra>().GetAllAsync(cancellationToken);
        var negociaciones = await _unitOfWork.Repository<Negociacion>().GetAllAsync(cancellationToken);
        var personas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
        var lotes = await _unitOfWork.Repository<Lote>().GetAllAsync(cancellationToken);

        // Aplicar filtros
        if (!string.IsNullOrEmpty(request.Estado))
        {
            compras = compras.Where(c => c.Estado == request.Estado).ToList();
        }

        if (request.IdNegociacion.HasValue)
        {
            compras = compras.Where(c => c.IdNegociacion == request.IdNegociacion.Value).ToList();
        }

        var comprasDto = new List<CompraDto>();

        foreach (var compra in compras)
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
                MontoTotal = compra.MontoTotal ?? 0,
                
                // Totales calculados desde los Lotes
                PesoTotal = pesoTotal,
                SacosTotales = sacosTotales,
                
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

            // Cargar lotes en el DTO
            compraDto.Lotes = lotesCompra.Select(l => _mapper.Map<LoteDto>(l)).ToList();

            comprasDto.Add(compraDto);
        }

        return comprasDto.OrderByDescending(c => c.FRegistro).ToList();
    }
}
