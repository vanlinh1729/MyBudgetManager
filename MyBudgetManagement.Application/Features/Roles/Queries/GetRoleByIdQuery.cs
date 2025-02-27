using System.Net;
using MediatR;
using MyBudgetManagement.Application.Features.AccountProfiles.Queries;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Roles.Queries;

public class GetRoleByIdQuery: IRequest<ApiResponse<Role>?>
{
    public Guid Id { get; set; }

    internal class
        GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, ApiResponse<Role?>>
    {
        private readonly IRoleRepositoryAsync _roleRepository;

        public GetRoleByIdQueryHandler(IRoleRepositoryAsync roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<ApiResponse<Role?>> Handle(GetRoleByIdQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var role =
                    await _roleRepository.GetByIdAsync(request.Id);

                if (role == null)
                    return new ApiResponse<Role>(null,
                        (int)HttpStatusCode.NotFound + "role not found.");

                return new ApiResponse<Role>(role, "Data fetched successfully.");
            }
            catch (Exception ex)
            {
                // Log exception nếu cần
                return new ApiResponse<Role>(null,
                    (int)HttpStatusCode.InternalServerError + $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}