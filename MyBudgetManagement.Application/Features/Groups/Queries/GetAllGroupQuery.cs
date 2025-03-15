using System.Net;
using MediatR;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Groups.Queries;

public class GetAllGroupQuery : IRequest<ApiResponse<IEnumerable<Group>>>
{
    
}

internal class GetAllGroupQueryHandler : IRequestHandler<GetAllGroupQuery, ApiResponse<IEnumerable<Group>>>
{
    private readonly IGroupRepositoryAsync _groupRepository;

    public GetAllGroupQueryHandler(IGroupRepositoryAsync groupRepository)
    {
        _groupRepository = groupRepository;
    }
    public async Task<ApiResponse<IEnumerable<Group>>> Handle(GetAllGroupQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var groups = await _groupRepository.GetAllAsync();

            if (groups == null)
            {
                throw new ApiException("groups not found.");

            }

            return new ApiResponse<IEnumerable<Group>>(groups, "Data fetched successfully.");
        }
        catch (Exception ex)
        {
            // Log exception nếu cần
            return new ApiResponse<IEnumerable<Group>>(null,
                (int)HttpStatusCode.InternalServerError + $"An unexpected error occurred: {ex.Message}");
        }
    }
}