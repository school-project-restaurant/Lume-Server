using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Lume.Application.Staff.Commands.CreateStaff;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Lume.Application.Tests.Staff.Commands.CreateStaff;

[TestSubject(typeof(CreateStaffCommandHandler))]
public class CreateStaffCommandHandlerTest
{

    [Fact]
    public async Task Handle_WhenRequestIsValid_ReturnIdOfCreatedStaff()
    {
        // arrange
        var loggerMock = new Mock<ILogger<CreateStaffCommandHandler>>();
        var mapperMock = new Mock<IMapper>();
        var staffRepositoryMock = new Mock<IStaffRepository>();
        
        var request = new CreateStaffCommand();
        var staff = new ApplicationUser();
        
        Guid staffId = Guid.NewGuid();
        mapperMock.Setup(m => m.Map<ApplicationUser>(request)).Returns(staff);
        
        staffRepositoryMock.Setup(r => r.CreateStaff(It.IsAny<ApplicationUser>())).ReturnsAsync(staffId);
        var handler = new CreateStaffCommandHandler(loggerMock.Object, mapperMock.Object, staffRepositoryMock.Object);
        
        // act
        var result = await handler.Handle(request, CancellationToken.None);
        
        // assert
        result.Should().Be(staffId);
        mapperMock.Verify(m => m.Map<ApplicationUser>(request), Times.Once);
        staffRepositoryMock.Verify(r => r.CreateStaff(staff), Times.Once);
    }
}
