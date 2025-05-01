using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Lume.Application.Staff.Commands.UpdateStaff;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Lume.Application.Tests.Staff.Commands.UpdateStaff;

[TestSubject(typeof(UpdateStaffCommandHandler))]
public class UpdateStaffCommandHandlerTest
{

    [Fact]
    public async Task Handle_WhenRequestIsValid_ShouldReturnTrue()
    {
        // arrange
        var loggerMock = new Mock<ILogger<UpdateStaffCommandHandler>>();
        var mapperMock = new Mock<IMapper>();
        var staffRepositoryMock = new Mock<IStaffRepository>();
        
        Guid staffId = Guid.NewGuid();
        var request = new UpdateStaffCommand
        {
            Id = staffId,
            Name = "Random",
            Surname = "Random",
            PhoneNumber = "+1234567890122"
        };

        var staff = new ApplicationUser
        {
            Id = staffId,
            Name = "Random Name",
            Surname = "Random Surname",
            PhoneNumber = "+1234567890123",
            UserType = "Staff"
        };
        staffRepositoryMock.Setup(r => r.GetStaffById(staffId)).ReturnsAsync(staff);
        mapperMock.Setup(m => m.Map(request, staff)).Callback<UpdateStaffCommand, ApplicationUser>
            ((src, dest) =>
            {
                dest.Name = src.Name;
                dest.Surname = src.Surname;
                dest.PhoneNumber = src.PhoneNumber;
            });
        
        var handler =
            new UpdateStaffCommandHandler(loggerMock.Object, mapperMock.Object, staffRepositoryMock.Object);
        
        // act
        var result = await handler.Handle(request, CancellationToken.None);
        
        // assert
        result.Should().Be(true);
        mapperMock.Verify(m => m.Map(request, staff), Times.Once);
        staffRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        
        staff.Name.Should().Be(request.Name);
        staff.Surname.Should().Be(request.Surname);
        staff.PhoneNumber.Should().Be(request.PhoneNumber);
    }
    
    [Fact]
    public async Task Handle_WhenStaffDoesNotExist_ShouldReturnFalse()
    {
        // arrange
        var loggerMock = new Mock<ILogger<UpdateStaffCommandHandler>>();
        var mapperMock = new Mock<IMapper>();
        var staffRepositoryMock = new Mock<IStaffRepository>();
        
        Guid staffId = Guid.NewGuid();
        var request = new UpdateStaffCommand
        {
            Id = staffId
        };

        staffRepositoryMock.Setup(r => r.GetStaffById(staffId))
            .ReturnsAsync((ApplicationUser)null);
        
        var handler =
            new UpdateStaffCommandHandler(loggerMock.Object, mapperMock.Object, staffRepositoryMock.Object);
        
        // act
        var result = await handler.Handle(request, CancellationToken.None);
        
        // assert
        result.Should().Be(false);
        staffRepositoryMock.Verify(r => r.SaveChanges(), Times.Never);
    }
}
