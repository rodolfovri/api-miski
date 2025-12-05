using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.Compras.Queries.GetCompraById;

public class GetCompraByIdHandler : IRequestHandler<GetCompraByIdQuery, CompraDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCompraByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CompraDto> Handle(GetCompraByIdQuery request, CancellationToken cancellationToken)
    {
        var compra = await _unitOfWork.Repository<Compra>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (compra == null)
            throw new NotFoundException("Compra", request.Id);

        // Cargar negociación
        var negociacion = await _unitOfWork.Repository<Negociacion>()
            .GetByIdAsync(compra.IdNegociacion, cancellationToken);

        // ? Cargar lote asociado a esta compra (relación 1:1)
        Lote? loteCompra = null;
        if (compra.IdLote.HasValue)
        {
            loteCompra = await _unitOfWork.Repository<Lote>()
                .GetByIdAsync(compra.IdLote.Value, cancellationToken);
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
            TipoPago = compra.TipoPago,

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

        if (negociacion != null)
        {
            // Buscar proveedor por documento si existe
            if (!string.IsNullOrEmpty(negociacion.NroDocumentoProveedor))
            {
                var personas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
                var proveedor = personas.FirstOrDefault(p => p.NumeroDocumento == negociacion.NroDocumentoProveedor);
                
                if (proveedor != null)
                {
                    compraDto.ProveedorNombre = $"{proveedor.Nombres} {proveedor.Apellidos}";
                }
            }

            // Cargar comisionista
            var comisionista = await _unitOfWork.Repository<Persona>()
                .GetByIdAsync(negociacion.IdComisionista, cancellationToken);
            
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

        return compraDto;
    }
}
