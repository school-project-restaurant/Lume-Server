using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Plates.Commands.DeletePlate;

public class DeletePlateCommandHandler(ILogger<DeletePlateCommandHandler> logger,
    IPlateRepository plateRepository) : IRequestHandler<DeletePlateCommand, bool>
{
    public async Task<bool> Handle(DeletePlateCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting plate with id {PlateGuid}", request.Id);
        var plate = await plateRepository.GetPlateById(request.Id);
        if (plate is null) return false;

        await plateRepository.DeletePlate(plate);
        return true;
    }
}
