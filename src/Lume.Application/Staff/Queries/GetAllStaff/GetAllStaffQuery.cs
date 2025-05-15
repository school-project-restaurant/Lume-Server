using Lume.Application.Common;
using Lume.Application.Staff.Dtos;
using Lume.Domain.Constants;
using MediatR;

namespace Lume.Application.Staff.Queries.GetAllStaff;

public class GetAllStaffQuery : IRequest<PagedResult<StaffDto>>
{
    public string? SearchEmail { get; set; }
    public string? SearchName { get; set; }
    public string? SearchSurname { get; set; }
    public string? SearchPhone { get; set; }

    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    
    public string? SortBy { get; set; }
    public SortDirection? SortDirection { get; set; }
}