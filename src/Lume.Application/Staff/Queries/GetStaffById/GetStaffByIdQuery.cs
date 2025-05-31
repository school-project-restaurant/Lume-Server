using Lume.Application.Staff.Dtos;
using MediatR;

namespace Lume.Application.Staff.Queries.GetStaffById;

public class GetStaffByIdQuery(Guid id) : IRequest<StaffDto>
{
    public Guid Id { get; set; } = id;
}