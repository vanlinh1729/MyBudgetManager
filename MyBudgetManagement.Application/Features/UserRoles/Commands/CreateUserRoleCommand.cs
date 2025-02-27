using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.UserRoles.Commands;

public class CreateUserRoleCommand : IRequest<ApiResponse<UserRole>>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    internal class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRoleCommand, ApiResponse<UserRole>>
    {
        private readonly IUserRoleRepositoryAsync _userRoleRepository;

        public CreateUserRoleCommandHandler(IUserRoleRepositoryAsync userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }
        public async Task<ApiResponse<UserRole>> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
        {
           var userRole = await _userRoleRepository.AddAsync(new UserRole()
            {
                UserId = request.UserId,
                RoleId = request.RoleId
            });
            return new ApiResponse<UserRole>(userRole);
        }
    }
}