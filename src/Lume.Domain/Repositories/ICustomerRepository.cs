using Lume.Domain.Constants;
using Lume.Domain.Entities;

namespace Lume.Domain.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<ApplicationUser>> GetAllCustomers();
    Task<(IEnumerable<ApplicationUser>, int)> GetMatchingCustomers(CustomerFilterOptions filterOptions);
    Task<ApplicationUser?> GetCustomerById(Guid id);
    Task<Guid> CreateCustomer(ApplicationUser customer);
    Task DeleteCustomer(ApplicationUser customer);
    Task SaveChanges();
}

public class CustomerFilterOptions
{
    public string? SearchEmail { get; set; }
    public string? SearchName { get; set; }
    public string? SearchSurname { get; set; }
    public Guid? ReservationId { get; set; }
    public string? SearchPhone { get; set; }

    public int PageSize { get; set; }
    public int PageIndex { get; set; }
}

public class CustomerSortOptions
{
    public string? SortBy { get; set; }
    public SortDirection SortDirection { get; set; }
}