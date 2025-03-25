using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBudgetManagement.Application.Features.Categories.Commands;
using MyBudgetManagement.Application.Features.Categories.Queries;

namespace MyBudgetManagement.API.Controllers;

[Route("api/categories")]
[ApiController]
public class CategoryController : ControllerBase
{
    // GET
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet()]
    public async Task<IActionResult> GetAllCategories()
    {
        var query = new GetAllCategoryQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        var query = new GetCategoryByIdQuery();
        query.Id = id;
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    [HttpGet("{userid}")]
    public async Task<IActionResult> GetAllCategoriesByUserId(Guid userid)
    {
        var query = new GetAllCategoryByUserIdQuery();
        query.UserId = userid;
        var result = await _mediator.Send(query);
        return Ok(result);
    }
     [HttpPost()]
    public async Task<IActionResult> CreateCategory(CreateCategoryCommand createCategoryCommand, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(createCategoryCommand);
        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id, [FromBody] DeleteCategoryCommand command)
    {
        command.CategoryId = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    [Authorize(Roles = "User")]
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentUserCategories()
    {
        var userId = Guid.Parse(User.FindFirstValue("UserId")); // Lấy UserId từ token
        var query = new GetAllCategoryByUserIdQuery { UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
