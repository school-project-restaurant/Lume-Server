using AutoMapper;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Plates.Commands.UpdatePlate;

public class UpdatePlateCommandHandler(ILogger<UpdatePlateCommandHandler> logger, IMapper mapper,
    IPlateRepository plateRepository) : IRequestHandler<UpdatePlateCommand, bool>
{
    public async Task<bool> Handle(UpdatePlateCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating plate with id {PlateId}", request.Id); // Use request.Id
        var plate = await plateRepository.GetPlateById(request.Id); // Use request.Id
        if (plate is null) return false;

        mapper.Map(request, plate);
        await plateRepository.SaveChanges();
        return true;
    }
}
