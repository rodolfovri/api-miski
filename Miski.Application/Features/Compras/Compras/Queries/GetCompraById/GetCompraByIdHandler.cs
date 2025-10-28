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

        var compraDto = new CompraDto
        {
            IdCompra = compra.IdCompra,
            IdNegociacion = compra.IdNegociacion,
            Serie = compra.Serie,
            FRegistro = compra.FRegistro,
            FEmision = compra.FEmision,
            Estado = compra.Estado
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

            compraDto.PesoTotal = negociacion.PesoTotal ?? 0;
            compraDto.SacosTotales = negociacion.SacosTotales ?? 0;
            compraDto.PrecioUnitario = negociacion.PrecioUnitario ?? 0;
            compraDto.MontoTotal = (negociacion.PesoTotal ?? 0) * (negociacion.PrecioUnitario ?? 0);
        }

        // Cargar lotes
        var lotes = await _unitOfWork.Repository<Lote>().GetAllAsync(cancellationToken);
        var lotesCompra = lotes.Where(l => l.IdCompra == compra.IdCompra).ToList();
        compraDto.Lotes = lotesCompra.Select(l => _mapper.Map<LoteDto>(l)).ToList();

        return compraDto;
    }
}
