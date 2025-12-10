using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Compras;

namespace Miski.Application.Features.Compras.Negociaciones.Queries.GetNegociaciones;

public class GetNegociacionesHandler : IRequestHandler<GetNegociacionesQuery, List<NegociacionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetNegociacionesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<NegociacionDto>> Handle(GetNegociacionesQuery request, CancellationToken cancellationToken)
    {
        var negociaciones = await _unitOfWork.Repository<Negociacion>().GetAllAsync(cancellationToken);
        var personas = await _unitOfWork.Repository<Persona>().GetAllAsync(cancellationToken);
        var variedadesProducto = await _unitOfWork.Repository<VariedadProducto>().GetAllAsync(cancellationToken);
        var productos = await _unitOfWork.Repository<Producto>().GetAllAsync(cancellationToken);
        var usuarios = await _unitOfWork.Repository<Usuario>().GetAllAsync(cancellationToken);

        // Aplicar filtros
        if (request.IdComisionista.HasValue)
        {
            negociaciones = negociaciones.Where(n => n.IdComisionista == request.IdComisionista.Value).ToList();
        }

        // ? Filtrar por rango de fechas si se proporcionan
        if (request.FechaDesde.HasValue)
        {
            // Considerar desde las 00:00:00 del día especificado
            var fechaDesde = request.FechaDesde.Value.Date;
            negociaciones = negociaciones.Where(n => n.FRegistro >= fechaDesde).ToList();
        }

        if (request.FechaHasta.HasValue)
        {
            // Considerar hasta las 23:59:59 del día especificado
            var fechaHasta = request.FechaHasta.Value.Date.AddDays(1).AddTicks(-1);
            negociaciones = negociaciones.Where(n => n.FRegistro <= fechaHasta).ToList();
        }

        // Cargar relaciones
        foreach (var negociacion in negociaciones)
        {
            // Buscar proveedor por documento si existe
            if (!string.IsNullOrEmpty(negociacion.NroDocumentoProveedor))
            {
                negociacion.Proveedor = personas.FirstOrDefault(p => p.NumeroDocumento == negociacion.NroDocumentoProveedor);
            }

            negociacion.Comisionista = usuarios.FirstOrDefault(u => u.IdUsuario == negociacion.IdComisionista) ?? new Usuario();

            if (negociacion.IdVariedadProducto.HasValue)
            {
                negociacion.VariedadProducto = variedadesProducto.FirstOrDefault(v => v.IdVariedadProducto == negociacion.IdVariedadProducto.Value);

                // ? CARGAR EL PRODUCTO DENTRO DE VARIEDAD PRODUCTO
                if (negociacion.VariedadProducto != null && negociacion.VariedadProducto.IdProducto > 0)
                {
                    negociacion.VariedadProducto.Producto = productos.FirstOrDefault(p => p.IdProducto == negociacion.VariedadProducto.IdProducto);
                }
            }

            if (negociacion.IdTipoDocumento.HasValue)
            {
                var tiposDocumento = await _unitOfWork.Repository<TipoDocumento>().GetAllAsync(cancellationToken);
                negociacion.TipoDocumento = tiposDocumento.FirstOrDefault(t => t.IdTipoDocumento == negociacion.IdTipoDocumento.Value);
            }

            if (negociacion.IdBanco.HasValue)
            {
                var bancos = await _unitOfWork.Repository<Banco>().GetAllAsync(cancellationToken);
                negociacion.Banco = bancos.FirstOrDefault(b => b.IdBanco == negociacion.IdBanco.Value);
            }

            if (negociacion.AprobadaPorIngeniero.HasValue)
            {
                negociacion.AprobadaPorUsuarioIngeniero = usuarios.FirstOrDefault(u => u.IdUsuario == negociacion.AprobadaPorIngeniero.Value);
            }

            if (negociacion.AprobadaPorContadora.HasValue)
            {
                negociacion.AprobadaPorUsuarioContadora = usuarios.FirstOrDefault(u => u.IdUsuario == negociacion.AprobadaPorContadora.Value);
            }

            if (negociacion.RechazadoPorIngeniero.HasValue)
            {
                negociacion.RechazadoPorUsuarioIngeniero = usuarios.FirstOrDefault(u => u.IdUsuario == negociacion.RechazadoPorIngeniero.Value);
            }

            if (negociacion.RechazadoPorContadora.HasValue)
            {
                negociacion.RechazadoPorUsuarioContadora = usuarios.FirstOrDefault(u => u.IdUsuario == negociacion.RechazadoPorContadora.Value);
            }
        }

        return negociaciones.Select(n => _mapper.Map<NegociacionDto>(n)).ToList();
    }
}
