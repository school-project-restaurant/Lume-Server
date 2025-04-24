using Lume.Domain.Entities;

namespace Lume.Domain.Repositories
{
    public interface IStaffRepository
    {
        Task<IEnumerable<Staff>> GetAllStaff(); 
        Task<Staff?> GetStaffById(Guid id);
        Task<Guid> CreateStaff(Staff staff);
        Task DeleteStaff(Staff staff); 
        Task SaveChanges();
    }
}