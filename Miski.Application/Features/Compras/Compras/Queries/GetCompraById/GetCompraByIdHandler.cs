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

        // Cargar negociaci�n
        var negociacion = await _unitOfWork.Repository<Negociacion>()
            .GetByIdAsync(compra.IdNegociacion, cancellationToken);

        // Cargar lotes asociados a esta compra
        var lotes = await _unitOfWork.Repository<Lote>().GetAllAsync(cancellationToken);
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

        // Cargar lotes en el DTO
        compraDto.Lotes = lotesCompra.Select(l => _mapper.Map<LoteDto>(l)).ToList();

        return compraDto;
    }
}
