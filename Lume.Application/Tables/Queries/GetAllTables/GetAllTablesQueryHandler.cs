using AutoMapper;
using Lume.Application.Tables.Dtos;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Tables.Queries.GetAllTables;

public class GetAllTablesQueryHandler(ILogger<GetAllTablesQueryHandler> logger, IMapper mapper,
    ITablesRepository tablesRepository) : IRequestHandler<GetAllTableQuery, IEnumerable<TablesDto>>
{
    public async Task<IEnumerable<TablesDto>> Handle(GetAllTableQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all tables from repository {OperationName}", nameof(tablesRepository.GetAllTables));
        var table = await tablesRepository.GetAllTables();

        var tablesDtos = mapper.Map<IEnumerable<TablesDto>>(table);
        return tablesDtos!;
    }
}
