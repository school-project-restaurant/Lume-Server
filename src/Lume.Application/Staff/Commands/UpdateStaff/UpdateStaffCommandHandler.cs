using AutoMapper;
using Lume.Domain.Exceptions;
using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Staff.Commands.UpdateStaff;

public class UpdateStaffCommandHandler(ILogger<UpdateStaffCommandHandler> logger, IMapper mapper,
    IStaffRepository staffRepository) : IRequestHandler<UpdateStaffCommand>
{
    public async Task Handle(UpdateStaffCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Update staff with id {request.Id}"); 
        var staff = await staffRepository.GetStaffById(request.Id);
        if (staff is null)
            throw new NotFoundException(nameof(staff), request.Id);
        
        mapper.Map(request, staff);
        await staffRepository.SaveChanges();
    }
}