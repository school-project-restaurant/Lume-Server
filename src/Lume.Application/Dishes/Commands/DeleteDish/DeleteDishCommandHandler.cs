using Lume.Domain.Exceptions;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Dishes.Commands.DeleteDish;

public class DeleteDishCommandHandler(ILogger<DeleteDishCommandHandler> logger,
    IDishRepository dishRepository) : IRequestHandler<DeleteDishCommand>
{
    public async Task Handle(DeleteDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting plate with id {PlateGuid}", request.Id);
        var dish = await dishRepository.GetDishById(request.Id);
        if (dish is null)
            throw new NotFoundException(nameof(dish), request.Id);

        await dishRepository.DeleteDish(dish);
    }
}
