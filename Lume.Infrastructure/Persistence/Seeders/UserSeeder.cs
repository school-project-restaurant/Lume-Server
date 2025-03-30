using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Lume.Domain.Entities;
using Lume.Infrastructure.Persistence.Seeders.Models;

namespace Lume.Infrastructure.Persistence.Seeders;

internal class UserSeeder(
    UserManager<Customer> customerManager,
    UserManager<Staff> staffManager,
    IMapper mapper,
    RestaurantDbContext dbContext)
    : BaseSeeder, ISeeder
{

    public async Task SeedAsync()
    {
        var seedData = await LoadSeedDataAsync<SeedData>();

        if (await dbContext.Database.CanConnectAsync() && !customerManager.Users.Any())
        {
            const string defaultPassword = "DefaultPassword123!";

            foreach (var customerData in seedData.Customers)
            {
                var customer = mapper.Map<Customer>(customerData);
                var result = await customerManager.CreateAsync(customer, defaultPassword);

                if (!result.Succeeded)
                {
                    Console.WriteLine($"[UserSeeder] Failed to create customer {customer.Email}: " +
                                      string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            foreach (var staffData in seedData.Staff)
            {
                var staff = mapper.Map<Staff>(staffData);

                var result = await staffManager.CreateAsync(staff, defaultPassword);

                if (!result.Succeeded)
                {
                    Console.WriteLine($"[UserSeeder] Failed to create staff {staff.Email}: " +
                                      string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}