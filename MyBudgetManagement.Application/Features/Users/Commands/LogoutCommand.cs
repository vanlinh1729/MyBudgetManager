using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Users.Commands;

public class LogoutCommand : IRequest<ApiResponse<string>>
{
    public Guid UserId { get; set; }
    internal class LogoutCommandHandler : IRequestHandler<LogoutCommand, ApiResponse<string>>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<ApiResponse<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Revoke the refresh token for the user
                await _refreshTokenRepository.RevokeToken(request.UserId);

                return new ApiResponse<string>("Logout successful.");
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return new ApiResponse<string>($"Logout failed: {ex.Message}");
            }
            
        }
    }
}