namespace Lume.Domain.Entities;

public class Staff : ApplicationUser
{
    public int Salary { get; set; }
    public bool IsActive { get; set; }
    public int MonthHours { get; set; }
}