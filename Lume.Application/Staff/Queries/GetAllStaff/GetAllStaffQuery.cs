using Lume.Application.Staff.Dtos;
using MediatR;

namespace Lume.Application.Staff.Queries.GetAllStaff;

public class GetAllStaffQuery : IRequest<IEnumerable<StaffDto>>
{

}