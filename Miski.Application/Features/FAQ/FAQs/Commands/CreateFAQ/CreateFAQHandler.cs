using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.FAQ;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.FAQ.FAQs.Commands.CreateFAQ;

public class CreateFAQHandler : IRequestHandler<CreateFAQCommand, FAQDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateFAQHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<FAQDto> Handle(CreateFAQCommand request, CancellationToken cancellationToken)
    {
        var dto = request.FAQ;

        // Validar que la categoría existe
        var categoria = await _unitOfWork.Repository<Domain.Entities.CategoriaFAQ>()
            .GetByIdAsync(dto.IdCategoriaFAQ, cancellationToken);

        if (categoria == null)
            throw new NotFoundException("CategoriaFAQ", dto.IdCategoriaFAQ);

        var faq = new Domain.Entities.FAQ
        {
            IdCategoriaFAQ = dto.IdCategoriaFAQ,
            Pregunta = dto.Pregunta,
            Respuesta = dto.Respuesta,
            Estado = dto.Estado,
            Orden = dto.Orden,
            FRegistro = DateTime.UtcNow
        };

        var faqCreado = await _unitOfWork.Repository<Domain.Entities.FAQ>()
            .AddAsync(faq, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar relación
        faqCreado.CategoriaFAQ = categoria;

        return _mapper.Map<FAQDto>(faqCreado);
    }
}
