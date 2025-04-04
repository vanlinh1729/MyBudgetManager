using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Interfaces;
using MyBudgetManagement.Infrastructure.Authentication;
using MyBudgetManagement.Infrastructure.FileStorage;

namespace MyBudgetManagement.Infrastructure;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //add services
        services.AddScoped<IJwtProvider, JwtProvider>();  
        services.AddScoped<IDataSeeder, DataSeeder>();
        services.AddScoped<IEmailService, EmailService.EmailService>();
        services.AddScoped<IFileStorageService, CloudinaryService>();
        
        // Validate Cloudinary configuration
        var cloudinarySection = configuration.GetSection("Cloudinary");
        if (!cloudinarySection.Exists())
        {
            throw new Exception("Cloudinary configuration section is missing");
        }
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

        services.AddAuthorization();


    }
}