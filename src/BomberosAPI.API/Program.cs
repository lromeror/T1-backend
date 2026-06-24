using BomberosAPI.API.Common.Extensions;
using BomberosAPI.Application.Features.Auth;
using BomberosAPI.Application.Features.HealthPersonnel;
using BomberosAPI.Application.Features.TraineeFirefighters;
using BomberosAPI.Application.Features.TrainingSessions;
using BomberosAPI.Infrastructure;
using BomberosAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Infrastructure (BCrypt, JWT, repositories)
builder.Services.AddInfrastructure(builder.Configuration);

// Application services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TrainingSessionService>();
builder.Services.AddScoped<TraineeFirefighterService>();
builder.Services.AddScoped<HealthPersonnelService>();

// Controllers + FluentValidation
builder.Services.AddControllers();
builder.Services.AddValidation();

// JWT Authentication + Authorization policies
builder.Services.AddJwtAuthentication(builder.Configuration);

// ICurrentUserService — lee claims del JWT en cada request
builder.Services.AddCurrentUser();

// CORS — permite peticiones desde Expo Web y cualquier localhost en desarrollo
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(
                "http://localhost:8081",
                "http://localhost:19006",
                "http://localhost:3000",
                "http://100.89.25.34:8081")
              .AllowAnyHeader()
              .AllowAnyMethod()));

// OpenAPI / Scalar / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    await DbSeeder.SeedAsync(app.Services);

app.UseGlobalExceptionMiddleware();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BomberosAPI v1"));

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
