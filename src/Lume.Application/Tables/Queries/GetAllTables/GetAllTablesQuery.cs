using Lume.Application.Common;
using Lume.Application.Tables.Dtos;
using Lume.Domain.Constants;
using MediatR;

namespace Lume.Application.Tables.Queries.GetAllTables;

public class GetAllTableQuery : IRequest<PagedResult<TablesDto>>
{
    public int? SearchNumber { get; set; }
    public int? SearchMinSeats { get; set; }
    public int? SearchMaxSeats { get; set; }
    
    public int PageSize { get; set; } = 10; // Default page size
    public int PageIndex { get; set; } = 1; // Default page index
    
    public string? SortBy { get; set; }
    public SortDirection? SortDirection { get; set; }
}
