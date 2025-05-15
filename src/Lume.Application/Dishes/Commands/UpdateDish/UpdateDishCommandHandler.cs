using AutoMapper;
using Lume.Domain.Exceptions;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Dishes.Commands.UpdateDish;

public class UpdateDishCommandHandler(ILogger<UpdateDishCommandHandler> logger, IMapper mapper,
    IDishRepository dishRepository) : IRequestHandler<UpdateDishCommand>
{
    public async Task Handle(UpdateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating plate with id {PlateId}", request.Id); // Use request.Id
        var plate = await dishRepository.GetDishById(request.Id); // Use request.Id
        if (plate is null)
            throw new NotFoundException(nameof(plate), request.Id); 

        mapper.Map(request, plate);
        await dishRepository.SaveChanges();
    }
}
