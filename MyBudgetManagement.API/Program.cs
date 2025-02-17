using System.Text;
using Microsoft.OpenApi.Models;
using MyBudgetManagement.Application;
using MyBudgetManagement.Infrastructure;
using MyBudgetManagement.Persistance;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.File;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Sink(new FileSink("Logs/logs.txt", new JsonFormatter(), long.MaxValue, Encoding.Default))
    .CreateLogger();

try
{
    Log.Information("Starting web host.");
    var builder = WebApplication.CreateBuilder(args);

    // Register services in the container
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "MyBudgetManagement API",
            Version = "v1",
            Description = "API for managing budgets"
        });

        // Swagger Authorization configuration
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    });

    // Application, Infrastructure, and Persistence registration
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddPersistance(builder.Configuration);

    builder.Host.UseSerilog();

    var app = builder.Build();

   

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseHttpsRedirection();
    app.MapControllers();
    
    // Seed data asynchronously
    await SeedDataAsync(app);
    await app.RunAsync();
    return 1;
}
catch (Exception ex)
{
    if (ex is HostAbortedException) throw;

    Log.Fatal(ex, "Host terminated unexpectedly!");
    return 0;
}
finally
{
    Log.CloseAndFlush();
}

async Task SeedDataAsync(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var seeder = services.GetRequiredService<IDataSeeder>();
            await seeder.SeedAsync();
            Log.Information("Data seeded successfully.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while seeding the database.");
        }
    }
}
