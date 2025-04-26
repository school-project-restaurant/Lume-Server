using AutoMapper;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Tables.Commands.UpdateTable;

public class UpdateTableCommandHandler(ILogger<UpdateTableCommandHandler> logger, IMapper mapper,
    ITablesRepository tablesRepository) : IRequestHandler<UpdateTableCommand, bool>
{
    public async Task<bool> Handle(UpdateTableCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating table with number {TableNumber}", request.Number);
        var table = await tablesRepository.GetTableByNumber(request.Number);
        if (table is null) return false;

        mapper.Map(request, table);
        await tablesRepository.SaveChanges();
        return true;
    }
}
