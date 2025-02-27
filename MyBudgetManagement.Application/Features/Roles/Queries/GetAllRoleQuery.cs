using MediatR;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Roles.Queries;

public class GetAllRoleQuery : IRequest<ApiResponse<IEnumerable<Role>>>
{
    internal class GetAllRoleQueryHandler : IRequestHandler<GetAllRoleQuery, ApiResponse<IEnumerable<Role>>>
    {
        private readonly IRoleRepositoryAsync _roleRepository;

        public GetAllRoleQueryHandler(IRoleRepositoryAsync roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<ApiResponse<IEnumerable<Role>>> Handle(GetAllRoleQuery request,
            CancellationToken cancellationToken)
        {
            var listRoles = await _roleRepository.GetAllAsync();
            if (listRoles == null)
            {
                throw new ApiException("Role not found.");
            }

            return new ApiResponse<IEnumerable<Role>>(listRoles, "Data Fetched successfully");

        }
    }
}