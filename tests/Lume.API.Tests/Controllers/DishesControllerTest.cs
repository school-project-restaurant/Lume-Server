using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Lume.Application.Dishes.Commands.CreateDish;
using Lume.Application.Dishes.Commands.UpdateDish;
using Lume.Application.Dishes.Dtos;
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
using System.Collections.Generic;

namespace Lume.API.Tests.Controllers;

[TestSubject(typeof(DishesController))]
public class DishesControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IDishRepository> _dishRepositoryMock = new();
    private readonly Mock<ISeederOrchestrator> _seederOrchestratorMock = new();

    public DishesControllerTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IDishRepository),
                    _ => _dishRepositoryMock.Object));
                services.Replace(ServiceDescriptor.Singleton(typeof(ISeederOrchestrator),
                    _ => _seederOrchestratorMock.Object));
            });
        });
    }
    
    [Fact]
    public async Task GetAllDishes_WhenRequestIsValid_Returns200Ok()
    {
        // arrange
        var client = _factory.CreateClient();
        
        // act
        var result = await client.GetAsync("/api/Dishes");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task GetDishById_WhenRequestIsValid_Returns200Ok()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var dish = new Dish
        {
            Id = id,
            Price = 1000,
            Calories = 500,
            Ingredients = new List<string> { "Tomato", "Cheese", "Basil" }
        };
        _dishRepositoryMock.Setup(r => r.GetDishById(id)).ReturnsAsync(dish);
        
        // act
        var result = await client.GetAsync($"/api/Dishes/{id}");
        var dishDto = await result.Content.ReadFromJsonAsync<DishDto>();
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        dishDto.Should().NotBeNull();
        dishDto.Price.Should().Be(1000);
        dishDto.Calories.Should().Be(500);
        dishDto.Ingredients.Should().BeEquivalentTo(new List<string> { "Tomato", "Cheese", "Basil" });
    }
    
    [Fact]
    public async Task GetDishById_WhenDishDoesNotExist_Returns404NotFound()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        _dishRepositoryMock.Setup(r => r.GetDishById(id)).ReturnsAsync((Dish)null);
        
        // act
        var result = await client.GetAsync($"/api/Dishes/{id}");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteDish_WhenDishDoesExist_Returns204NoContent()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var dish = new Dish
        {
            Id = id,
            Price = 1000,
            Calories = 500,
            Ingredients = new List<string> { "Tomato", "Cheese", "Basil" }
        };
        _dishRepositoryMock.Setup(r => r.GetDishById(id)).ReturnsAsync(dish);
        _dishRepositoryMock.Setup(r => r.DeleteDish(dish));
        
        // act
        var result = await client.DeleteAsync($"/api/Dishes/{id}");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        _dishRepositoryMock.Verify(r => r.DeleteDish(dish), Times.Once);
    }
    
    [Fact]
    public async Task DeleteDish_WhenDishDoesNotExist_Returns404NotFound()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        _dishRepositoryMock.Setup(r => r.GetDishById(id)).ReturnsAsync((Dish)null);
        _dishRepositoryMock.Setup(r => r.DeleteDish(It.IsAny<Dish>()));
        
        // act
        var result = await client.DeleteAsync($"/api/Dishes/{id}");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        _dishRepositoryMock.Verify(r => r.DeleteDish(It.IsAny<Dish>()), Times.Never);
    }

    [Fact]
    public async Task UpdateDish_WhenDishDoesExist_Returns204NoContent()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var request = new UpdateDishCommand
        {
            Id = id,
            Price = 1200,
            Calories = 600,
            Ingredients = new List<string> { "Tomato", "Mozzarella", "Basil", "Olive Oil" }
        };
        var dish = new Dish
        {
            Id = id,
            Price = 1000,
            Calories = 500,
            Ingredients = new List<string> { "Tomato", "Cheese", "Basil" }
        };
        _dishRepositoryMock.Setup(r => r.GetDishById(id)).ReturnsAsync(dish);
        
        // act
        var result = await client.PatchAsJsonAsync($"/api/Dishes/{id}", request);
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        _dishRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
    }
    
    [Fact]
    public async Task UpdateDish_WithInvalidModel_ReturnsBadRequest()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var request = new UpdateDishCommand
        {
            Id = id,
            Price = -200, // Negative price should be invalid
            Calories = -100, // Negative calories should be invalid
            Ingredients = null
        };

        // act
        var result = await client.PatchAsJsonAsync($"/api/Dishes/{id}", request);

        // assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _dishRepositoryMock.Verify(r => r.SaveChanges(), Times.Never);
    }
    
    [Fact]
    public async Task UpdateDish_WhenDishDoesNotExist_Returns404NotFound()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var request = new UpdateDishCommand
        {
            Id = id,
            Price = 1200,
            Calories = 600,
            Ingredients = new List<string> { "Tomato", "Mozzarella", "Basil", "Olive Oil" }
        };
        _dishRepositoryMock.Setup(r => r.GetDishById(id)).ReturnsAsync((Dish)null);
        
        // act
        var result = await client.PatchAsJsonAsync($"/api/Dishes/{id}", request);
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        _dishRepositoryMock.Verify(r => r.SaveChanges(), Times.Never);
    }
    
    [Fact]
    public async Task CreateDish_WhenRequestIsValid_Returns201Created()
    {
        // arrange
        var client = _factory.CreateClient();
        var expectedId = Guid.NewGuid();
        var request = new CreateDishCommand
        {
            Price = 1000,
            Calories = 500,
            Ingredients = new List<string> { "Tomato", "Cheese", "Basil" }
        };
        
        _dishRepositoryMock.Setup(r => r.CreateDish(It.IsAny<Dish>()))
            .ReturnsAsync(expectedId);
        
        // act
        var result = await client.PostAsJsonAsync("/api/Dishes", request);
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Headers.Location.Should().NotBeNull();
        _dishRepositoryMock.Verify(r => r.CreateDish(It.IsAny<Dish>()), Times.Once);
    }
    
    [Fact]
    public async Task CreateDish_WithInvalidModel_ReturnsBadRequest()
    {
        // arrange
        var client = _factory.CreateClient();
        var request = new CreateDishCommand
        {
            Price = -100, // Negative price should be invalid
            Calories = -50, // Negative calories should be invalid
            Ingredients = null // Missing ingredients should be invalid
        };

        // act
        var result = await client.PostAsJsonAsync("/api/Dishes", request);

        // assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _dishRepositoryMock.Verify(r => r.CreateDish(It.IsAny<Dish>()), Times.Never);
    }
}
