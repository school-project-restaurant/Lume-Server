using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Lume.Domain.Entities;

namespace Lume.Infrastructure.Persistence.Seeders;

internal class UserSeeder(
    UserManager<Customer> customerManager,
    UserManager<Staff> staffManager,
    RestaurantDbContext dbContext)
    : BaseSeeder, IUserSeeder
{

    public async Task SeedAsync()
    {
        var seedData = await LoadSeedDataAsync<SeedData>();

        if (await dbContext.Database.CanConnectAsync() && !customerManager.Users.Any())
        {
            const string defaultPassword = "DefaultPassword123!";

            foreach (var customerData in seedData.Customers)
            {
                var customer = new Customer
                {
                    Id = customerData.Id,
                    Email = customerData.Email,
                    UserName = customerData.Email,
                    PhoneNumber = customerData.PhoneNumber,
                    Name = customerData.Name,
                    Surname = customerData.Surname,
                    ReservationsId = customerData.ReservationsId.ToList(),
                    UserType = "Customer"
                };

                var result = await customerManager.CreateAsync(customer, defaultPassword);

                if (!result.Succeeded)
                {
                    Console.WriteLine($"[UserSeeder] Failed to create customer {customer.Email}: " +
                                      string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            foreach (var staffData in seedData.Staff)
            {
                var staff = new Staff
                {
                    Id = staffData.Id,
                    PhoneNumber = staffData.PhoneNumber,
                    UserName = staffData.PhoneNumber,
                    Name = staffData.Name,
                    Surname = staffData.Surname,
                    Salary = staffData.Salary,
                    IsActive = staffData.IsActive,
                    MonthHours = staffData.MonthHours,
                    UserType = "Staff"
                };

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

    private class SeedData
    {
        public IEnumerable<CustomerSeedDataModel> Customers { get; init; } = Array.Empty<CustomerSeedDataModel>();
        public IEnumerable<StaffSeedDataModel> Staff { get; init; } = Array.Empty<StaffSeedDataModel>();
    }

    private class BaseSeedDataModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    private class CustomerSeedDataModel : BaseSeedDataModel
    {
        public string Email { get; set; } = string.Empty;
        public IEnumerable<Guid> ReservationsId { get; set; } = [];
    }

    private class StaffSeedDataModel : BaseSeedDataModel
    {
        public int Salary { get; set; }
        public bool IsActive { get; set; }
        public int MonthHours { get; set; }
    }
}