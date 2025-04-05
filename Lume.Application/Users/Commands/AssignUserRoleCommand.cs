using MediatR;

namespace Lume.Application.Users.Commands;

public class AssignUserRoleCommand : IRequest
{
    public string UserEmail { get; set; } = null!;
    public string RoleName { get; set; } = null!;
}