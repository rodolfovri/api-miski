using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.FAQ;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.FAQ.FAQs.Queries.GetFAQById;

public class GetFAQByIdHandler : IRequestHandler<GetFAQByIdQuery, FAQDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetFAQByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<FAQDto> Handle(GetFAQByIdQuery request, CancellationToken cancellationToken)
    {
        var faq = await _unitOfWork.Repository<Domain.Entities.FAQ>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (faq == null)
            throw new NotFoundException("FAQ", request.Id);

        // Cargar relación
        var categoria = await _unitOfWork.Repository<Domain.Entities.CategoriaFAQ>()
            .GetByIdAsync(faq.IdCategoriaFAQ, cancellationToken);
        faq.CategoriaFAQ = categoria!;

        return _mapper.Map<FAQDto>(faq);
    }
}
