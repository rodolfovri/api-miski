using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Miski.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
}