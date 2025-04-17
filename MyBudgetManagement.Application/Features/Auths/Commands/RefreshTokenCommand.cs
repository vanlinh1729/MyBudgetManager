using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Auths.Commands;

public class RefreshTokenCommand : IRequest<AuthResponse>
{
    public string RefreshToken { get; set; }
    internal class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
    {
        private readonly ITokenRepositoryAsync _tokenRepositoryAsync;

        public RefreshTokenHandler(ITokenRepositoryAsync tokenRepositoryAsync)
        {
            _tokenRepositoryAsync = tokenRepositoryAsync;
        }

        public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var accessToken = await _tokenRepositoryAsync.RefreshAccessToken(request.RefreshToken);
            var refreshToken = await _tokenRepositoryAsync.RefreshToken(request.RefreshToken);

            return new AuthResponse(accessToken, refreshToken);
        }
    }
}