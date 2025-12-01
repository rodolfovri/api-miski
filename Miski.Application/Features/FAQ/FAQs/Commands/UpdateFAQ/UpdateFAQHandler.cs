using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Shared.DTOs.FAQ;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.FAQ.FAQs.Commands.UpdateFAQ;

public class UpdateFAQHandler : IRequestHandler<UpdateFAQCommand, FAQDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateFAQHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<FAQDto> Handle(UpdateFAQCommand request, CancellationToken cancellationToken)
    {
        var dto = request.FAQ;

        var faq = await _unitOfWork.Repository<Domain.Entities.FAQ>()
            .GetByIdAsync(dto.IdFAQ, cancellationToken);

        if (faq == null)
            throw new NotFoundException("FAQ", dto.IdFAQ);

        // Validar que la categoría existe si se cambió
        if (faq.IdCategoriaFAQ != dto.IdCategoriaFAQ)
        {
            var categoriaValidacion = await _unitOfWork.Repository<Domain.Entities.CategoriaFAQ>()
                .GetByIdAsync(dto.IdCategoriaFAQ, cancellationToken);

            if (categoriaValidacion == null)
                throw new NotFoundException("CategoriaFAQ", dto.IdCategoriaFAQ);
        }

        faq.IdCategoriaFAQ = dto.IdCategoriaFAQ;
        faq.Pregunta = dto.Pregunta;
        faq.Respuesta = dto.Respuesta;
        faq.Estado = dto.Estado;
        faq.Orden = dto.Orden;

        await _unitOfWork.Repository<Domain.Entities.FAQ>()
            .UpdateAsync(faq, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Cargar relación
        var categoria = await _unitOfWork.Repository<Domain.Entities.CategoriaFAQ>()
            .GetByIdAsync(faq.IdCategoriaFAQ, cancellationToken);
        faq.CategoriaFAQ = categoria!;

        return _mapper.Map<FAQDto>(faq);
    }
}
