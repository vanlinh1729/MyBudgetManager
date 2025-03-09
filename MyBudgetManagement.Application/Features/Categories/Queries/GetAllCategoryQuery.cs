using System.Net;
using MediatR;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Categories.Queries;

public class GetAllCategoryQuery: IRequest<ApiResponse<IEnumerable<Category>>>
{
}

internal class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, ApiResponse<IEnumerable<Category>>>
{
    private readonly ICategoryRepositoryAsync _categoryRepository;

    public GetAllCategoryQueryHandler(ICategoryRepositoryAsync categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ApiResponse<IEnumerable<Category>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _categoryRepository.GetAllAsync();

            if (category == null)
            {
                throw new ApiException("Category not found.");

            }

            return new ApiResponse<IEnumerable<Category>>(category, "Data fetched successfully.");
        }
        catch (Exception ex)
        {
            // Log exception nếu cần
            return new ApiResponse<IEnumerable<Category>>(null,
                (int)HttpStatusCode.InternalServerError + $"An unexpected error occurred: {ex.Message}");
        }
    }
    
}