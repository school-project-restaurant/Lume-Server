using AutoMapper;
using Lume.Application.Customers.Commands.CreateCustomer;
using Lume.Application.Customers.Commands.UpdateCustomer;
using Lume.Application.Customers.Queries.GetAllCustomers;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;

namespace Lume.Application.Customers.Dtos;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<ApplicationUser, CustomerDto>();
        CreateMap<CreateCustomerCommand, ApplicationUser>();
        CreateMap<UpdateCustomerCommand, ApplicationUser>();
        CreateMap<GetAllCustomersQuery, CustomerFilterOptions>();
        CreateMap<GetAllCustomersQuery, CustomerSortOptions>();
    }
}