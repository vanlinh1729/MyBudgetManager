using System.Collections;
using System.Net;
using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Categories.Queries;

public class GetAllCategoryByUserIdQuery: IRequest<ApiResponse<IEnumerable<Category>>>
{
    public Guid UserId { get; set; }
}

internal class GetAllCategoryByUserIdQueryHandler : IRequestHandler<GetAllCategoryByUserIdQuery, ApiResponse<IEnumerable<Category>>>
{
    private readonly ICategoryRepositoryAsync _categoryRepository;

    public GetAllCategoryByUserIdQueryHandler(ICategoryRepositoryAsync categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ApiResponse<IEnumerable<Category>>> Handle(GetAllCategoryByUserIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _categoryRepository.GetCategoriesByUserId(request.UserId);

            if (category == null)
            {
                return new ApiResponse<IEnumerable<Category>>(null,
                    (int)HttpStatusCode.NotFound + $"Category of user {request.UserId} not found.");
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