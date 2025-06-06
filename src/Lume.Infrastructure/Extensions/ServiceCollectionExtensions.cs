using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Lume.Infrastructure.Identity;
using Lume.Infrastructure.Persistence;
using Lume.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Lume.Infrastructure.Persistence.Seeders;
using Lume.Infrastructure.Persistence.Seeders.Profiles;
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
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IDishRepository, DishRepository>();
        services.AddScoped<ITablesRepository, TableRepository>();
        services.AddScoped<IStaffRepository, StaffRepository>();

        services.AddTransient<ISeeder, RolesSeeder>();
        services.AddTransient<ISeeder, UserSeeder>();
        services.AddTransient<ISeeder, TableSeeder>();
        services.AddTransient<ISeeder, ReservationSeeder>();
        services.AddTransient<ISeederOrchestrator, SeederOrchestrator>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        });

        services.AddIdentityApiEndpoints<ApplicationUser>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<RestaurantDbContext>();

        // Register the custom Blake3 password hasher
        services.AddScoped<IPasswordHasher<ApplicationUser>, Blake3PasswordHasher>();

        services.AddAutoMapper(typeof(SeedDataProfile).Assembly);
        services.AddStackExchangeRedisCache(options =>
        {
            var redisConnection = configuration.GetConnectionString("Redis") ?? "redis:6379";
            options.Configuration = redisConnection;
            options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
            {
                AbortOnConnectFail = true,
                EndPoints = { options.Configuration }
            };
        });
    }
}
