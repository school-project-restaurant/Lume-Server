using AutoMapper;
using Lume.Application.Staff.Dtos;
using Lume.Domain.Exceptions;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Staff.Queries.GetStaffById;

public class GetStaffByIdQueryHandler(ILogger<GetStaffByIdQueryHandler> logger,
    IMapper mapper, IStaffRepository staffRepository) : IRequestHandler<GetStaffByIdQuery, StaffDto>
{
    public async Task<StaffDto> Handle(GetStaffByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving staff member {@Request}", request);
        var staffMember = await staffRepository.GetStaffById(request.Id);
        if (staffMember is null)
            throw new NotFoundException(nameof(staffMember), request.Id);
        
        var staffDto = mapper.Map<StaffDto>(staffMember);
        return staffDto;
    }
}