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
            new(UserRoles.Customer)
            {
                NormalizedName = UserRoles.Customer.ToUpper()
            },
            new(UserRoles.Admin)
            {
                NormalizedName = UserRoles.Admin.ToUpper()
            },
            new(UserRoles.Staff)
            {
                NormalizedName = UserRoles.Staff.ToUpper()
            },
            new(UserRoles.Chef)
            {
                NormalizedName = UserRoles.Chef.ToUpper()
            }
        ];
        return roles;
    }
}