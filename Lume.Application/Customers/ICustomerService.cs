using Lume.Application.Customers.Dtos;
using Lume.Domain.Entities;

namespace Lume.Application.Customers;

public interface ICustomerService
{
    public Task<IEnumerable<CustomerDto>> GetAll();
    Task<CustomerDto?> GetById(int id);
    Task Create(CustomerDto customer);
    Task Update(int id, CustomerDto customer);
    Task Delete(int id);
}