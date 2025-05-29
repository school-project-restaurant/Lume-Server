using Lume.Domain.Entities;

namespace Lume.Domain.Repositories;

public interface ITablesRepository
{
    Task<IEnumerable<Table>> GetAllTables();
    Task<(IEnumerable<Table>, int)> GetMatchingTables(TableFilterOptions filterOptions, TableSortOptions sortOptions);
    Task<Table?> GetTableByNumber(int id);
    Task<int> CreateTable(Table table);
    Task DeleteTable(Table table);
    Task SaveChanges();
}

public class TableFilterOptions
{
    public int? SearchNumber { get; set; }
    public int? SearchMinSeats { get; set; }
    public int? SearchMaxSeats { get; set; }
    public int PageSize { get; set; } = 10;
    public int PageIndex { get; set; } = 1;
}

public class TableSortOptions
{
    public string? SortBy { get; set; }
    public Constants.SortDirection? SortDirection { get; set; }
}
