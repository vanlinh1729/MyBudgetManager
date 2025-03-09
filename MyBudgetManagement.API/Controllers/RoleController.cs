using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.Application.Features.Roles.Queries;

namespace MyBudgetManagement.API.Controllers;


[Route("/api/roles")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet()]
    public async Task<IActionResult> GetAllRoles()
    {
        var query = new GetAllRoleQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleById(Guid id)
    {
        var query = new GetRoleByIdQuery(){Id = id};
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}