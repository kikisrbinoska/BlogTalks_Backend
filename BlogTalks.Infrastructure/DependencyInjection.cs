using BlogTalks.Application.Abstractions;
using BlogTalks.Domain.Reposotories;
using BlogTalks.Infrastructure.Atuhenticatin;
using BlogTalks.Infrastructure.Data.DataContext;
using BlogTalks.Infrastructure.Messaging;
using BlogTalks.Infrastructure.Repositories;
using BlogTalks.Infrastructure.Reposotories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace BlogTalks.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
                npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Default)));

        services.AddTransient<IBlogPostRepository, BlogPostRepository>();
        services.AddTransient<ICommentRepository, CommentRepository>();
        services.AddTransient<IUserRepository, UserRepository>();

        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

        services.AddTransient<MessagingServiceHttp>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            var client = factory.CreateClient("EmailSenderApi"); 
            return new MessagingServiceHttp(client);
        });

        services.AddKeyedTransient<IMessagingService, MessagingServiceHttp>("MessagingServiceHttp",
            (sp, _) => sp.GetRequiredService<MessagingServiceHttp>());

            
        services.AddKeyedTransient<IMessagingService, MessagingServiceRabbitMQ>("MessagingServiceRabbitMQ");

        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
   
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        if (string.IsNullOrEmpty(authHeader)) return Task.CompletedTask;
                        context.Token = authHeader.StartsWith("Bearer ") ? authHeader["Bearer ".Length..].Trim() : authHeader;
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();

        return services;
    }
}