using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Personas.Queries.GetPersonas;

public class GetPersonasHandler : IRequestHandler<GetPersonasQuery, List<PersonaDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPersonasHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<PersonaDto>> Handle(GetPersonasQuery request, CancellationToken cancellationToken)
    {
        var personas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
        var tiposDocumento = await _unitOfWork.Repository<TipoDocumento>().GetAllAsync(cancellationToken);

        // Aplicar filtros
        if (!string.IsNullOrEmpty(request.NumeroDocumento))
        {
            personas = personas.Where(p => 
                p.NumeroDocumento.Contains(request.NumeroDocumento, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (!string.IsNullOrEmpty(request.Nombres))
        {
            personas = personas.Where(p =>
                p.Nombres.Contains(request.Nombres, StringComparison.OrdinalIgnoreCase) ||
                p.Apellidos.Contains(request.Nombres, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (!string.IsNullOrEmpty(request.Estado))
        {
            personas = personas.Where(p => p.Estado == request.Estado).ToList();
        }

        // Cargar relaciones
        foreach (var persona in personas)
        {
            persona.TipoDocumento = tiposDocumento.FirstOrDefault(td => 
                td.IdTipoDocumento == persona.IdTipoDocumento);
        }

        return personas.Select(p => _mapper.Map<PersonaDto>(p)).ToList();
    }
}