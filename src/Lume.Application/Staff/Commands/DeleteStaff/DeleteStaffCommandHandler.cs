using Lume.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Staff.Commands.DeleteStaff;

public class DeleteStaffCommandHandler(ILogger<DeleteStaffCommandHandler> logger, 
    IStaffRepository staffRepository) : IRequestHandler<DeleteStaffCommand, bool>
{
    public async Task<bool> Handle(DeleteStaffCommand request, CancellationToken cancellationToken)
    {
        
        logger.LogInformation($"Delete staff with id {request.Id}");
        var staff = await staffRepository.GetStaffById(request.Id);
        if (staff is null) return false;
        
        await staffRepository.DeleteStaff(staff);
        return true;
    }
}