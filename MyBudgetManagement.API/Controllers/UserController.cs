using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.Application.Features.Users.Commands;
using MyBudgetManagement.Application.Features.Users.Queries;

namespace MyBudgetManagement.API.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize()]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery {Id = id} );
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet()]
    public async Task<IActionResult> GetUsers([FromQuery] string? email)
    {
        if (!string.IsNullOrEmpty(email))
        {
            var result = await _mediator.Send(new GetUserByEmailQuery { Email = email });
            return Ok(result);
        }
    
        var resultAll = await _mediator.Send(new GetAllUserQuery());
        return Ok(resultAll);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserCommand command)
    {
        command.UserId = id; 
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
}