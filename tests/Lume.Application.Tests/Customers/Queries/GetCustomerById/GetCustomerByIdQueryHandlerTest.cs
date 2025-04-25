using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Lume.Application.Customers.Dtos;
using Lume.Application.Customers.Queries.GetCustomerById;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Lume.Application.Tests.Customers.Queries.GetCustomerById;

[TestSubject(typeof(GetCustomerByIdQueryHandler))]
public class GetCustomerByIdQueryHandlerTest
{

    [Fact]
    public async Task Handle_WhenIdIsValid_ReturnCustomerDto()
    {
        // arrange
        var loggerMock = new Mock<ILogger<GetCustomerByIdQueryHandler>>();
        var mapperMock = new Mock<IMapper>();
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        
        Guid customerId = Guid.NewGuid(); 
        string phoneNumber = "+1234567890123";
        var query = new GetCustomerByIdQuery(customerId);

        var customer = new ApplicationUser
        {
            Id = customerId,
            PhoneNumber = phoneNumber,
            UserType = "Customer"
        };
        var expectedCustomerDto = new CustomerDto
        {
            PhoneNumber = phoneNumber
        };
        
        mapperMock.Setup(m => m.Map<CustomerDto>(customer)).Returns(expectedCustomerDto);
        customerRepositoryMock.Setup(r => r.GetCustomerById(customerId)).ReturnsAsync(customer);
        
        var handler = new GetCustomerByIdQueryHandler(loggerMock.Object, mapperMock.Object, customerRepositoryMock.Object);
        
        // act
        var result = await handler.Handle(query, CancellationToken.None);
        
        // assert
        result.Should().BeEquivalentTo(expectedCustomerDto);
        customerRepositoryMock.Verify(r => r.GetCustomerById(customerId), Times.Once);
    }
    [Fact]
    public async Task Handle_WhenIdIsInValid_ReturnNullCustomerDto()
    {
        // arrange
        var loggerMock = new Mock<ILogger<GetCustomerByIdQueryHandler>>();
        var mapperMock = new Mock<IMapper>();
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        
        Guid customerId = Guid.NewGuid(); 
        var query = new GetCustomerByIdQuery(customerId);
        
        customerRepositoryMock.Setup(r => r.GetCustomerById(customerId)).ReturnsAsync((ApplicationUser)null);
        
        var handler = new GetCustomerByIdQueryHandler(loggerMock.Object, mapperMock.Object, customerRepositoryMock.Object);
        
        // act
        var result = await handler.Handle(query, CancellationToken.None);
        
        // assert
        result.Should().BeNull();
        customerRepositoryMock.Verify(r => r.GetCustomerById(customerId), Times.Once);
    }
}