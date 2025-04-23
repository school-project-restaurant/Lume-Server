using Lume.Domain.Entities;
using Lume.Domain.Repositories;

namespace Lume.Infrastructure.Persistence.Repositories;

public class TableRepository : ITablesRepository
{
    public Task<IEnumerable<Table>> GetAllTables()
    {
        throw new NotImplementedException();
    }

    public Task<Table?> GetTableByNumber(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> CreateTable(Table table)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTable(Table table)
    {
        throw new NotImplementedException();
    }

    public Task SaveChanges()
    {
        throw new NotImplementedException();
    }
}