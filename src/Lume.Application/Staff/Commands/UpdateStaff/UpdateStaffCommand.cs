using MediatR;

namespace Lume.Application.Staff.Commands.UpdateStaff;

public class UpdateStaffCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public int Salary { get; set; }
    public string PhoneNumber { get; set; } = null!;
}