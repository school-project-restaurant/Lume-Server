using AutoMapper;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Tables.Commands.CreateTable;

public class CreateTableCommandHandler(ILogger<CreateTableCommandHandler> logger, IMapper mapper,
    ITablesRepository tablesRepository) : IRequestHandler<CreateTableCommand, int>
{
    public async Task<int> Handle(CreateTableCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new table {@Table}", request); // TODO exclude sensitive data from logs
        var table = mapper.Map<Table>(request);
        var number = await tablesRepository.CreateTable(table);
        return number;
    }
}
