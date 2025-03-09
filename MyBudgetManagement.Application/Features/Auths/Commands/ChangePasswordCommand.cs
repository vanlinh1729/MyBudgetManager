using MediatR;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Helpers;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Auths.Commands;

public class ChangePasswordCommand : IRequest<ApiResponse<bool>>
{
    public Guid UserId { get; set; }
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;

    internal class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ApiResponse<bool>>
    {
        private readonly IUserRepositoryAsync _userRepository;

        public ChangePasswordCommandHandler(IUserRepositoryAsync userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId)
                       ?? throw new ApiException("User not found.");

            if (string.IsNullOrWhiteSpace(request.NewPassword))
                throw new ApiException("New password cannot be empty.");

            if (!BCryptHelper.VerifyPassword(request.OldPassword, user.PasswordHash))
                throw new ApiException("Old password is incorrect.");

            user.PasswordHash = BCryptHelper.HashPassword(request.NewPassword);
            await _userRepository.UpdateAsync(user);

            return new ApiResponse<bool>(true, "Password changed successfully.");
        }
    }
}