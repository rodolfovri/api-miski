using MediatR;
using Miski.Shared.DTOs.FAQ;

namespace Miski.Application.Features.FAQ.CategoriaFAQ.Queries.GetCategoriaById;

public record GetCategoriaByIdQuery(int Id) : IRequest<CategoriaFAQDto>;
