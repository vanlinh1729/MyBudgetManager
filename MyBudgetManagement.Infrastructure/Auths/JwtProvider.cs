using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace MyBudgetManagement.Infrastructure.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly IConfiguration _configuration;

    public JwtProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user, int expiryMinutes = 15)
    {
        var claims =  new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("UserId", user.Id.ToString())

        };
        claims.AddRange(user.UserRoles.Select(ur => new Claim(ClaimTypes.Role, ur.Role.Name)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(expiryMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
    
        try
        {
            // Các tham số validation cho JWT
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero // Không chấp nhận chênh lệch thời gian
            };

            // Validate token và trả về ClaimsPrincipal
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            // Kiểm tra xem token có phải là loại JwtSecurityToken không
            if (validatedToken is not JwtSecurityToken jwtToken)
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        catch (SecurityTokenException ex)
        {
            // Xử lý lỗi nếu token không hợp lệ
            throw new UnauthorizedAccessException("Invalid token", ex);
        }
        catch (Exception ex)
        {
            // Xử lý các lỗi khác (ví dụ: lỗi khi giải mã)
            throw new UnauthorizedAccessException("An error occurred while validating the token", ex);
        }    }
}