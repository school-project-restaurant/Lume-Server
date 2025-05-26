using AutoMapper;
using Lume.Application.Dishes.Dtos;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Dishes.Queries.GetDishById;

public class GetDishByIdQueryHandler(ILogger<GetDishByIdQueryHandler> logger, IMapper mapper,
    IDishRepository dishRepository) : IRequestHandler<GetDishByIdQuery, DishDto?>
{
    public async Task<DishDto?> Handle(GetDishByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting plate with id {@Id}", request.Id);
        var plate = await dishRepository.GetDishById(request.Id);

        var plateDto = mapper.Map<DishDto>(plate);
        return plateDto;
    }
}
