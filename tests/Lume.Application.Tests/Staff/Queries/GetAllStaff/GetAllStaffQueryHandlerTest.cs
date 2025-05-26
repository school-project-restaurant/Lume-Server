using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Lume.Application.Staff.Dtos;
using Lume.Application.Staff.Queries.GetAllStaff;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Lume.Application.Tests.Staff.Queries.GetAllStaff;

[TestSubject(typeof(GetAllStaffQueryHandler))]
public class GetAllStaffQueryHandlerTest
{

    [Fact]
    public async Task Handle_WhenRequestIsValid_ReturnStaffDtos()
    {
        // arrange
        var loggerMock = new Mock<ILogger<GetAllStaffQueryHandler>>();
        var mapperMock = new Mock<IMapper>();
        var staffRepositoryMock = new Mock<IStaffRepository>();
        var query = new GetAllStaffQuery();

        var staffs = new List<ApplicationUser>()
        {
            new ApplicationUser(),
            new ApplicationUser()
        };
        
        var expectedDtos = new List<StaffDto>
        {
            new StaffDto { },
            new StaffDto { }
        };
        
        staffRepositoryMock.Setup(r => r.GetAllStaff())
            .ReturnsAsync(staffs);
        
        mapperMock.Setup(m => m.Map<IEnumerable<StaffDto>>(staffs))
            .Returns(expectedDtos);

        var queryHandler = new GetAllStaffQueryHandler(loggerMock.Object, 
            mapperMock.Object, staffRepositoryMock.Object);
        
        // act
        var result = await queryHandler.Handle(query, CancellationToken.None);
        
        // assert
        result.Should().BeEquivalentTo(expectedDtos);
        staffRepositoryMock.Verify(r => r.GetAllStaff(), Times.Once);
    }
    
    [Fact]
    public async Task Handle_WhenNoStaffExist_ReturnEmptyList()
    {
        // arrange
        var loggerMock = new Mock<ILogger<GetAllStaffQueryHandler>>();
        var mapperMock = new Mock<IMapper>();
        var staffRepositoryMock = new Mock<IStaffRepository>();
        var query = new GetAllStaffQuery();
    
        var emptyUsersList = new List<ApplicationUser>();
        var emptyDtosList = new List<StaffDto>();

        staffRepositoryMock.Setup(r => r.GetAllStaff())
            .ReturnsAsync(emptyUsersList);

        mapperMock.Setup(m => m.Map<IEnumerable<StaffDto>>(emptyUsersList))
            .Returns(emptyDtosList);

        var queryHandler = new GetAllStaffQueryHandler(loggerMock.Object,
            mapperMock.Object, staffRepositoryMock.Object);
    
        // act
        var result = await queryHandler.Handle(query, CancellationToken.None);

        // assert
        result.Should().BeEmpty();
        staffRepositoryMock.Verify(r => r.GetAllStaff(), Times.Once);
    }
}