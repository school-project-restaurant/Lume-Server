using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Dishes.Commands.DeleteDish;

public class DeleteDishCommandHandler(ILogger<DeleteDishCommandHandler> logger,
    IDishRepository dishRepository) : IRequestHandler<DeleteDishCommand, bool>
{
    public async Task<bool> Handle(DeleteDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting plate with id {PlateGuid}", request.Id);
        var plate = await dishRepository.GetDishById(request.Id);
        if (plate is null) return false;

        await dishRepository.DeleteDish(plate);
        return true;
    }
}
