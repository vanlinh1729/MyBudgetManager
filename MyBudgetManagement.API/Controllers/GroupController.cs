using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.Application.Features.Groups.Queries;

namespace MyBudgetManagement.API.Controllers;

[ApiController]
[Route("api/groups")]
public class GroupController : ControllerBase
{
   private readonly IMediator _mediator;

   public GroupController(IMediator mediator)
   {
      _mediator = mediator;
   }

   [HttpGet()]
   public async Task<IActionResult> GetAllGroup()
   {
      var result = await _mediator.Send(new GetAllGroupQuery());
      return Ok(result);
   }
   
   [HttpGet("{id}")]
   public async Task<IActionResult> GetGroupById(Guid Id)
   {
      var result = await _mediator.Send(new GetGroupByIdQuery()
      {
         Id = Id
      });
      return Ok(result);
   }
}