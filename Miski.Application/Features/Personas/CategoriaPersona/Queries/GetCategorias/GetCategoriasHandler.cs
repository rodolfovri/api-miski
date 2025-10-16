using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.CategoriaPersona.Queries.GetCategorias;

public class GetCategoriasHandler : IRequestHandler<GetCategoriasQuery, List<CategoriaPersonaDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoriasHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CategoriaPersonaDto>> Handle(GetCategoriasQuery request, CancellationToken cancellationToken)
    {
        var categorias = await _unitOfWork.Repository<Domain.Entities.CategoriaPersona>().GetAllAsync(cancellationToken);

        // Aplicar filtro por nombre si se proporciona
        if (!string.IsNullOrEmpty(request.Nombre))
        {
            categorias = categorias.Where(c =>
                c.Nombre.Contains(request.Nombre, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return categorias.Select(c => _mapper.Map<CategoriaPersonaDto>(c)).ToList();
    }
}