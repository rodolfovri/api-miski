using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.FAQ;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.FAQ.CategoriaFAQ.Queries.GetCategoriaById;

public class GetCategoriaByIdHandler : IRequestHandler<GetCategoriaByIdQuery, CategoriaFAQDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoriaByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoriaFAQDto> Handle(GetCategoriaByIdQuery request, CancellationToken cancellationToken)
    {
        var categoria = await _unitOfWork.Repository<Domain.Entities.CategoriaFAQ>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (categoria == null)
            throw new NotFoundException("CategoriaFAQ", request.Id);

        return _mapper.Map<CategoriaFAQDto>(categoria);
    }
}
