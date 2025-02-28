using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.UserRoles.Commands;

public class UpdateUserRoleCommand : IRequest<ApiResponse<bool>>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserRoleCommand, ApiResponse<bool>>
    {
        public IUserRoleRepositoryAsync _userRoleRepository { get; set; }

        public UpdateUserCommandHandler(IUserRoleRepositoryAsync userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }
        public async Task<ApiResponse<bool>> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var updatedCheck = await _userRoleRepository.UpdateUserRoleAsync(request.UserId, request.RoleId);
            return new ApiResponse<bool>(updatedCheck);
        }
    }
    
}