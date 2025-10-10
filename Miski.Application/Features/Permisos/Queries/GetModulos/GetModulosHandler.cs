using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Permisos;

namespace Miski.Application.Features.Permisos.Queries.GetModulos;

public class GetModulosHandler : IRequestHandler<GetModulosQuery, List<ModuloDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetModulosHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<ModuloDto>> Handle(GetModulosQuery request, CancellationToken cancellationToken)
    {
        var modulos = await _unitOfWork.Repository<Modulo>().GetAllAsync(cancellationToken);
        var subModulos = await _unitOfWork.Repository<SubModulo>().GetAllAsync(cancellationToken);
        var detalles = await _unitOfWork.Repository<SubModuloDetalle>().GetAllAsync(cancellationToken);

        // Filtrar por tipo de plataforma si se especifica
        if (!string.IsNullOrEmpty(request.TipoPlataforma))
        {
            modulos = modulos.Where(m => m.TipoPlataforma == request.TipoPlataforma).ToList();
        }

        var resultado = modulos
            .Where(m => m.Estado == "ACTIVO")
            .OrderBy(m => m.Orden)
            .Select(modulo => new ModuloDto
            {
                IdModulo = modulo.IdModulo,
                Nombre = modulo.Nombre,
                Orden = modulo.Orden,
                Estado = modulo.Estado,
                TipoPlataforma = modulo.TipoPlataforma,
                SubModulos = subModulos
                    .Where(sm => sm.IdModulo == modulo.IdModulo && sm.Estado == "ACTIVO")
                    .OrderBy(sm => sm.Orden)
                    .Select(subModulo => new SubModuloDto
                    {
                        IdSubModulo = subModulo.IdSubModulo,
                        IdModulo = subModulo.IdModulo,
                        Nombre = subModulo.Nombre,
                        Orden = subModulo.Orden,
                        Estado = subModulo.Estado,
                        ModuloNombre = modulo.Nombre,
                        SubModuloDetalles = detalles
                            .Where(d => d.IdSubModulo == subModulo.IdSubModulo && d.Estado == "ACTIVO")
                            .OrderBy(d => d.Orden)
                            .Select(detalle => new SubModuloDetalleDto
                            {
                                IdSubModuloDetalle = detalle.IdSubModuloDetalle,
                                IdSubModulo = detalle.IdSubModulo,
                                Nombre = detalle.Nombre,
                                Orden = detalle.Orden,
                                Estado = detalle.Estado,
                                SubModuloNombre = subModulo.Nombre
                            })
                            .ToList()
                    })
                    .ToList()
            })
            .ToList();

        return resultado;
    }
}