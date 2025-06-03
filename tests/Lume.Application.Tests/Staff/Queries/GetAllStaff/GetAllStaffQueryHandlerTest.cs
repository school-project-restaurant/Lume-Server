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
    public async Task Handle_WhenRequestIsValid_ReturnPagedStaffDtos()
    {
        // arrange
        var loggerMock = new Mock<ILogger<GetAllStaffQueryHandler>>();
        var mapperMock = new Mock<IMapper>();
        var staffRepositoryMock = new Mock<IStaffRepository>();
        var query = new GetAllStaffQuery { PageSize = 10, PageIndex = 0 };

        var staffFilterOptions = new StaffFilterOptions();
        var staffSortOptions = new StaffSortOptions();
        
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
        
        var totalCount = 2;
        
        mapperMock.Setup(m => m.Map<StaffFilterOptions>(query))
            .Returns(staffFilterOptions);
            
        mapperMock.Setup(m => m.Map<StaffSortOptions>(query))
            .Returns(staffSortOptions);
        
        staffRepositoryMock.Setup(r => r.GetMatchingStaff(staffFilterOptions, staffSortOptions))
            .ReturnsAsync((staffs, totalCount));
        
        mapperMock.Setup(m => m.Map<IEnumerable<StaffDto>>(staffs))
            .Returns(expectedDtos);

        var queryHandler = new GetAllStaffQueryHandler(loggerMock.Object, 
            mapperMock.Object, staffRepositoryMock.Object);
        
        // act
        var result = await queryHandler.Handle(query, CancellationToken.None);
        
        // assert
        result.Items.Should().BeEquivalentTo(expectedDtos);
        result.TotalItemsCount.Should().Be(totalCount);
        staffRepositoryMock.Verify(r => r.GetMatchingStaff(staffFilterOptions, staffSortOptions), Times.Once);
    }
    
    [Fact]
    public async Task Handle_WhenNoStaffExist_ReturnEmptyPagedResult()
    {
        // arrange
        var loggerMock = new Mock<ILogger<GetAllStaffQueryHandler>>();
        var mapperMock = new Mock<IMapper>();
        var staffRepositoryMock = new Mock<IStaffRepository>();
        var query = new GetAllStaffQuery { PageSize = 10, PageIndex = 0 };
    
        var staffFilterOptions = new StaffFilterOptions();
        var staffSortOptions = new StaffSortOptions();
        
        var emptyUsersList = new List<ApplicationUser>();
        var emptyDtosList = new List<StaffDto>();
        var totalCount = 0;

        mapperMock.Setup(m => m.Map<StaffFilterOptions>(query))
            .Returns(staffFilterOptions);
            
        mapperMock.Setup(m => m.Map<StaffSortOptions>(query))
            .Returns(staffSortOptions);
        
        staffRepositoryMock.Setup(r => r.GetMatchingStaff(staffFilterOptions, staffSortOptions))
            .ReturnsAsync((emptyUsersList, totalCount));

        mapperMock.Setup(m => m.Map<IEnumerable<StaffDto>>(emptyUsersList))
            .Returns(emptyDtosList);

        var queryHandler = new GetAllStaffQueryHandler(loggerMock.Object,
            mapperMock.Object, staffRepositoryMock.Object);
    
        // act
        var result = await queryHandler.Handle(query, CancellationToken.None);

        // assert
        result.Items.Should().BeEmpty();
        result.TotalItemsCount.Should().Be(0);
        staffRepositoryMock.Verify(r => r.GetMatchingStaff(staffFilterOptions, staffSortOptions), Times.Once);
    }
}

