using AutoMapper;
using Lume.Domain.Exceptions;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Tables.Commands.UpdateTable;

public class UpdateTableCommandHandler(ILogger<UpdateTableCommandHandler> logger, IMapper mapper,
    ITablesRepository tablesRepository) : IRequestHandler<UpdateTableCommand>
{
    public async Task Handle(UpdateTableCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating table with number {TableNumber}", request.Number);
        var table = await tablesRepository.GetTableByNumber(request.Number);
        if (table is null)
            throw new NotFoundException(nameof(table), request.Number);

        mapper.Map(request, table);
        await tablesRepository.SaveChanges();
    }
}
