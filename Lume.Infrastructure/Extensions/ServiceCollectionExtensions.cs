using Lume.Domain.Repositories;
using Lume.Infrastructure.Persistence;
using Lume.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lume.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add the infrastructure services to the DI container
    /// </summary>
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")))
        {
            string envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Lume.API", "Secrets.env");
            if (File.Exists(envPath))
            {
                DotNetEnv.Env.Load(envPath);
            }
        }
        
        services.AddDbContext<RestaurantDbContext>(options =>
            options.UseNpgsql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")));
        services.AddScoped<ICustomerRepository, CustomerRepository>();
    }
}