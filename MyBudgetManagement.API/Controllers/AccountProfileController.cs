using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.Application.Features.AccountProfiles.Commands;

namespace MyBudgetManagement.API.Controllers;

[ApiController]
[Route("/api/accountprofiles")]
public class AccountProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccountProfile(CreateAccountProfileCommand createAccountProfile, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(createAccountProfile, cancellationToken);
        return Ok(result);
    }
}