using Lume.Application.Tables.Dtos;
using Lume.Domain.Entities;

namespace Lume.Application.Customers;

public interface ITablesService
{
    Task<TablesDto> GetTableByNumber(int id);
    Task CreateTable(int Number, int Seats);
}
