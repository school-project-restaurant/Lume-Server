using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lume.Application.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add the application services to the DI container
    /// </summary>
    public static void AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddAutoMapper(applicationAssembly)
            .AddFluentValidationAutoValidation();
        
    }
}