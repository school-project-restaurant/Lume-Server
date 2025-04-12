namespace Lume.Application.Staff.Dtos;

public class StaffDto
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Email { get; set; }
    public string PhoneNumber { get; set; } = null!;

}