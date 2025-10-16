using AutoMapper;
using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Maestros;

namespace Miski.Application.Features.Maestros.TipoDocumento.Queries.GetTipoDocumento;

public class GetTipoDocumentoHandler : IRequestHandler<GetTipoDocumentoQuery, List<TipoDocumentoDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTipoDocumentoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<TipoDocumentoDto>> Handle(GetTipoDocumentoQuery request, CancellationToken cancellationToken)
    {
        var tiposDocumento = await _unitOfWork.Repository<Domain.Entities.TipoDocumento>().GetAllAsync(cancellationToken);

        // Aplicar filtro por nombre si se proporciona
        if (!string.IsNullOrEmpty(request.Nombre))
        {
            tiposDocumento = tiposDocumento.Where(td =>
                td.Nombre.Contains(request.Nombre, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return tiposDocumento.Select(td => _mapper.Map<TipoDocumentoDto>(td)).ToList();
    }
}
