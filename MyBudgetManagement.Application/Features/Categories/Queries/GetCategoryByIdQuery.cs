using System.Collections;
using System.Net;
using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Categories.Queries;

public class GetCategoryByIdQuery : IRequest<ApiResponse<Category>>
{
    public Guid Id { get; set; }
}

internal class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, ApiResponse<Category>>
{
    private readonly ICategoryRepositoryAsync _categoryRepository;

    public GetCategoryByIdQueryHandler(ICategoryRepositoryAsync categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    public async Task<ApiResponse<Category>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
                
            if (category == null)
            {
                return new ApiResponse<Category>(null, (int)HttpStatusCode.NotFound +$"Category: {request.Id} not found.");
            }

            return new ApiResponse<Category>(category, "Data fetched successfully.");
        }
        catch (Exception ex)
        {
            // Log exception nếu cần
            return new ApiResponse<Category>(null, (int)HttpStatusCode.InternalServerError + $"An unexpected error occurred: {ex.Message}");
        }   
    }
}