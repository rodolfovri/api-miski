using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.TipoCalidadProducto.Queries.GetTipoCalidadProductoById;

public class GetTipoCalidadProductoByIdHandler : IRequestHandler<GetTipoCalidadProductoByIdQuery, TipoCalidadProductoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTipoCalidadProductoByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TipoCalidadProductoDto> Handle(GetTipoCalidadProductoByIdQuery request, CancellationToken cancellationToken)
    {
        var tipoCalidadProducto = await _unitOfWork.Repository<Domain.Entities.TipoCalidadProducto>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (tipoCalidadProducto == null)
            throw new NotFoundException("TipoCalidadProducto", request.Id);

        // Cargar relación del producto
        tipoCalidadProducto.Producto = await _unitOfWork.Repository<Producto>()
            .GetByIdAsync(tipoCalidadProducto.IdProducto, cancellationToken);

        return _mapper.Map<TipoCalidadProductoDto>(tipoCalidadProducto);
    }
}
