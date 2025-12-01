using MediatR;
using Miski.Domain.Contracts;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.FAQ.FAQs.Commands.DeleteFAQ;

public class DeleteFAQHandler : IRequestHandler<DeleteFAQCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFAQHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteFAQCommand request, CancellationToken cancellationToken)
    {
        var faq = await _unitOfWork.Repository<Domain.Entities.FAQ>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (faq == null)
            throw new NotFoundException("FAQ", request.Id);

        // Cambiar estado a INACTIVO (eliminación lógica)
        faq.Estado = "INACTIVO";

        await _unitOfWork.Repository<Domain.Entities.FAQ>()
            .UpdateAsync(faq, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
