using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Lume.Application.Customers.Commands.CreateCustomer;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Lume.Application.Tests.Customers.Commands.CreateCustomer;

[TestSubject(typeof(CreateCustomerCommandHandler))]
public class CreateCustomerCommandHandlerTest
{

    [Fact]
    public async Task Handle_WhenRequestIsValid_ReturnIdOfCreatedCustomer()
    {
        // arrange
        var loggerMock = new Mock<ILogger<CreateCustomerCommandHandler>>();
        var mapperMock = new Mock<IMapper>();
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        
        var request = new CreateCustomerCommand();
        var customer = new ApplicationUser();
        
        Guid customerId = Guid.NewGuid();
        mapperMock.Setup(m => m.Map<ApplicationUser>(request)).Returns(customer);
        
        customerRepositoryMock.Setup(r => r.CreateCustomer(It.IsAny<ApplicationUser>())).ReturnsAsync(customerId);
        var handler = new CreateCustomerCommandHandler(loggerMock.Object, mapperMock.Object, customerRepositoryMock.Object);
        
        // act
        var result = await handler.Handle(request, CancellationToken.None);
        
        // assert
        result.Should().Be(customerId);
        mapperMock.Verify(m => m.Map<ApplicationUser>(request), Times.Once);
        customerRepositoryMock.Verify(r => r.CreateCustomer(customer), Times.Once);
    }
}