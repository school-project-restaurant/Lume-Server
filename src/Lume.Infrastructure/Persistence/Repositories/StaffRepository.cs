using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lume.Infrastructure.Persistence.Repositories;

internal class StaffRepository(RestaurantDbContext dbContext) : IStaffRepository
{
    public async Task<IEnumerable<ApplicationUser>> GetAllStaff()
    {
        var staff = await dbContext.Users.Where(u => u.UserType == "Staff").ToListAsync();
        return staff;
    }

    public async Task<ApplicationUser?> GetStaffById(Guid id)
    {
        var staff = await dbContext.Users.FirstOrDefaultAsync(x => 
            x.Id == id && x.UserType == "Staff");
        return staff;
    }

    public async Task<Guid> CreateStaff(ApplicationUser staff)
    {
        staff.UserType = "Staff";
        dbContext.Users.Add(staff);
        await dbContext.SaveChangesAsync();
        return staff.Id;
    }

    public async Task DeleteStaff(ApplicationUser staff)
    {
        dbContext.Users.Remove(staff);
        await dbContext.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
       await dbContext.SaveChangesAsync();
    }
}