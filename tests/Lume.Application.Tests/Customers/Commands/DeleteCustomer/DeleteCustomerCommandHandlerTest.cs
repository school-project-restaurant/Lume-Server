using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Lume.Application.Customers.Commands.DeleteCustomer;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Lume.Application.Tests.Customers.Commands.DeleteCustomer;

[TestSubject(typeof(DeleteCustomerCommandHandler))]
public class DeleteCustomerCommandHandlerTest
{

    [Fact]
    public async Task Handle_WhenRequestIsValid_ShouldReturnTrue()
    {
        // arrange
        var loggerMock = new Mock<ILogger<DeleteCustomerCommandHandler>>();
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        var customerId = Guid.NewGuid();
        var request = new DeleteCustomerCommand(customerId);
        var customer = new ApplicationUser
        {
            Id = customerId
        };

        customerRepositoryMock.Setup(r => r.GetCustomerById(customerId)).ReturnsAsync(customer);
        customerRepositoryMock.Setup(r => r.DeleteCustomer(customer));

        var handler = new DeleteCustomerCommandHandler(loggerMock.Object, customerRepositoryMock.Object);
        
        // act
        var result = await handler.Handle(request, CancellationToken.None);
        
        // assert
        result.Should().Be(true);
        customerRepositoryMock.Verify(r => r.DeleteCustomer(customer), Times.Once);
    }
    
    [Fact]
    public async Task Handle_WhenCustomerDoesNotExist_ShouldReturnFalse()
    {
        // arrange
        var loggerMock = new Mock<ILogger<DeleteCustomerCommandHandler>>();
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        var customerId = Guid.NewGuid();
        var request = new DeleteCustomerCommand(customerId);
        var customer = new ApplicationUser();

        customerRepositoryMock.Setup(r => r.GetCustomerById(customerId)).ReturnsAsync((ApplicationUser)null);

        var handler = new DeleteCustomerCommandHandler(loggerMock.Object, customerRepositoryMock.Object);
        
        // act
        var result = await handler.Handle(request, CancellationToken.None);
        
        // assert
        result.Should().Be(false);
        customerRepositoryMock.Verify(r => r.DeleteCustomer(customer), Times.Never);
    }
}