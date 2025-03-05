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
    [HttpPut()]
    public async Task<IActionResult> UpdateAccountProfile(UpdateAccountProfileCommand updateAccountProfile, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(updateAccountProfile, cancellationToken);
        return Ok(result);
    }
}