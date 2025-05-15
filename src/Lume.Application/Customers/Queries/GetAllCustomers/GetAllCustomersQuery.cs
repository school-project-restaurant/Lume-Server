using Lume.Application.Common;
using Lume.Application.Customers.Dtos;
using Lume.Application.Reservations.Dtos;
using Lume.Domain.Constants;
using MediatR;

namespace Lume.Application.Customers.Queries.GetAllCustomers;

public class GetAllCustomersQuery : IRequest<PagedResult<CustomerDto>>
{
    public string? SearchEmail { get; set; }
    public string? SearchName { get; set; }
    public string? SearchSurname { get; set; }
    public ReservationFindCustomerRequestModel? SearchReservation { get; set; }
    public string? SearchPhone { get; set; }

    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    
    public string? SortBy { get; set; }
    public SortDirection? SortDirection { get; set; }
}