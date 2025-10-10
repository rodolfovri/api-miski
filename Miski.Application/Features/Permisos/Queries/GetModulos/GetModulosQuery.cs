using MediatR;
using Miski.Shared.DTOs.Permisos;

namespace Miski.Application.Features.Permisos.Queries.GetModulos;

public record GetModulosQuery(string? TipoPlataforma = null) : IRequest<List<ModuloDto>>;