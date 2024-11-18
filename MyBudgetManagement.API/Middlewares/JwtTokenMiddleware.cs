using MyBudgetManagement.Domain.Interfaces;

namespace MyBudgetManagement.API;

public class JwtTokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IJwtProvider _jwtProvider;

    public JwtTokenMiddleware(RequestDelegate next, IJwtProvider jwtProvider)
    {
        _next = next;
        _jwtProvider = jwtProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // var token = await _jwtProvider.GenerateToken(context.Request.Headers);
        // if (!string.IsNullOrEmpty(token))
        // {
        //     context.Request.Headers["Authorization"] = $"Bearer {token}";
        // }

        await _next(context);
    }
}