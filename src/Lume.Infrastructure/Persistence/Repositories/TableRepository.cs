using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lume.Infrastructure.Persistence.Repositories;

internal class TableRepository(RestaurantDbContext dbContext) : ITablesRepository
{
    public async Task<IEnumerable<Table>> GetAllTables()
    {
        return await dbContext.Tables.ToListAsync();
    }

    public async Task<Table?> GetTableByNumber(int id)
    {
        return await dbContext.Tables.FirstOrDefaultAsync(t => t.Number == id);
    }

    public async Task<int> CreateTable(Table table)
    {
        await dbContext.Tables.AddAsync(table);
        await dbContext.SaveChangesAsync();
        return table.Number;
    }

    public async Task DeleteTable(Table table)
    {
        dbContext.Tables.Remove(table);
        await dbContext.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
        await dbContext.SaveChangesAsync();
    }
}
