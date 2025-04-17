using MediatR;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Auths.Commands;

public class RevokeTokenCommand : IRequest<ApiResponse<string>>
{
    public Guid UserId { get; set; }
    internal class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, ApiResponse<string>>
    {
        private readonly ITokenRepositoryAsync _tokenRepositoryAsync;

        public RevokeTokenCommandHandler(ITokenRepositoryAsync tokenRepositoryAsync)
        {
            _tokenRepositoryAsync = tokenRepositoryAsync;
        }

        public async Task<ApiResponse<string>> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            await _tokenRepositoryAsync.RevokeToken(request.UserId);
            return new ApiResponse<string>("Revoke refreshtoken successfully");
        }
    }
}