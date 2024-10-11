using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.Application.Features.Users.Queries;

namespace MyBudgetManagement.API.Controllers;

[Route("/api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _mediator.Send(new GetAllUserQuery());
        return Ok(result);
    } 
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery {Id = id} );
        return Ok(result);
    }
}