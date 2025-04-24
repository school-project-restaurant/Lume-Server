using MediatR;

namespace Lume.Application.Staff.Commands.CreateStaff;

public class CreateStaffCommand : IRequest<Guid>
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public double Salary { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
}