using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Lume.Domain.Entities;
using Lume.Infrastructure.Persistence.Seeders.Models;

namespace Lume.Infrastructure.Persistence.Seeders;

internal class UserSeeder(
    UserManager<ApplicationUser> userManager,
    IMapper mapper,
    RestaurantDbContext dbContext)
    : BaseSeeder, ISeeder
{
    public async Task SeedAsync()
    {
        var seedData = await LoadSeedDataAsync<SeedData>();

        if (await dbContext.Database.CanConnectAsync() && !userManager.Users.Any())
        {
            const string defaultPassword = "DefaultPassword123!";

            foreach (var customerData in seedData.Customers)
            {
                var customer = mapper.Map<ApplicationUser>(customerData);
                // Set the discriminator or role for customer
                await CreateUserWithRole(customer, defaultPassword, "Customer");
            }

            foreach (var staffData in seedData.Staff)
            {
                var staff = mapper.Map<ApplicationUser>(staffData);
                // Set the discriminator or role for staff
                await CreateUserWithRole(staff, defaultPassword, "Staff");
            }

            await dbContext.SaveChangesAsync();
        }
    }

    private async Task CreateUserWithRole(ApplicationUser user, string password, string roleName)
    {
        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, roleName);
        }
        else
        {
            Console.WriteLine($"[UserSeeder] Failed to create {roleName.ToLower()} {user.Email}: " +
                              string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}