using AutoMapper;
using Lume.Application.Customers.Commands.CreateCustomer;
using Lume.Application.Customers.Commands.UpdateCustomer;
using Lume.Domain.Entities;

namespace Lume.Application.Customers.Dtos;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<ApplicationUser, CustomerDto>();
        CreateMap<CreateCustomerCommand, ApplicationUser>();
        CreateMap<UpdateCustomerCommand, ApplicationUser>();
    }
}