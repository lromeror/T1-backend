using BomberosAPI.Application.Common.Interfaces;
using BomberosAPI.Application.Features.Auth;
using BomberosAPI.Application.Features.Users;
using BomberosAPI.Domain.Repositories;
using BomberosAPI.Infrastructure.Repositories;
using BomberosAPI.Infrastructure.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BomberosAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IAuthRepository, AuthRepository>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<UserService>();
        services.AddScoped<IValidator<CreateUserRequest>, CreateUserValidator>();

        return services;
    }
}
