using Lume.Domain.Entities;

namespace Lume.Domain.Repositories;

public interface ITablesRepository
{
    Task<IEnumerable<Table>> GetAllTables();
    Task<Table?> GetTableByNumber(int id);
    Task<int> CreateTable(Table table);
    Task DeleteTable(Table table);
    Task SaveChanges();
}
