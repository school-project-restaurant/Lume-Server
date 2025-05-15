using AutoMapper;
using Lume.Application.Common;
using Lume.Application.Tables.Dtos;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Tables.Queries.GetAllTables;

public class GetAllTablesQueryHandler(ILogger<GetAllTablesQueryHandler> logger, IMapper mapper,
    ITablesRepository tablesRepository) : IRequestHandler<GetAllTableQuery, PagedResult<TablesDto>>
{
    public async Task<PagedResult<TablesDto>> Handle(GetAllTableQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting tables with pagination from repository {OperationName}", nameof(tablesRepository.GetMatchingTables));
        
        TableFilterOptions filterOptions = mapper.Map<TableFilterOptions>(request);
        TableSortOptions sortOptions = mapper.Map<TableSortOptions>(request);
        var (tables, totalCount) = await tablesRepository.GetMatchingTables(filterOptions, sortOptions);
        
        var tablesDtos = mapper.Map<IEnumerable<TablesDto>>(tables);
        var result = new PagedResult<TablesDto>(tablesDtos, totalCount, request.PageSize, request.PageIndex);
        
        return result;
    }
}
