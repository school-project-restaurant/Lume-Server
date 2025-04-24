using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lume.Infrastructure.Persistence.Repositories;

internal class StaffRepository(RestaurantDbContext dbContext) : IStaffRepository
{
    public async Task<IEnumerable<Staff>> GetAllStaff()
    {
        var staff = await dbContext.Staffs.ToListAsync();
        return staff;
    }

    public async Task<Staff?> GetStaffById(Guid id)
    {
        var staff = await dbContext.Staffs.FirstOrDefaultAsync(x => x.Id == id);
        return staff;
    }

    public async Task<Guid> CreateStaff(Staff staff)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteStaff(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChanges()
    {
        throw new NotImplementedException();
    }
}