using Lume.Domain.Entities;

namespace Lume.Domain.Repositories;

public interface ITablesRepository
{
    Task<IEnumerable<ApplicationUser>> GetAllTables();
    Task<int> GetTablesByNumber(Guid id);
    Task<Guid> CreateTable(ApplicationUser table);
    Task DeleteTable(ApplicationUser table);
    Task SaveChanges();
}
