using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Lume.Application.Tables.Commands.CreateTable;
using Lume.Application.Tables.Commands.UpdateTable;
using Lume.Application.Tables.Dtos;
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

[TestSubject(typeof(TablesController))]
public class TablesControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<ITablesRepository> _tableRepositoryMock = new();
    private readonly Mock<ISeederOrchestrator> _seederOrchestratorMock = new();

    public TablesControllerTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(ITablesRepository),
                    _ => _tableRepositoryMock.Object));
                services.Replace(ServiceDescriptor.Singleton(typeof(ISeederOrchestrator),
                    _ => _seederOrchestratorMock.Object));
            });
        });
    }

    [Fact]
    public async Task GetAllTables_WhenRequestIsValid_Returns200Ok()
    {
        // arrange
        var client = _factory.CreateClient();
        var tables = new List<Table>
        {
            new()
            {
                Number = 1,
                Seats = 4,
                ReservationsId = new List<string>()
            },
            new()
            {
                Number = 2,
                Seats = 6,
                ReservationsId = new List<string>()
            }
        };
        
        _tableRepositoryMock.Setup(r => r.GetAllTables())
            .ReturnsAsync(tables);
        
        // act
        var result = await client.GetAsync("/api/Tables");
        var tablesDtos = await result.Content.ReadFromJsonAsync<List<TablesDto>>();
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        tablesDtos.Should().NotBeNull();
        tablesDtos.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task GetTableByNumber_WhenTableExists_Returns200Ok()
    {
        // arrange
        var client = _factory.CreateClient();
        var tableNumber = 5;
        var table = new Table
        {
            Number = tableNumber,
            Seats = 8,
            ReservationsId = new List<string>()
        };
        
        _tableRepositoryMock.Setup(r => r.GetTableByNumber(tableNumber))
            .ReturnsAsync(table);
        
        // act
        var result = await client.GetAsync($"/api/Tables/{tableNumber}");
        var tableDto = await result.Content.ReadFromJsonAsync<TablesDto>();
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        tableDto.Should().NotBeNull();
        tableDto.Number.Should().Be(tableNumber);
        tableDto.Seats.Should().Be(8);
    }
    
    [Fact]
    public async Task GetTableByNumber_WhenTableDoesNotExist_Returns404NotFound()
    {
        // arrange
        var client = _factory.CreateClient();
        var tableNumber = 10;
        
        _tableRepositoryMock.Setup(r => r.GetTableByNumber(tableNumber))
            .ReturnsAsync((Table)null);
        
        // act
        var result = await client.GetAsync($"/api/Tables/{tableNumber}");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateTable_WhenRequestIsValid_Returns201Created()
    {
        // arrange
        var client = _factory.CreateClient();
        var tableNumber = 3;
        var request = new CreateTableCommand
        {
            Number = tableNumber,
            Seats = 4
        };
        var table = new Table
        {
            Number = tableNumber,
            Seats = 4
        };
        
        _tableRepositoryMock.Setup(r => r.CreateTable(table))
            .ReturnsAsync(tableNumber);
        
        // act
        var result = await client.PostAsJsonAsync("/api/Tables", request);
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Headers.Location.Should().NotBeNull();
        _tableRepositoryMock.Verify(r => r.CreateTable(It.IsAny<Table>()), Times.Once);
    }
    
    [Fact]
    public async Task CreateTable_WithInvalidModel_ReturnsBadRequest()
    {
        // arrange
        var client = _factory.CreateClient();
        var request = new CreateTableCommand
        {
            Number = -1, // Invalid table number
            Seats = 0 // Invalid seat count
        };
        
        // act
        var result = await client.PostAsJsonAsync("/api/Tables", request);
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _tableRepositoryMock.Verify(r => r.CreateTable(It.IsAny<Table>()), Times.Never);
    }
    
    [Fact]
    public async Task UpdateTable_WhenTableExists_Returns200Ok()
    {
        // arrange
        var client = _factory.CreateClient();
        var tableNumber = 5;
        var request = new UpdateTableCommand
        {
            Number = tableNumber,
            Seats = 10
        };
        var table = new Table
        {
            Number = tableNumber,
            Seats = 10
        };

        _tableRepositoryMock.Setup(r => r.GetTableByNumber(tableNumber)).ReturnsAsync(table);
        
        // act
        var result = await client.PutAsJsonAsync("/api/Tables", request);
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        _tableRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
    }
    
    [Fact]
    public async Task UpdateTable_WhenTableDoesNotExist_Returns404NotFound()
    {
        // arrange
        var client = _factory.CreateClient();
        var request = new UpdateTableCommand
        {
            Number = 99,
            Seats = 8
        };
        
        // act
        var result = await client.PutAsJsonAsync("/api/Tables", request);
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        _tableRepositoryMock.Verify(r => r.SaveChanges(), Times.Never);
    }
    
    [Fact]
    public async Task DeleteTable_WhenTableExists_Returns200Ok()
    {
        // arrange
        var client = _factory.CreateClient();
        var tableNumber = 7;
        var table = new Table
        {
            Number = tableNumber,
            ReservationsId = ["123", "123"],
            Seats = 5
        };

        _tableRepositoryMock.Setup(r => r.GetTableByNumber(tableNumber)).ReturnsAsync(table);
        _tableRepositoryMock.Setup(r => r.DeleteTable(table));
        
        // act
        var result = await client.DeleteAsync($"/api/Tables/{tableNumber}");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        _tableRepositoryMock.Verify(r => r.DeleteTable(table), Times.Once);
    }
    
    [Fact]
    public async Task DeleteTable_WhenTableDoesNotExist_Returns404NotFound()
    {
        // arrange
        var client = _factory.CreateClient();
        var tableNumber = 20;
        var table = new Table
        {
            Number = 5,
            ReservationsId = ["123", "123"],
            Seats = 5
        };

        _tableRepositoryMock.Setup(r => r.GetTableByNumber(tableNumber));
        
        // act
        var result = await client.DeleteAsync($"/api/Tables/{tableNumber}");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        _tableRepositoryMock.Verify(r => r.DeleteTable(table), Times.Never);
    }
}
