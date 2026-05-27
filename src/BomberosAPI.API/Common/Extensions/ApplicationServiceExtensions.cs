using System.Reflection;
using BomberosAPI.API.Common.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace BomberosAPI.API.Common.Extensions;

public static class ApplicationServiceExtensions
{
    /// <summary>
    /// Registra FluentValidation y deshabilita la validacion automatica de ASP.NET Core
    /// para que el middleware global maneje los errores de forma consistente.
    /// </summary>
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        // Registra todos los validadores del assembly de Application
        services.AddValidatorsFromAssembly(
            Assembly.Load("BomberosAPI.Application"),
            includeInternalTypes: true);

        // Suprime la respuesta automatica 400 de ModelState para que el
        // GlobalExceptionMiddleware sea el unico punto de manejo de errores
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        return services;
    }

    /// <summary>
    /// Registra el middleware global de excepciones en el pipeline.
    /// Debe llamarse lo mas temprano posible en Program.cs.
    /// </summary>
    public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
        return app;
    }
}
