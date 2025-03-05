using MediatR;
using MyBudgetManagement.Application.Wrappers;

namespace MyBudgetManagement.Application.Features.Categories.Commands;

public class UpdateCategoryCommand : IRequest<ApiResponse<string>>
{
}