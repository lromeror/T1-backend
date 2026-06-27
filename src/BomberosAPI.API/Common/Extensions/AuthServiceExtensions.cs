using System.Text;
using System.Text.Json;
using BomberosAPI.API.Common.Responses;
using BomberosAPI.Application.Common.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BomberosAPI.API.Common.Extensions;

public static class AuthServiceExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var secretKey = configuration["JwtSettings:SecretKey"]
            ?? throw new InvalidOperationException("JwtSettings:SecretKey is not configured.");

        // Without this, .NET remaps "sub" → ClaimTypes.NameIdentifier, breaking FindFirstValue(JwtRegisteredClaimNames.Sub)
        System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services
            .AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer           = true,
                    ValidIssuer              = configuration["JwtSettings:Issuer"],
                    ValidateAudience         = true,
                    ValidAudience            = configuration["JwtSettings:Audience"],
                    ValidateLifetime         = true,
                    ClockSkew                = TimeSpan.Zero
                };

                // Devuelve ApiResponse consistente cuando el token falta o es invalido (401)
                o.Events = new JwtBearerEvents
                {
                    OnChallenge = async ctx =>
                    {
                        ctx.HandleResponse();
                        ctx.Response.StatusCode  = 401;
                        ctx.Response.ContentType = "application/json";
                        var body = ApiResponse.Fail(401, "Authentication is required. Provide a valid Bearer token.");
                        await ctx.Response.WriteAsync(JsonSerializer.Serialize(body, JsonOptions));
                    },
                    // Devuelve ApiResponse consistente cuando el rol no es suficiente (403)
                    OnForbidden = async ctx =>
                    {
                        ctx.Response.StatusCode  = 403;
                        ctx.Response.ContentType = "application/json";
                        var body = ApiResponse.Fail(403, "You do not have permission to perform this action.");
                        await ctx.Response.WriteAsync(JsonSerializer.Serialize(body, JsonOptions));
                    }
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("AdminOnly",          p => p.RequireRole(Roles.Admin, Roles.SystemAdmin))
            .AddPolicy("HealthPersonnel",    p => p.RequireRole(Roles.Medical, Roles.Admin, Roles.SystemAdmin))
            .AddPolicy("InstructorOrAbove",  p => p.RequireRole(Roles.Capacitator, Roles.Admin, Roles.SystemAdmin))
            .AddPolicy("Authenticated",      p => p.RequireAuthenticatedUser());

        return services;
    }
}
