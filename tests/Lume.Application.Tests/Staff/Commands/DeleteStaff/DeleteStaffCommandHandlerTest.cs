using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Lume.Application.Staff.Commands.DeleteStaff;
using Lume.Domain.Entities;
using Lume.Domain.Exceptions;
using Lume.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Lume.Application.Tests.Staff.Commands.DeleteStaff;

[TestSubject(typeof(DeleteStaffCommandHandler))]
public class DeleteStaffCommandHandlerTest
{
    [Fact]
    public async Task Handle_WhenRequestIsValid_ShouldDeleteStaff()
    {
        // arrange
        var loggerMock = new Mock<ILogger<DeleteStaffCommandHandler>>();
        var staffRepositoryMock = new Mock<IStaffRepository>();
        var staffId = Guid.NewGuid();
        var request = new DeleteStaffCommand(staffId);
        var staff = new ApplicationUser
        {
            Id = staffId
        };

        staffRepositoryMock.Setup(r => r.GetStaffById(staffId)).ReturnsAsync(staff);
        staffRepositoryMock.Setup(r => r.DeleteStaff(staff));

        var handler = new DeleteStaffCommandHandler(loggerMock.Object, staffRepositoryMock.Object);
        
        // act
        await handler.Handle(request, CancellationToken.None);
        
        // assert
        staffRepositoryMock.Verify(r => r.DeleteStaff(staff), Times.Once);
    }
    
    [Fact]
    public async Task Handle_WhenStaffDoesNotExist_ShouldThrowNotFoundException()
    {
        // arrange
        var loggerMock = new Mock<ILogger<DeleteStaffCommandHandler>>();
        var staffRepositoryMock = new Mock<IStaffRepository>();
        var staffId = Guid.NewGuid();
        var request = new DeleteStaffCommand(staffId);

        staffRepositoryMock.Setup(r => r.GetStaffById(staffId)).ReturnsAsync((ApplicationUser)null);

        var handler = new DeleteStaffCommandHandler(loggerMock.Object, staffRepositoryMock.Object);
        
        // act & assert
        await Assert.ThrowsAsync<NotFoundException>(() => 
            handler.Handle(request, CancellationToken.None));
            
        staffRepositoryMock.Verify(r => r.DeleteStaff(It.IsAny<ApplicationUser>()), Times.Never);
    }
}
