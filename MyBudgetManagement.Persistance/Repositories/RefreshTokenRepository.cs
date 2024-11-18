using Microsoft.EntityFrameworkCore;
using MyBudgetManagement.Application.Wrappers;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Persistance.Context;

namespace MyBudgetManagement.Persistance.Repositories;


public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtProvider _jwtProvider;

    public RefreshTokenRepository(ApplicationDbContext context, IJwtProvider jwtProvider)
    {
        _context = context;
        _jwtProvider = jwtProvider;
    }

    public async Task<RefreshToken> GetByToken(string token) =>
        await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);

    public async Task SaveToken(RefreshToken token)
    {
        _context.RefreshTokens.Add(token);
        await _context.SaveChangesAsync();
    }

    public async Task RevokeToken(Guid userId)
    {
        var oldRefreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.RevokedAt == null && rt.ExpiryDate > DateTime.UtcNow);

        if (oldRefreshToken != null)
        {
                oldRefreshToken.RevokedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
        }
    }

    public async Task<string> RevokeAndGenerateNewRefreshTokenAsync(Guid userId)
    {
        await RevokeToken(userId);
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = Guid.NewGuid().ToString(),
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        };

        await SaveToken(refreshToken);
        return refreshToken.Token;
    }

    public async Task<string> RefreshAccessToken(string refreshTokenStr)
    {
        var refreshToken = await GetByToken(refreshTokenStr);
        if (refreshToken == null || !refreshToken.IsActive)
            throw new UnauthorizedAccessException("Invalid refresh token");

        if (refreshToken.IsExpired)
            throw new UnauthorizedAccessException("Expired refresh token");
        
        // Logic to create a new access token here
        var user = await _context.Users.Include(u=>u.Role).Include(u=>u.UserBalance).FirstOrDefaultAsync(u => u.Id == refreshToken.UserId);
        var newAccessToken = _jwtProvider.GenerateToken(user); // Placeholder

        return newAccessToken;
    }
    public async Task<string> RefreshToken(string refreshTokenStr)
    {
        var token = await GetByToken(refreshTokenStr);
        if (token == null || !token.IsActive)
            throw new UnauthorizedAccessException("Invalid refresh token");

        if (token.IsExpired)
            throw new UnauthorizedAccessException("Expired refresh token");

        await RevokeToken(token.UserId);
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = token.UserId,
            Token = Guid.NewGuid().ToString(),
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        };

        await SaveToken(refreshToken);
        return refreshToken.Token;
    }
}