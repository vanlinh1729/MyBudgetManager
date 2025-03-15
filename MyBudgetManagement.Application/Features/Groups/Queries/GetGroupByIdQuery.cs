using System.Net;
using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Groups.Queries;

public class GetGroupByIdQuery : IRequest<ApiResponse<Group>>
{
    public Guid Id { get; set; }   
    
}

internal class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, ApiResponse<Group>>
{
    private readonly IGroupRepositoryAsync _groupRepository;

    public GetGroupByIdQueryHandler(IGroupRepositoryAsync groupRepository)
    {
        _groupRepository = groupRepository;
    }
    public async Task<ApiResponse<Group>> Handle(GetGroupByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var group = await _groupRepository.GetByIdAsync(request.Id);

            if (group == null)
                return new ApiResponse<Group>(null,
                    (int)HttpStatusCode.NotFound + "Group not found.");

            return new ApiResponse<Group>(group, "Data fetched successfully.");
        }
        catch (Exception ex)
        {
            // Log exception nếu cần
            return new ApiResponse<Group>(null,
                (int)HttpStatusCode.InternalServerError + $"An unexpected error occurred: {ex.Message}");
        }
    }
}