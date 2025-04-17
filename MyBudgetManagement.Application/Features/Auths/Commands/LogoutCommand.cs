using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Auths.Commands;

public class LogoutCommand : IRequest<ApiResponse<string>>
{
    public Guid UserId { get; set; }
    
    internal class LogoutCommandHandler : IRequestHandler<LogoutCommand, ApiResponse<string>>
    {
        private readonly ITokenRepositoryAsync _tokenRepositoryAsync;

        public LogoutCommandHandler(ITokenRepositoryAsync tokenRepositoryAsync)
        {
            _tokenRepositoryAsync = tokenRepositoryAsync ?? throw new ArgumentNullException(nameof(tokenRepositoryAsync));
        }

        public async Task<ApiResponse<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
            {
                return new ApiResponse<string>("Invalid user ID.");
            }

            try
            {
                var revoked = await _tokenRepositoryAsync.RevokeToken(request.UserId, cancellationToken);

                if (!revoked)
                {
                    return new ApiResponse<string>("User has no active session.");
                }

                return new ApiResponse<string>("Logout successful.");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>($"Logout failed: {ex.Message}");
            }
        }
    }
}