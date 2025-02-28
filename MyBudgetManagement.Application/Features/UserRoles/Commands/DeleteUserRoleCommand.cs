using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.UserRoles.Commands;

public class DeleteUserRoleCommand : IRequest<ApiResponse<string>>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    internal class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRoleCommand, ApiResponse<string>>
    {
        private readonly IUserRoleRepositoryAsync _userRoleRepository;

        public DeleteUserRoleCommandHandler(IUserRoleRepositoryAsync userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }
        public async Task<ApiResponse<string>> Handle(DeleteUserRoleCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                await _userRoleRepository.DeleteUserRoleAsync(request.UserId, request.RoleId);
                return new ApiResponse<string>("UserRole has been successful deleted.");
            }
            catch (Exception e)
            {
                return new ApiResponse<string>("Delete failed, err: " + e.Message);
            }
            
        }
    }
}