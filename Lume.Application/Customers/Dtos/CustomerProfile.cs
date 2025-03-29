using AutoMapper;
using Lume.Application.Customers.Commands.CreateCustomer;
using Lume.Application.Customers.Commands.UpdateCustomer;
using Lume.Domain.Entities;

namespace Lume.Application.Customers.Dtos;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateCustomerCommand, Customer>();
        CreateMap<UpdateCustomerCommand, Customer>();
    }
}