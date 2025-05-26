using MediatR;

namespace Lume.Application.Staff.Commands.DeleteStaff;

public class DeleteStaffCommand(Guid id) : IRequest<bool>
{
    public Guid Id { get; set; } = id;
}