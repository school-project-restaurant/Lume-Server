using AutoMapper;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Plates.Commands.CreatePlate;

public class CreatePlateCommandHandler(ILogger<CreatePlateCommandHandler> logger, IMapper mapper,
    IPlateRepository plateRepository) : IRequestHandler<CreatePlateCommand, Guid>
{
    public async Task<Guid> Handle(CreatePlateCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new plate {@Plate}", request); // TODO exclude sensitive data from logs
        var plate = mapper.Map<Plate>(request);
        var guid = await plateRepository.CreatePlate(plate);
        return guid;
    }
}
