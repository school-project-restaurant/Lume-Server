using Lume.Application.Plates.Dtos;
using MediatR;

namespace Lume.Application.Plates.Queries.GetAllPlates;

public class GetAllPlatesQuery : IRequest<IEnumerable<PlateDto>>
{

}
