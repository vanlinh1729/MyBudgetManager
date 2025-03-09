using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Enums;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Categories.Commands;

public class UpdateCategoryCommand : IRequest<ApiResponse<string>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Budget { get; set; }
    public CategoryType CategoryType { get; set; }
    public Guid UserBalanceId { get; set; }

}

internal class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ApiResponse<string>>
{
    private readonly ICategoryRepositoryAsync _categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepositoryAsync categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    public async Task<ApiResponse<string>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id);
        if (category == null)
        {
            return new ApiResponse<string>("Category not found.");
        }

        category.CategoryType = request.CategoryType;
        category.Budget = request.Budget;
        category.Name = request.Name;

        await _categoryRepository.UpdateAsync(category);
        return new ApiResponse<string>("Category updated successfully.");
    }
    
}