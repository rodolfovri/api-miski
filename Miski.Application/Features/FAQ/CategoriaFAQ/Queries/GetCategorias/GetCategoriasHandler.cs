using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.FAQ;

namespace Miski.Application.Features.FAQ.CategoriaFAQ.Queries.GetCategorias;

public class GetCategoriasHandler : IRequestHandler<GetCategoriasQuery, List<CategoriaFAQDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoriasHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CategoriaFAQDto>> Handle(GetCategoriasQuery request, CancellationToken cancellationToken)
    {
        var categorias = await _unitOfWork.Repository<Domain.Entities.CategoriaFAQ>()
            .GetAllAsync(cancellationToken);

        // Aplicar filtro por estado si se proporciona
        if (!string.IsNullOrEmpty(request.Estado))
        {
            categorias = categorias
                .Where(c => c.Estado == request.Estado)
                .ToList();
        }

        return categorias.Select(c => _mapper.Map<CategoriaFAQDto>(c)).ToList();
    }
}
