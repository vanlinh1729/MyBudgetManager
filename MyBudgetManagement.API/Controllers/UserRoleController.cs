using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.Application.Features.UserRoles.Commands;
using MyBudgetManagement.Application.Features.UserRoles.Queries;
using MyBudgetManagement.Application.Wrappers;
using System;
using System.Threading.Tasks;

namespace MyBudgetManagement.API.Controllers
{
    [ApiController]
    [Route("api/user-roles")]
    public class UserRoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserRoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllUserRoles()
        {
            var query = new GetAllUserRoleQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserRoles(Guid userId)
        {
            var query = new GetUserRoleByUserIdQuery() { UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserRole([FromBody] CreateUserRoleCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{userId}/{roleId}")]
        public async Task<IActionResult> RemoveUserRole(Guid userId, Guid roleId)
        {
            var command = new DeleteUserRoleCommand() { UserId = userId, RoleId = roleId };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}