using System.Security.Claims;
using System.Text;
using MediatR;
using MyBudgetManagement.Application.Exceptions;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.Application.Features.Auths.Commands;

public class LoginCommand: IRequest<ApiResponse<string>>
{
    public string Email { get; set; }
    public string Password { get; set; }
    
    internal class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<string>>
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly IUserRepositoryAsync _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;


        public LoginCommandHandler(IJwtProvider jwtProvider, IUserRepositoryAsync userRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _jwtProvider = jwtProvider;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<ApiResponse<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.UserLogin(request.Email, request.Password);
        
            if (user == null)
            {
                throw new ApiException("Invalid email or password");
            }

            // Tạo token JWT
            var token = _jwtProvider.GenerateToken(user);
            //tao refresh token, revoke rftken cu
            var refreshToken = await _refreshTokenRepository.RevokeAndGenerateNewRefreshTokenAsync(user.Id);
            
            return new ApiResponse<string>(token, "Login successful");
        }
    }
}

