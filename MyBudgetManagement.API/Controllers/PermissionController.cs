using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.Application.Features.Permissions.Queries;

namespace MyBudgetManagement.API.Controllers;

[Route("/api/permissions")]
[ApiController]
public class PermissionController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [Authorize]
    [HttpGet()]
    public async Task<IActionResult> GetAllPermissions ()
    {
        var result = await _mediator.Send(new GetAllPermissionsQuery());
        return Ok(result);
    }
}