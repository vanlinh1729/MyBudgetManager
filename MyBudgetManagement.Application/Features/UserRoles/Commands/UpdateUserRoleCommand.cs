using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.UserRoles.Commands;

public class UpdateUserRoleCommand : IRequest<ApiResponse<string>>
{
    public Guid RoleId { get; set; }

    internal class UpdateUserCommandHandler : IRequestHandler<UpdateUserRoleCommand, ApiResponse<string>>
    {
        public IUserRepositoryAsync _userRoleRepository { get; set; }

        public UpdateUserCommandHandler(IUserRepositoryAsync userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }
        public async Task<ApiResponse<string>> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
           // await _userRoleRepository.UpdateAsync()
        }
    }
    
}