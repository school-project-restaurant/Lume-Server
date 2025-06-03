using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Lume.Application.Customers.Commands.UpdateCustomer;
using Lume.Domain.Entities;
using Lume.Domain.Exceptions;
using Lume.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Lume.Application.Tests.Customers.Commands.UpdateCustomer;

[TestSubject(typeof(UpdateCustomerCommandHandler))]
public class UpdateCustomerCommandHandlerTest
{
    [Fact]
    public async Task Handle_WhenRequestIsValid_ShouldUpdateCustomer()
    {
        // arrange
        var loggerMock = new Mock<ILogger<UpdateCustomerCommandHandler>>();
        var mapperMock = new Mock<IMapper>();
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        
        Guid customerId = Guid.NewGuid();
        var request = new UpdateCustomerCommand
        {
            Id = customerId,
            Email = "rand@example.com",
            Name = "Random",
            Surname = "Random",
            PhoneNumber = "+1234567890122"
        };

        var customer = new ApplicationUser
        {
            Id = customerId,
            Email = "random@example.com",
            Name = "Random Name",
            Surname = "Random Surname",
            PhoneNumber = "+1234567890123",
            UserType = "Customer"
        };
        customerRepositoryMock.Setup(r => r.GetCustomerById(customerId)).ReturnsAsync(customer);
        mapperMock.Setup(m => m.Map(request, customer)).Callback<UpdateCustomerCommand, ApplicationUser>
            ((src, dest) =>
            {
                dest.Email = src.Email;
                dest.Name = src.Name;
                dest.Surname = src.Surname;
                dest.PhoneNumber = src.PhoneNumber;
            });
        
        var handler =
            new UpdateCustomerCommandHandler(loggerMock.Object, mapperMock.Object, customerRepositoryMock.Object);
        
        // act
        await handler.Handle(request, CancellationToken.None);
        
        // assert
        mapperMock.Verify(m => m.Map(request, customer), Times.Once);
        customerRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        
        customer.Email.Should().Be(request.Email);
        customer.Name.Should().Be(request.Name);
        customer.Surname.Should().Be(request.Surname);
        customer.PhoneNumber.Should().Be(request.PhoneNumber);
    }
    
    [Fact]
    public async Task Handle_WhenCustomerDoesNotExist_ShouldThrowNotFoundException()
    {
        // arrange
        var loggerMock = new Mock<ILogger<UpdateCustomerCommandHandler>>();
        var mapperMock = new Mock<IMapper>();
        var customerRepositoryMock = new Mock<ICustomerRepository>();
        
        Guid customerId = Guid.NewGuid();
        var request = new UpdateCustomerCommand
        {
            Id = customerId
        };

        customerRepositoryMock.Setup(r => r.GetCustomerById(customerId))
            .ReturnsAsync((ApplicationUser)null);
        
        var handler =
            new UpdateCustomerCommandHandler(loggerMock.Object, mapperMock.Object, customerRepositoryMock.Object);
        
        // act
        Func<Task> act = async () => await handler.Handle(request, CancellationToken.None);
        
        // assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Entity \"customer\" ({customerId}) was not found.");
        customerRepositoryMock.Verify(r => r.SaveChanges(), Times.Never);
        mapperMock.Verify(m => m.Map(It.IsAny<UpdateCustomerCommand>(), It.IsAny<ApplicationUser>()), Times.Never);
    }
}