using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Maestros.CategoriaProducto.Queries.GetCategoriaById;

public class GetCategoriaProductoByIdHandler : IRequestHandler<GetCategoriaProductoByIdQuery, CategoriaProductoDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoriaProductoByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoriaProductoDto> Handle(GetCategoriaProductoByIdQuery request, CancellationToken cancellationToken)
    {
        var categoria = await _unitOfWork.Repository<Domain.Entities.CategoriaProducto>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (categoria == null)
            throw new NotFoundException("CategoriaProducto", request.Id);

        return _mapper.Map<CategoriaProductoDto>(categoria);
    }
}