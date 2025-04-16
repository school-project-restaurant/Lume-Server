using Lume.Domain.Entities;

namespace Lume.Domain.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<ApplicationUser>> GetAllCustomers();
    Task<ApplicationUser?> GetCustomerById(Guid id);
    Task<Guid> CreateCustomer(ApplicationUser customer);
    Task DeleteCustomer(ApplicationUser customer);
    Task SaveChanges();
}