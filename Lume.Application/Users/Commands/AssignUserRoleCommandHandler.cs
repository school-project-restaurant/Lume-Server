using Lume.Domain.Entities;
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
                   ?? throw new KeyNotFoundException(nameof(ApplicationUser)); // TODO create NotFoundException 

        var role = await roleManager.FindByNameAsync(request.RoleName)
                   ?? throw new KeyNotFoundException(nameof(IdentityRole<Guid>)); // TODO create NotFoundException

        await userManager.AddToRoleAsync(user, role.Name!);
    }
}