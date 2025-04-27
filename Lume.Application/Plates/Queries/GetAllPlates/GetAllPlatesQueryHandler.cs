using AutoMapper;
using Lume.Application.Plates.Dtos;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Plates.Queries.GetAllPlates;

public class GetAllPlatesQueryHandler(ILogger<GetAllPlatesQueryHandler> logger, IMapper mapper,
    IPlateRepository plateRepository) : IRequestHandler<GetAllPlatesQuery, IEnumerable<PlateDto>>
{
    public async Task<IEnumerable<PlateDto>> Handle(GetAllPlatesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all plate from repository {OperationName}", nameof(plateRepository.GetAllPlates));
        var plate = await plateRepository.GetAllPlates();

        var plateDtos = mapper.Map<IEnumerable<PlateDto>>(plate);
        return plateDtos!;
    }
}
