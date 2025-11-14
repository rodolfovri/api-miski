using MediatR;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.Exceptions;
using Miski.Application.Services;

namespace Miski.Application.Features.Ubicaciones.Commands.DeleteUbicacion;

public class DeleteUbicacionHandler : IRequestHandler<DeleteUbicacionCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;

    public DeleteUbicacionHandler(IUnitOfWork unitOfWork, IFileStorageService fileStorageService)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
    }

    public async Task<bool> Handle(DeleteUbicacionCommand request, CancellationToken cancellationToken)
    {
        var ubicacion = await _unitOfWork.Repository<Ubicacion>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (ubicacion == null)
            throw new NotFoundException("Ubicacion", request.Id);

        // Verificar si hay stock asociado a esta ubicación
        var stocks = await _unitOfWork.Repository<Stock>().GetAllAsync(cancellationToken);
        var tieneStock = stocks.Any(s => s.IdPlanta == request.Id);

        if (tieneStock)
        {
            throw new ValidationException("No se puede eliminar la ubicación porque tiene stock asociado");
        }

        // Eliminar el PDF asociado si existe
        if (!string.IsNullOrEmpty(ubicacion.ComprobantePdf))
        {
            await _fileStorageService.DeleteFileAsync(ubicacion.ComprobantePdf, cancellationToken);
        }

        // Cambiar estado a INACTIVO en lugar de eliminar físicamente
        ubicacion.Estado = "INACTIVO";
        await _unitOfWork.Repository<Ubicacion>().UpdateAsync(ubicacion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}