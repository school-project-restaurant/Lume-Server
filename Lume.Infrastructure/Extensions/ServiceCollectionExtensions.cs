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
        services.AddDbContext<RestaurantDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<ICustomerRepository, CustomerRepository>();
    }
}