using AutoMapper;
using Lume.Application.Plates.Dtos;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Plates.Queries.GetPlateById;

public class GetPlateByIdQueryHandler(ILogger<GetPlateByIdQueryHandler> logger, IMapper mapper,
    IPlateRepository plateRepository) : IRequestHandler<GetPlateByIdQuery, PlateDto?>
{
    public async Task<PlateDto?> Handle(GetPlateByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting plate with id {@Id}", request.Id);
        var plate = await plateRepository.GetPlateById(request.Id);

        var plateDto = mapper.Map<PlateDto>(plate);
        return plateDto;
    }
}
