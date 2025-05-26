using AutoMapper;
using Lume.Application.Common;
using Lume.Application.Staff.Dtos;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Staff.Queries.GetAllStaff;

public class GetAllStaffQueryHandler(ILogger<GetAllStaffQueryHandler> logger, IMapper mapper, IStaffRepository staffRepository)
    : IRequestHandler<GetAllStaffQuery, PagedResult<StaffDto>>
{
    public async Task<PagedResult<StaffDto>> Handle(GetAllStaffQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all staff members from repository {OperationName}", nameof(staffRepository.GetMatchingStaff));
        
        StaffFilterOptions filterOptions = mapper.Map<StaffFilterOptions>(request);
        StaffSortOptions sortOptions = mapper.Map<StaffSortOptions>(request);
        var (staffMembers, totalCount) = await staffRepository.GetMatchingStaff(filterOptions, sortOptions);
        
        var staffDtos = mapper.Map<IEnumerable<StaffDto>>(staffMembers);
        var result = new PagedResult<StaffDto>(staffDtos, totalCount, request.PageSize, request.PageIndex);
        
        return result;
    }
}
