using Microsoft.AspNetCore.Mvc;
using MediatR;
using System;
using System.Threading.Tasks;
using MyBudgetManagement.Application.Features.Transactions.Commands;
using MyBudgetManagement.Application.Features.Transactions.Queries;

namespace MyBudgetManagement.API.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(Guid id, [FromBody] UpdateTransactionCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(Guid id)
        {
            var result = await _mediator.Send(new DeleteTransactionCommand { Id = id });
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var result = await _mediator.Send(new GetAllTransactionQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(Guid id)
        {
            var result = await _mediator.Send(new GetTransactionByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpGet("by-category/{categoryId}")]
        public async Task<IActionResult> GetTransactionsByCategoryId(Guid categoryId)
        {
            var result = await _mediator.Send(new GetAllTransactionsByCategoryIdQuery() { CategoryId = categoryId });
            return Ok(result);
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetTransactionsByUserId(Guid userId)
        {
            var result = await _mediator.Send(new GetAllTransactionByUserId() { UserId = userId });
            return Ok(result);
        }
    }
}
