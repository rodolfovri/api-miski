using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;

namespace Miski.Application.Features.Personas.Commands.RemoverCategoria;

public class RemoverCategoriaHandler : IRequestHandler<RemoverCategoriaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoverCategoriaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(RemoverCategoriaCommand request, CancellationToken cancellationToken)
    {
        // Buscar la relación persona-categoría
        var personaCategorias = await _unitOfWork.Repository<PersonaCategoria>().GetAllAsync(cancellationToken);
        var personaCategoria = personaCategorias.FirstOrDefault(pc =>
            pc.IdPersona == request.PersonaId &&
            pc.IdCategoria == request.CategoriaId);

        if (personaCategoria == null)
        {
            throw new NotFoundException(
                "PersonaCategoria", 
                $"PersonaId: {request.PersonaId}, CategoriaId: {request.CategoriaId}");
        }

        await _unitOfWork.Repository<PersonaCategoria>().DeleteAsync(personaCategoria, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}