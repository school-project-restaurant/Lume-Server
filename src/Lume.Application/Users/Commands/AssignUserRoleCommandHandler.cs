using Lume.Domain.Entities;
using Lume.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Lume.Application.Users.Commands;

public class AssignUserRoleCommandHandler(
    ILogger<AssignUserRoleCommandHandler> logger,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole<Guid>> roleManager) : IRequestHandler<AssignUserRoleCommand>
{
    public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Assigning user role: {@Request}", request);
        
        var user = await userManager.FindByEmailAsync(request.UserEmail)
                   ?? throw new NotFoundException(nameof(ApplicationUser), request.UserEmail);

        var role = await roleManager.FindByNameAsync(request.RoleName)
                   ?? throw new NotFoundException(nameof(IdentityRole<Guid>), request.RoleName);

        await userManager.AddToRoleAsync(user, role.Name!);
    }
}