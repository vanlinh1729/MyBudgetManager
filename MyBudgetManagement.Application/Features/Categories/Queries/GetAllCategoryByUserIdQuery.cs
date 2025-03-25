using System.Collections;
using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.DTOs;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Categories.Queries;

public class GetAllCategoryByUserIdQuery: IRequest<ApiResponse<IEnumerable<CategoryWithTotalDto>>>
{
    public Guid UserId { get; set; }
}

internal class GetAllCategoryByUserIdQueryHandler : IRequestHandler<GetAllCategoryByUserIdQuery, ApiResponse<IEnumerable<CategoryWithTotalDto>>>
{
    private readonly ICategoryRepositoryAsync _categoryRepository;
    private readonly ITransactionRepositoryAsync _transactionRepository;

    public GetAllCategoryByUserIdQueryHandler(
        ICategoryRepositoryAsync categoryRepository,
        ITransactionRepositoryAsync transactionRepository)
    {
        _categoryRepository = categoryRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<ApiResponse<IEnumerable<CategoryWithTotalDto>>> Handle(GetAllCategoryByUserIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var categories = await _categoryRepository.GetCategoriesByUserId(request.UserId);

            if (categories == null || !categories.Any())
            {
                return new ApiResponse<IEnumerable<CategoryWithTotalDto>>(null,
                    (int)HttpStatusCode.NotFound + $"Categories of user {request.UserId} not found.");
            }

            var result = new List<CategoryWithTotalDto>();

            foreach (var category in categories)
            {
                var transactions = await _transactionRepository.GetTransactionsByCategoryId(category.Id);
                decimal totalAmount = transactions?.Sum(t => t.Amount) ?? 0;

                result.Add(new CategoryWithTotalDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Budget = category.Budget,
                    TotalAmount = totalAmount,
                    RemainingBudget = category.Budget - totalAmount,
                    CategoryType = category.CategoryType,
                    Created = category.Created,
                    CreatedBy = category.CreatedBy,
                    LastModified = category.LastModified,
                    LastModifiedBy = category.LastModifiedBy
                });
            }

            return new ApiResponse<IEnumerable<CategoryWithTotalDto>>(result, "Data fetched successfully.");
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<CategoryWithTotalDto>>(null,
                (int)HttpStatusCode.InternalServerError + $"An unexpected error occurred: {ex.Message}");
        }
    }
}