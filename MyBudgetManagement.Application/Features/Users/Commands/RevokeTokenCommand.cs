using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Users.Commands;

public class RevokeTokenCommand : IRequest<ApiResponse<string>>
{
    public Guid UserId { get; set; }
    internal class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, ApiResponse<string>>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RevokeTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<ApiResponse<string>> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            await _refreshTokenRepository.RevokeToken(request.UserId);
            return new ApiResponse<string>("Revoke refreshtoken successfully");
        }
    }
}