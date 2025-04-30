using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Lume.Application.Customers.Commands.CreateCustomer;
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
        _customerRepositoryMock.Setup(r => r.CreateCustomer(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(expectedId);
        
        // act
        var result = await client.PostAsync("/api/Customers", JsonContent.Create(request));
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Headers.Location.Should().NotBeNull();
        result.Headers.Location!.ToString().Should().EndWith($"/api/Customers/{expectedId}");
    }
    [Fact]
    public async Task Post_WhenRequestIsInValid_Returns400BadRequest()
    {
        // arrange
        var client = _factory.CreateClient();
        var expectedId = Guid.NewGuid();
        var request = new CreateCustomerCommand
        {
            Email = "r.example.com",
            Name = "R",
            Surname = "R",
            PasswordHash = "a",
            PhoneNumber = "+123"
        };
        var user = new ApplicationUser
        {
            Id = expectedId,
            Email = request.Email,
            Name = request.Name,
            Surname = request.Surname,
            PasswordHash = request.PasswordHash,
            PhoneNumber = request.PhoneNumber
        };
        _customerRepositoryMock.Setup(r => r.CreateCustomer(user))
            .ReturnsAsync(expectedId);
        
        // act
        var result = await client.PostAsync("/api/Customers", JsonContent.Create(request));
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}