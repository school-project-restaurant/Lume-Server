using AutoMapper;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandHandler(ILogger<CreateDishCommandHandler> logger, IMapper mapper,
    IDishRepository dishRepository) : IRequestHandler<CreateDishCommand, Guid>
{
    public async Task<Guid> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new plate {@Plate}", request); // TODO exclude sensitive data from logs
        var plate = mapper.Map<Dish>(request);
        var guid = await dishRepository.CreateDish(plate);
        return guid;
    }
}
