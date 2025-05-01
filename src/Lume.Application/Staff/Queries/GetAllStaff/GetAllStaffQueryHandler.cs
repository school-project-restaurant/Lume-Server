using AutoMapper;
using Lume.Application.Staff.Dtos;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Staff.Queries.GetAllStaff;

public class GetAllStaffQueryHandler(ILogger<GetAllStaffQueryHandler> logger, IMapper mapper, IStaffRepository staffRepository)
    : IRequestHandler<GetAllStaffQuery, IEnumerable<StaffDto>>
{
    public async Task<IEnumerable<StaffDto>> Handle(GetAllStaffQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Get all staff members");

        var staffMembers = await staffRepository.GetAllStaff();
        var staffDtos = mapper.Map<IEnumerable<StaffDto>>(staffMembers);

        return staffDtos;
    }
}
