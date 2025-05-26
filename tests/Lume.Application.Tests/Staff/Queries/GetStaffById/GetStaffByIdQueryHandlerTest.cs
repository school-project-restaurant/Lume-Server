using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Lume.Application.Staff.Dtos;
using Lume.Application.Staff.Queries.GetStaffById;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Lume.Application.Tests.Staff.Queries.GetStaffById;

[TestSubject(typeof(GetStaffByIdQueryHandler))]
public class GetStaffByIdQueryHandlerTest
{
    [Fact]
    public async Task Handle_WhenIdIsValid_ReturnStaffDto()
    {
        // arrange
        var loggerMock = new Mock<ILogger<GetStaffByIdQueryHandler>>();
        var mapperMock = new Mock<IMapper>();
        var staffRepositoryMock = new Mock<IStaffRepository>();
        
        Guid staffId = Guid.NewGuid();
        string phoneNumber = "+1234567890123";
        var query = new GetStaffByIdQuery(staffId);

        var staffMember = new ApplicationUser
        {
            Id = staffId,
            PhoneNumber = phoneNumber,
            UserType = "Staff"
        };
        var expectedStaffDto = new StaffDto
        {
            PhoneNumber = phoneNumber
        };
        
        mapperMock.Setup(m => m.Map<StaffDto>(staffMember)).Returns(expectedStaffDto);
        staffRepositoryMock.Setup(r => r.GetStaffById(staffId)).ReturnsAsync(staffMember);
        
        var handler = new GetStaffByIdQueryHandler(loggerMock.Object, mapperMock.Object, staffRepositoryMock.Object);
        
        // act
        var result = await handler.Handle(query, CancellationToken.None);
        
        // assert
        result.Should().BeEquivalentTo(expectedStaffDto);
        staffRepositoryMock.Verify(r => r.GetStaffById(staffId), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenIdIsInvalid_ReturnNullStaffDto()
    {
        // arrange
        var loggerMock = new Mock<ILogger<GetStaffByIdQueryHandler>>();
        var mapperMock = new Mock<IMapper>();
        var staffRepositoryMock = new Mock<IStaffRepository>();
        
        Guid staffId = Guid.NewGuid();
        var query = new GetStaffByIdQuery(staffId);
        
        staffRepositoryMock.Setup(r => r.GetStaffById(staffId)).ReturnsAsync((ApplicationUser)null);
        
        var handler = new GetStaffByIdQueryHandler(loggerMock.Object, mapperMock.Object, staffRepositoryMock.Object);
        
        // act
        var result = await handler.Handle(query, CancellationToken.None);
        
        // assert
        result.Should().BeNull();
        staffRepositoryMock.Verify(r => r.GetStaffById(staffId), Times.Once);
    }
}