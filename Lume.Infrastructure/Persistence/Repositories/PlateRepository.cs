using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lume.Infrastructure.Persistence.Repositories;

internal class PlateRepository(RestaurantDbContext dbContext) : IPlateRepository
{
    public async Task<IEnumerable<Plate>> GetAllPlates()
    {
        return await dbContext.Plates.ToListAsync();
    }

    public async Task<Plate?> GetPlateById(Guid id)
    {
        return await dbContext.Plates.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Guid> CreatePlate(Plate plate)
    {
        await dbContext.Plates.AddAsync(plate);
        await dbContext.SaveChangesAsync();
        return plate.Id;
    }

    public async Task DeletePlate(Plate plate)
    {
        dbContext.Plates.Remove(plate);
        await dbContext.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
        await dbContext.SaveChangesAsync();
    }
}
