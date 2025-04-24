using Lume.Domain.Entities;

namespace Lume.Domain.Repositories
{
    public interface IStaffRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllStaff(); 
        Task<ApplicationUser?> GetStaffById(Guid id);
        Task<Guid> CreateStaff(ApplicationUser staff);
        Task DeleteStaff(ApplicationUser staff); 
        Task SaveChanges();
    }
}