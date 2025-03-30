using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Lume.Infrastructure.Persistence;
using Lume.Infrastructure.Persistence.Repositories;
using Lume.Infrastructure.Persistence.Seeders;
using Lume.Infrastructure.Persistence.Seeders.Profiles;
using Lume.Infrastructure.Persistence.Seeders.ReservationSeeders;
using Lume.Infrastructure.Persistence.Seeders.TableSeeders;
using Lume.Infrastructure.Persistence.Seeders.UserSeeders;
using Microsoft.AspNetCore.Identity;
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
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        if (string.IsNullOrEmpty(connectionString))
        {
            var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Lume.API", "Secrets.env");
            if (File.Exists(envPath))
            {
                DotNetEnv.Env.Load(envPath);
                connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            }
        }

        services.AddDbContext<RestaurantDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        
        services.AddScoped<IUserSeeder, UserSeeder>();
        services.AddScoped<ITableSeeder, TableSeeder>();
        services.AddScoped<IReservationSeeder, ReservationSeeder>();
        
        services.AddIdentity<Customer, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<RestaurantDbContext>()
            .AddDefaultTokenProviders();

        services.AddIdentityCore<Staff>()
            .AddEntityFrameworkStores<RestaurantDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddAutoMapper(typeof(SeedDataProfile).Assembly);
    }
}