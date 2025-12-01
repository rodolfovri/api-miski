using MediatR;
using Miski.Shared.DTOs.FAQ;

namespace Miski.Application.Features.FAQ.FAQs.Queries.GetFAQs;

public record GetFAQsQuery(
    int? IdCategoriaFAQ = null,
    string? Estado = null
) : IRequest<List<FAQDto>>;
