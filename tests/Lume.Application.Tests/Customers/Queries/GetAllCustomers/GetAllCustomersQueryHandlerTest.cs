using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Lume.Application.Customers.Dtos;
using Lume.Application.Customers.Queries.GetAllCustomers;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Lume.Application.Tests.Customers.Queries.GetAllCustomers;

[TestSubject(typeof(GetAllCustomersQueryHandler))]
public class GetAllCustomersQueryHandlerTest
{

    [Fact]
    public async Task Handle_WhenRequestIsValid_ReturnCustomerDtos()
    {
        // arrange
        var loggerMock = new Mock<ILogger<GetAllCustomersQueryHandler>>();
        var mapperMock = new Mock<IMapper>();
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        var query = new GetAllCustomersQuery();

        var customers = new List<ApplicationUser>()
        {
            new ApplicationUser(),
            new ApplicationUser()
        };
        
        var expectedDtos = new List<CustomerDto>
        {
            new CustomerDto { },
            new CustomerDto { }
        };
        
        customerRepositoryMock.Setup(r => r.GetAllCustomers())
            .ReturnsAsync(customers);
        
        mapperMock.Setup(m => m.Map<IEnumerable<CustomerDto>>(customers))
            .Returns(expectedDtos);

        var queryHandler = new GetAllCustomersQueryHandler(loggerMock.Object, 
            mapperMock.Object, customerRepositoryMock.Object);
        
        // act
        var result = await queryHandler.Handle(query, CancellationToken.None);
        
        // assert
        result.Should().BeEquivalentTo(expectedDtos);
        customerRepositoryMock.Verify(r => r.GetAllCustomers(), Times.Once);
    }
    
    [Fact]
    public async Task Handle_WhenNoCustomersExist_ReturnEmptyList()
    {
        // arrange
        var loggerMock = new Mock<ILogger<GetAllCustomersQueryHandler>>();
        var mapperMock = new Mock<IMapper>();
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        var query = new GetAllCustomersQuery();
    
        var emptyUsersList = new List<ApplicationUser>();
        var emptyDtosList = new List<CustomerDto>();

        customerRepositoryMock.Setup(r => r.GetAllCustomers())
            .ReturnsAsync(emptyUsersList);

        mapperMock.Setup(m => m.Map<IEnumerable<CustomerDto>>(emptyUsersList))
            .Returns(emptyDtosList);

        var queryHandler = new GetAllCustomersQueryHandler(loggerMock.Object,
            mapperMock.Object, customerRepositoryMock.Object);
    
        // act
        var result = await queryHandler.Handle(query, CancellationToken.None);

        // assert
        result.Should().BeEmpty();
        customerRepositoryMock.Verify(r => r.GetAllCustomers(), Times.Once);
    }
}