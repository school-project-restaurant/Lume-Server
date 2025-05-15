using Lume.Domain.Constants;
using Lume.Domain.Entities;

namespace Lume.Domain.Repositories
{
    public interface IStaffRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllStaff(); 
        Task<(IEnumerable<ApplicationUser>, int)> GetMatchingStaff(StaffFilterOptions filterOptions, StaffSortOptions sortOptions);
        Task<ApplicationUser?> GetStaffById(Guid id);
        Task<Guid> CreateStaff(ApplicationUser staff);
        Task DeleteStaff(ApplicationUser staff); 
        Task SaveChanges();
    }

    public class StaffFilterOptions
    {
        public string? SearchName { get; set; }
        public string? SearchSurname { get; set; }
        public string? SearchPhone { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }

    public class StaffSortOptions
    {
        public string? SortBy { get; set; }
        public SortDirection? SortDirection { get; set; }
    }
}