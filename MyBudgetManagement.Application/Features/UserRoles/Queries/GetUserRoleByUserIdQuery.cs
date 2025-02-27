using System.Net;
using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.UserRoles.Queries;

public class GetUserRoleByUserIdQuery: IRequest<ApiResponse<List<UserRole>>>
{
    public Guid UserId { get; set; }
    internal class GetUserRoleByUserIdQueryHandler : IRequestHandler<GetUserRoleByUserIdQuery, ApiResponse<List<UserRole>>>
    {
        private readonly IUserRoleRepositoryAsync _userRoleRepository;

        public GetUserRoleByUserIdQueryHandler(IUserRoleRepositoryAsync userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<ApiResponse<List<UserRole>>> Handle(GetUserRoleByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var listUserRoles = await _userRoleRepository.GetUserRolesByUserIdAsync(request.UserId);
                
                if (listUserRoles == null)
                {
                    return new ApiResponse<List<UserRole>>(null, (int)HttpStatusCode.NotFound +"UserRole not found.");
                }

                return new ApiResponse<List<UserRole>>(listUserRoles, "Data fetched successfully.");
            }
            catch (Exception ex)
            {
                // Log exception nếu cần
                return new ApiResponse<List<UserRole>>(null, (int)HttpStatusCode.InternalServerError + $"An unexpected error occurred: {ex.Message}");
            }
            
        }
    }
    
}