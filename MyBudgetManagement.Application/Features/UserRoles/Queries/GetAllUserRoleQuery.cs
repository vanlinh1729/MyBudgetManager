using MediatR;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Features.Users.Queries;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.UserRoles.Queries;

public class GetAllUserRoleQuery : IRequest<ApiResponse<IEnumerable<UserRole>>>
{
    internal class GetAllUserQueryHandler : IRequestHandler<GetAllUserRoleQuery, ApiResponse<IEnumerable<UserRole>>>
    {
        private readonly IUserRoleRepositoryAsync _userRoleRepository;

        public GetAllUserQueryHandler(IUserRoleRepositoryAsync userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<ApiResponse<IEnumerable<UserRole>>> Handle(GetAllUserRoleQuery request,
            CancellationToken cancellationToken)
        {
            var listUserRoles = await _userRoleRepository.GetAllAsync();
            if (listUserRoles == null)
            {
                throw new ApiException("UserRole not found.");
            }

            return new ApiResponse<IEnumerable<UserRole>>(listUserRoles, "Data Fetched successfully");

        }
    }
}