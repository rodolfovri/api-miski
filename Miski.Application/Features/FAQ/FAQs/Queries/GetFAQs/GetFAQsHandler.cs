using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.FAQ;

namespace Miski.Application.Features.FAQ.FAQs.Queries.GetFAQs;

public class GetFAQsHandler : IRequestHandler<GetFAQsQuery, List<FAQDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetFAQsHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<FAQDto>> Handle(GetFAQsQuery request, CancellationToken cancellationToken)
    {
        var faqs = await _unitOfWork.Repository<Domain.Entities.FAQ>()
            .GetAllAsync(cancellationToken);

        var categorias = await _unitOfWork.Repository<Domain.Entities.CategoriaFAQ>()
            .GetAllAsync(cancellationToken);

        // Aplicar filtros
        if (request.IdCategoriaFAQ.HasValue)
        {
            faqs = faqs
                .Where(f => f.IdCategoriaFAQ == request.IdCategoriaFAQ.Value)
                .ToList();
        }

        if (!string.IsNullOrEmpty(request.Estado))
        {
            faqs = faqs
                .Where(f => f.Estado == request.Estado)
                .ToList();
        }

        // Cargar relaciones
        foreach (var faq in faqs)
        {
            faq.CategoriaFAQ = categorias.FirstOrDefault(c => c.IdCategoriaFAQ == faq.IdCategoriaFAQ)!;
        }

        // Ordenar por orden
        var faqsOrdenados = faqs.OrderBy(f => f.Orden).ToList();

        return faqsOrdenados.Select(f => _mapper.Map<FAQDto>(f)).ToList();
    }
}
