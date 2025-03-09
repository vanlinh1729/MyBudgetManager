using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.Application.Features.UserBalances.Commands;
using MyBudgetManagement.Application.Features.UserBalances.Queries;
using MyBudgetManagement.Application.Wrappers;
using System;
using System.Threading.Tasks;

namespace MyBudgetManagement.API.Controllers
{
    [ApiController]
    [Route("api/user-balances")]
    public class UserBalanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserBalanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Lấy thông tin số dư của người dùng
        [HttpGet()]
        public async Task<IActionResult> GetAllUserBalances()
        {
            var query = new GetAllUserBalanceQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
  // Lấy thông tin số dư của người dùng
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserBalance(Guid userId)
        {
            var query = new GetUserBalanceByUserIdQuery() { UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // Tạo mới số dư cho người dùng
        [HttpPost]
        public async Task<IActionResult> CreateUserBalance([FromBody] CreateUserBalanceCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}