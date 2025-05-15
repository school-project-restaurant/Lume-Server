using AutoMapper;
using Lume.Application.Tables.Dtos;
using Lume.Domain.Exceptions;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Tables.Queries.GetTableByNumber;

public class GetTableByNumberQueryHandler(ILogger<GetTableByNumberQueryHandler> logger, IMapper mapper,
    ITablesRepository tablesRepository) : IRequestHandler<GetTableByNumberQuery, TablesDto>
{
    public async Task<TablesDto> Handle(GetTableByNumberQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting tables with number {@Number}", request.Number);
        var table = await tablesRepository.GetTableByNumber(request.Number);
        if (table is null)
            throw new NotFoundException(nameof(table), request.Number);

        var tableDto = mapper.Map<TablesDto>(table);
        return tableDto;
    }
}
