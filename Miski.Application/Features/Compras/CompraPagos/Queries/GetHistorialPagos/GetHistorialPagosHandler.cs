using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Compras.CompraPagos.Queries.GetHistorialPagos;

public class GetHistorialPagosHandler : IRequestHandler<GetHistorialPagosQuery, List<CompraPagoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetHistorialPagosHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CompraPagoDto>> Handle(GetHistorialPagosQuery request, CancellationToken cancellationToken)
    {
        // Validar que la compra existe
        var compra = await _unitOfWork.Repository<Compra>().GetByIdAsync(request.IdCompra, cancellationToken);
        if (compra == null)
            throw new NotFoundException("Compra", request.IdCompra);

        // Obtener todos los pagos de la compra
        var todosPagos = await _unitOfWork.Repository<CompraPago>().GetAllAsync(cancellationToken);
        var pagosCompra = todosPagos
            .Where(p => p.IdCompra == request.IdCompra)
            .OrderBy(p => p.FRegistro)
            .ToList();

        var pagosDtos = pagosCompra.Select(p =>
        {
            var dto = _mapper.Map<CompraPagoDto>(p);
            dto.CompraSerie = compra.Serie;
            dto.CompraMontoTotal = compra.MontoTotal;
            return dto;
        }).ToList();

        return pagosDtos;
    }
}
