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
    public async Task Handle_WhenRequestIsValid_ReturnRestaurantDtos()
    {
        // arrange
        var loggerMock = new Mock<ILogger<GetAllCustomersQueryHandler>>();
        var mapperMock = new Mock<IMapper>();
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        var query = new GetAllCustomersQuery();
        
        var expectedDtos = new List<CustomerDto>
        {
            new CustomerDto { },
            new CustomerDto { }
        };
        
        customerRepositoryMock.Setup(r => r.GetAllCustomers())
            .ReturnsAsync(It.IsAny<IEnumerable<ApplicationUser>>());
        
        mapperMock.Setup(m => m.Map<IEnumerable<CustomerDto>>(It.IsAny<IEnumerable<ApplicationUser>>()))
            .Returns(expectedDtos);

        var queryHandler = new GetAllCustomersQueryHandler(loggerMock.Object, 
            mapperMock.Object, customerRepositoryMock.Object);
        
        // act
        var result = await queryHandler.Handle(query, CancellationToken.None);
        
        // assert
        result.Should().BeEquivalentTo(expectedDtos);
        customerRepositoryMock.Verify(r => r.GetAllCustomers(), Times.Once);
    }
}