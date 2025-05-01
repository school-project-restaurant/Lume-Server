using AutoMapper;
using Lume.Application.Dishes.Dtos;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Dishes.Queries.GetAllDishes;

public class GetAllDishesQueryHandler(ILogger<GetAllDishesQueryHandler> logger, IMapper mapper,
    IDishRepository dishRepository) : IRequestHandler<GetAllDishesQuery, IEnumerable<DishDto>>
{
    public async Task<IEnumerable<DishDto>> Handle(GetAllDishesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all plate from repository {OperationName}", nameof(dishRepository.GetAllDishes));
        var plate = await dishRepository.GetAllDishes();

        var plateDtos = mapper.Map<IEnumerable<DishDto>>(plate);
        return plateDtos!;
    }
}
