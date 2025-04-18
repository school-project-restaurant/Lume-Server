using Lume.Domain.Entities;

namespace Lume.Domain.Repositories
{
    public interface IStaffRepository
    {
        Task<IEnumerable<Staff>> GetAllStaff(); 
        Task<Staff?> GetStaffById(Guid id);
        Task<Guid> CreateStaff(Staff staff);
        Task DeleteStaff(Guid id); 
        Task SaveChanges();
    }
}