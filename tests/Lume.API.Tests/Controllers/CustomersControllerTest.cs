using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Lume.Application.Customers.Dtos;
using Lume.Controllers;
using Lume.Domain.Repositories;
using Lume.Infrastructure.Persistence.Seeders;
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
    
}