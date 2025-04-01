using Lume.Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace Lume.Infrastructure.Persistence.Seeders;

internal class RolesSeeder(RestaurantDbContext dbContext) : ISeeder
{
    public async Task SeedAsync()
    {
        if (!dbContext.Roles.Any())
        {
            var roles = GetRoles();
            dbContext.Roles.AddRange(roles);
            await dbContext.SaveChangesAsync();
        }
    }

    private IEnumerable<IdentityRole<Guid>> GetRoles()
    {
        List<IdentityRole<Guid>> roles =
        [
            new(UserRoels.Customer)
            {
                NormalizedName = UserRoels.Customer.ToUpper()
            },
            new(UserRoels.Admin)
            {
                NormalizedName = UserRoels.Admin.ToUpper()
            },
            new(UserRoels.Staff)
            {
                NormalizedName = UserRoels.Staff.ToUpper()
            },
            new(UserRoels.Chef)
            {
                NormalizedName = UserRoels.Chef.ToUpper()
            }
        ];
        return roles;
    }
}