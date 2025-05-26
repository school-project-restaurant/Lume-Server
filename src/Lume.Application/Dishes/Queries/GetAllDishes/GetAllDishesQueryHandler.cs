using AutoMapper;
using Lume.Application.Common;
using Lume.Application.Dishes.Dtos;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Dishes.Queries.GetAllDishes;

public class GetAllDishesQueryHandler(ILogger<GetAllDishesQueryHandler> logger, IMapper mapper,
    IDishRepository dishRepository) : IRequestHandler<GetAllDishesQuery, PagedResult<DishDto>>
{
    public async Task<PagedResult<DishDto>> Handle(GetAllDishesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting dishes with pagination from repository {OperationName}", nameof(dishRepository.GetMatchingDishes));
        
        DishFilterOptions filterOptions = mapper.Map<DishFilterOptions>(request);
        DishSortOptions sortOptions = mapper.Map<DishSortOptions>(request);
        var (dishes, totalCount) = await dishRepository.GetMatchingDishes(filterOptions, sortOptions);
        
        var dishDtos = mapper.Map<IEnumerable<DishDto>>(dishes);
        var result = new PagedResult<DishDto>(dishDtos, totalCount, request.PageSize, request.PageIndex);
        
        return result;
    }
}
