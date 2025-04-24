using AutoMapper;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Staff.Commands.CreateStaff;

public class CreateStaffCommandHandler(ILogger<CreateStaffCommandHandler> logger, IMapper mapper,
    IStaffRepository staffRepository) : IRequestHandler<CreateStaffCommand, Guid>
{
    public async Task<Guid> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new staff");
        var staff = mapper.Map<ApplicationUser>(request);
        var id = await staffRepository.CreateStaff(staff);
        return id;
    }
}