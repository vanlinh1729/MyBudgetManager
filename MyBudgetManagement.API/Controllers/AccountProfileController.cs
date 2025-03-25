using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.Application.Features.AccountProfiles.Commands;
using MyBudgetManagement.Application.Features.AccountProfiles.Queries;

namespace MyBudgetManagement.API.Controllers;

[Authorize(Roles = "User")]
[ApiController]
[Route("/api/accountprofiles")]
public class AccountProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet()]
    public async Task<IActionResult> GetAllAccountProfiles(GetAllAccountProfileQuery getAllAccountProfileQuery, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(getAllAccountProfileQuery, cancellationToken);
        return Ok(result);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAccountProfileById(Guid id)
    {
        var result = await _mediator.Send(new GetAccountProfileByIdQuery() {Id = id} );
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost()]
    public async Task<IActionResult> CreateAccountProfile(CreateAccountProfileCommand createAccountProfile, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(createAccountProfile, cancellationToken);
        return Ok(result);
    }
    [Authorize(Roles = "User")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccountProfile(Guid id, UpdateAccountProfileCommand updateAccountProfile, CancellationToken cancellationToken)
    {
        updateAccountProfile.Id = id;
        var result = await _mediator.Send(updateAccountProfile, cancellationToken);
        return Ok(result);
    }

    [Authorize(Roles = "User")]
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentUserAccountProfile()
    {
        var userId = Guid.Parse(User.FindFirstValue("UserId")); // Lấy UserId từ token
        var query = new GetAccountProfileOfCurrentUserQuery { UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [Authorize(Roles = "User")]
    [HttpPost("avatar")]
    public async Task<IActionResult> UploadAvatar(IFormFile file, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue("UserId"));
        var command = new UploadAvatarCommand
        {
            UserId = userId,
            AvatarFile = file
        };
        
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
