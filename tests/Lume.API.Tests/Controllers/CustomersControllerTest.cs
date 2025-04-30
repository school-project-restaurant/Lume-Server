using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Lume.Application.Customers.Commands.CreateCustomer;
using Lume.Application.Customers.Commands.UpdateCustomer;
using Lume.Application.Customers.Dtos;
using Lume.Controllers;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Lume.Infrastructure.Persistence.Seeders;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Xunit;

namespace Lume.API.Tests.Controllers;

[TestSubject(typeof(CustomersController))]
public class CustomersControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();
    private readonly Mock<ISeederOrchestrator> _seederOrchestratorMock = new();

    public CustomersControllerTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(ICustomerRepository),
                    _ => _customerRepositoryMock.Object));
                services.Replace(ServiceDescriptor.Singleton(typeof(ISeederOrchestrator),
                    _ => _seederOrchestratorMock.Object));
            });
        });
    }
    [Fact]
    public async Task GetAllCustomers_WhenRequestIsValid_Returns200Ok()
    {
        // arrange
        var client = _factory.CreateClient();
        
        // act
        var result = await client.GetAsync("/api/Customers");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task GetCustomerById_WhenRequestIsValid_Returns200Ok()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var customer = new ApplicationUser
        {
            Id = id,
            Name = "Test Name",
            Surname = "Test Surname",
            UserType = "Customer"
        };
        _customerRepositoryMock.Setup(r => r.GetCustomerById(id)).ReturnsAsync(customer);
        
        // act
        var result = await client.GetAsync($"/api/Customers/{id}");
        var customerDto = await result.Content.ReadFromJsonAsync<CustomerDto>();
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        customerDto.Should().NotBeNull();
        customerDto.Name.Should().Be("Test Name");
        customerDto.Surname.Should().Be("Test Surname");
    }
    
    [Fact]
    public async Task GetCustomerById_WhenCustomerDoesNotExist_Returns404NotFound()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        _customerRepositoryMock.Setup(r => r.GetCustomerById(id)).ReturnsAsync((ApplicationUser)null);
        
        // act
        var result = await client.GetAsync($"/api/Customers/{id}");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_WhenCustomerDoesExist_Returns204NoContent()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var customer = new ApplicationUser
        {
            Id = id,
            Name = "Test Name",
            Surname = "Test Surname",
            UserType = "Customer"
        };
        _customerRepositoryMock.Setup(r => r.GetCustomerById(id)).ReturnsAsync(customer);
        _customerRepositoryMock.Setup(r => r.DeleteCustomer(customer));
        
        // act
        var result = await client.DeleteAsync($"/api/Customers/{id}");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        _customerRepositoryMock.Verify(r => r.DeleteCustomer(customer), Times.Once);
    }
    [Fact]
    public async Task Delete_WhenCustomerDoesNotExist_Returns404NotFound()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        _customerRepositoryMock.Setup(r => r.GetCustomerById(id)).ReturnsAsync((ApplicationUser)null);
        
        // act
        var result = await client.DeleteAsync($"/api/Customers/{id}");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        _customerRepositoryMock.Verify(r => r.DeleteCustomer(It.IsAny<ApplicationUser>()), Times.Never);
    }

    [Fact]
    public async Task UpdateCustomer_WhenCustomerDoesExist_Returns204NoContent()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var request = new UpdateCustomerCommand
        {
            Id = id,
            Name = "New Name",
            Surname = "New Surname",
            PhoneNumber = "+1234567890123"
        };
        var customer = new ApplicationUser
        {
            Id = id,
            Name = "Test Name",
            Surname = "Test Surname",
            PhoneNumber = "+1234567890321",
            UserType = "Customer"
        };
        _customerRepositoryMock.Setup(r => r.GetCustomerById(id)).ReturnsAsync(customer);
        
        // act
        var result = await client.PatchAsync($"/api/Customers/{id}", JsonContent.Create(request));
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        customer.Name.Should().Be("New Name");
        customer.Surname.Should().Be("New Surname");
        customer.PhoneNumber.Should().Be("+1234567890123");
        _customerRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
    }
    
    [Fact]
    public async Task UpdateCustomer_WithInvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var request = new UpdateCustomerCommand
        {
            Id = id,
            Name = "",
            Surname = "",
            PhoneNumber = "",
        };

        // Act
        var result = await client.PatchAsJsonAsync($"/api/Customers/{id}", request);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _customerRepositoryMock.Verify(r => r.SaveChanges(), Times.Never);
    }
    
    [Fact]
    public async Task UpdateCustomer_WhenCustomerDoesExist_Returns404NotFound()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var request = new UpdateCustomerCommand
        {
            Id = id,
            Name = "Name",
            Surname = "Surname",
            PhoneNumber = "+1234567890123"
        };
        _customerRepositoryMock.Setup(r => r.GetCustomerById(id)).ReturnsAsync((ApplicationUser)null);
        
        // act
        var result = await client.PatchAsJsonAsync($"/api/Customers/{id}", request);
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        _customerRepositoryMock.Verify(r => r.SaveChanges(), Times.Never);
    }
    
    [Fact]
    public async Task Post_WhenRequestIsValid_Returns201Created()
    {
        // arrange
        var client = _factory.CreateClient();
        var expectedId = Guid.NewGuid();
        var request = new CreateCustomerCommand
        {
            Email = "r@example.com",
            Name = "Random Name",
            Surname = "Random Surname",
            PasswordHash = "Password123!",
            PhoneNumber = "+1234567890123"
        };
        var customer = new ApplicationUser
        {
            Id = expectedId,
            Email = "r@example.com",
            Name = "Random Name",
            Surname = "Random Surname",
            PasswordHash = "Password123!",
            PhoneNumber = "+1234567890123",
            UserType = "Customer"
        };
        _customerRepositoryMock.Setup(r => r.CreateCustomer(customer))
            .ReturnsAsync(expectedId);
        
        // act
        var result = await client.PostAsync("/api/Customers", JsonContent.Create(request));
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Headers.Location.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Post_WithInvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new CreateCustomerCommand()
        {
            Name = "",
            Surname = "",
            PasswordHash = "",
            PhoneNumber = "",
        };

        // Act
        var result = await client.PostAsJsonAsync("/api/Customers", request);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}