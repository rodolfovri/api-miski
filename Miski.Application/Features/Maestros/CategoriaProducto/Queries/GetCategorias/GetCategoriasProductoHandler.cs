using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.CategoriaProducto.Queries.GetCategorias;

public class GetCategoriasProductoHandler : IRequestHandler<GetCategoriasProductoQuery, List<CategoriaProductoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoriasProductoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CategoriaProductoDto>> Handle(GetCategoriasProductoQuery request, CancellationToken cancellationToken)
    {
        var categorias = await _unitOfWork.Repository<Domain.Entities.CategoriaProducto>().GetAllAsync(cancellationToken);

        // Aplicar filtros
        if (!string.IsNullOrEmpty(request.Nombre))
        {
            categorias = categorias.Where(c =>
                c.Nombre.Contains(request.Nombre, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (!string.IsNullOrEmpty(request.Estado))
        {
            categorias = categorias.Where(c => c.Estado == request.Estado).ToList();
        }

        return categorias.Select(c => _mapper.Map<CategoriaProductoDto>(c)).ToList();
    }
}