using Lume.Application.Plates.Dtos;
using MediatR;

namespace Lume.Application.Plates.Queries.GetPlateById;

public class GetPlateByIdQuery(Guid id) : IRequest<PlateDto?>
{
    public Guid Id { get; set; } = id;
}
