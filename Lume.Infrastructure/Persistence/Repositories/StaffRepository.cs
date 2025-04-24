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

    public Task<Staff?> GetStaffById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> CreateStaff(Staff staff)
    {
        throw new NotImplementedException();
    }

    public Task DeleteStaff(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task SaveChanges()
    {
        throw new NotImplementedException();
    }
}