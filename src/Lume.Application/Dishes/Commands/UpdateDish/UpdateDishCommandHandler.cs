using AutoMapper;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Dishes.Commands.UpdateDish;

public class UpdateDishCommandHandler(ILogger<UpdateDishCommandHandler> logger, IMapper mapper,
    IDishRepository dishRepository) : IRequestHandler<UpdateDishCommand, bool>
{
    public async Task<bool> Handle(UpdateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating plate with id {PlateId}", request.Id); // Use request.Id
        var plate = await dishRepository.GetDishById(request.Id); // Use request.Id
        if (plate is null) return false;

        mapper.Map(request, plate);
        await dishRepository.SaveChanges();
        return true;
    }
}
