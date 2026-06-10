using BomberosAPI.API.Common.Extensions;
using BomberosAPI.Application.Features.Auth;
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

// Controllers + FluentValidation
builder.Services.AddControllers();
builder.Services.AddValidation();

// JWT Authentication + Authorization policies
builder.Services.AddJwtAuthentication(builder.Configuration);

// ICurrentUserService — lee claims del JWT en cada request
builder.Services.AddCurrentUser();

// OpenAPI / Scalar / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseGlobalExceptionMiddleware();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BomberosAPI v1"));

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
