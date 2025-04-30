using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Tables.Commands.DeleteTable;

public class DeleteTableCommandHandler(ILogger<DeleteTableCommandHandler> logger,
    ITablesRepository tablesRepository) : IRequestHandler<DeleteTableCommand, bool>
{
    public async Task<bool> Handle(DeleteTableCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting table with number {TableNumber}", request.Number);
        var table = await tablesRepository.GetTableByNumber(request.Number);
        if (table is null) return false;

        await tablesRepository.DeleteTable(table);
        return true;
    }
}
