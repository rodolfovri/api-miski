using MediatR;
using AutoMapper;
using Miski.Domain.Contracts;
using Miski.Domain.Entities;
using Miski.Shared.DTOs.Personas;

namespace Miski.Application.Features.Usuarios.Queries.GetPersonasSinUsuario;

public class GetPersonasSinUsuarioHandler : IRequestHandler<GetPersonasSinUsuarioQuery, List<PersonaDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPersonasSinUsuarioHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<PersonaDto>> Handle(GetPersonasSinUsuarioQuery request, CancellationToken cancellationToken)
    {
        // Obtener todas las personas
        var personas = await _unitOfWork.Repository<Persona>()
            .GetAllAsync(cancellationToken);

        // Obtener todos los usuarios para identificar qué personas ya tienen usuario
        var usuarios = await _unitOfWork.Repository<Usuario>()
            .GetAllAsync(cancellationToken);

        // Obtener IDs de personas que ya tienen usuario
        var personasConUsuario = usuarios
            .Where(u => u.IdPersona.HasValue)
            .Select(u => u.IdPersona.Value)
            .ToHashSet();

        // Si se proporciona IdUsuario, obtener el IdPersona de ese usuario para incluirlo
        int? idPersonaDelUsuario = null;
        if (request.IdUsuario.HasValue)
        {
            var usuarioEspecifico = usuarios.FirstOrDefault(u => u.IdUsuario == request.IdUsuario.Value);
            if (usuarioEspecifico?.IdPersona.HasValue == true)
            {
                idPersonaDelUsuario = usuarioEspecifico.IdPersona.Value;
            }
        }

        // Filtrar personas que NO tienen usuario O que pertenecen al usuario especificado
        var personasSinUsuario = personas
            .Where(p => !personasConUsuario.Contains(p.IdPersona) || 
                        (idPersonaDelUsuario.HasValue && p.IdPersona == idPersonaDelUsuario.Value))
            .ToList();

        // Aplicar filtro de estado si se proporciona
        if (!string.IsNullOrEmpty(request.Estado))
        {
            personasSinUsuario = personasSinUsuario
                .Where(p => p.Estado == request.Estado)
                .ToList();
        }

        // Obtener todos los tipos de documento para mapear
        var tiposDocumento = await _unitOfWork.Repository<TipoDocumento>()
            .GetAllAsync(cancellationToken);

        // Mapear a DTOs
        var resultado = personasSinUsuario.Select(p =>
        {
            var tipoDoc = tiposDocumento.FirstOrDefault(td => td.IdTipoDocumento == p.IdTipoDocumento);
            
            return new PersonaDto
            {
                IdPersona = p.IdPersona,
                IdTipoDocumento = p.IdTipoDocumento,
                TipoDocumentoNombre = tipoDoc?.Nombre ?? string.Empty,
                NumeroDocumento = p.NumeroDocumento,
                Nombres = p.Nombres,
                Apellidos = p.Apellidos,
                NombreCompleto = $"{p.Nombres} {p.Apellidos}",
                Telefono = p.Telefono,
                Email = p.Email,
                Direccion = p.Direccion,
                Estado = p.Estado ?? "ACTIVO",
                FRegistro = p.FRegistro
            };
        }).ToList();

        return resultado;
    }
}
