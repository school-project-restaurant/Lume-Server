using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Lume.Application.Staff.Commands.CreateStaff;
using Lume.Application.Staff.Commands.UpdateStaff;
using Lume.Application.Staff.Dtos;
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

[TestSubject(typeof(StaffController))]
public class StaffControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IStaffRepository> _staffRepositoryMock = new();
    private readonly Mock<ISeederOrchestrator> _seederOrchestratorMock = new();

    public StaffControllerTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IStaffRepository),
                    _ => _staffRepositoryMock.Object));
                services.Replace(ServiceDescriptor.Singleton(typeof(ISeederOrchestrator),
                    _ => _seederOrchestratorMock.Object));
            });
        });
    }
    
    [Fact]
    public async Task GetAllStaff_WhenRequestIsValid_Returns200Ok()
    {
        // arrange
        var client = _factory.CreateClient();
        
        // act
        var result = await client.GetAsync("/api/Staff");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task GetStaffById_WhenRequestIsValid_Returns200Ok()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var staff = new ApplicationUser
        {
            Id = id,
            Name = "Staff Name",
            Surname = "Staff Surname",
            UserType = "Staff",
            Salary = 2000
        };
        _staffRepositoryMock.Setup(r => r.GetStaffById(id)).ReturnsAsync(staff);
        
        // act
        var result = await client.GetAsync($"/api/Staff/{id}");
        var staffDto = await result.Content.ReadFromJsonAsync<StaffDto>();
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        staffDto.Should().NotBeNull();
        staffDto.Name.Should().Be("Staff Name");
        staffDto.Surname.Should().Be("Staff Surname");
        staffDto.Salary.Should().Be(2000);
    }
    
    [Fact]
    public async Task GetStaffById_WhenStaffDoesNotExist_Returns404NotFound()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        _staffRepositoryMock.Setup(r => r.GetStaffById(id)).ReturnsAsync((ApplicationUser)null);
        
        // act
        var result = await client.GetAsync($"/api/Staff/{id}");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteStaff_WhenStaffDoesExist_Returns204NoContent()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var staff = new ApplicationUser
        {
            Id = id,
            Name = "Staff Name",
            Surname = "Staff Surname",
            UserType = "Staff",
            Salary = 2000,
            IsActive = true,
            MonthHours = 160
        };
        _staffRepositoryMock.Setup(r => r.GetStaffById(id)).ReturnsAsync(staff);
        _staffRepositoryMock.Setup(r => r.DeleteStaff(staff));
        
        // act
        var result = await client.DeleteAsync($"/api/Staff/{id}");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        _staffRepositoryMock.Verify(r => r.DeleteStaff(staff), Times.Once);
    }
    
    [Fact]
    public async Task DeleteStaff_WhenStaffDoesNotExist_Returns404NotFound()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        _staffRepositoryMock.Setup(r => r.GetStaffById(id)).ReturnsAsync((ApplicationUser)null);
        
        // act
        var result = await client.DeleteAsync($"/api/Staff/{id}");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        _staffRepositoryMock.Verify(r => r.DeleteStaff(It.IsAny<ApplicationUser>()), Times.Never);
    }

    [Fact]
    public async Task UpdateStaff_WhenStaffDoesExist_Returns204NoContent()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var request = new UpdateStaffCommand
        {
            Id = id,
            Name = "New Staff Name",
            Surname = "New Staff Surname",
            PhoneNumber = "+1234567890123",
            Salary = 2000
        };
        var staff = new ApplicationUser
        {
            Id = id,
            Name = "Staff Name",
            Surname = "Staff Surname",
            PhoneNumber = "+1234567890321",
            UserType = "Staff",
            Salary = 3000,
            IsActive = true,
            MonthHours = 160
        };
        _staffRepositoryMock.Setup(r => r.GetStaffById(id)).ReturnsAsync(staff);
        
        // act
        var result = await client.PatchAsJsonAsync($"/api/Staff/{id}", request);
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        staff.Name.Should().Be("New Staff Name");
        staff.Surname.Should().Be("New Staff Surname");
        staff.PhoneNumber.Should().Be("+1234567890123");
        staff.Salary.Should().Be(2000);
        _staffRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
    }
    
    [Fact]
    public async Task UpdateStaff_WithInvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var request = new UpdateStaffCommand
        {
            Id = id,
            Name = "",
            Surname = "",
            PhoneNumber = "",
            Salary = -1000
        };

        // Act
        var result = await client.PatchAsJsonAsync($"/api/Staff/{id}", request);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _staffRepositoryMock.Verify(r => r.SaveChanges(), Times.Never);
    }
    
    [Fact]
    public async Task UpdateStaff_WhenStaffDoesNotExist_Returns404NotFound()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var request = new UpdateStaffCommand
        {
            Id = id,
            Name = "Name",
            Surname = "Surname",
            PhoneNumber = "+1234567890123",
            Salary = 2000
        };
        _staffRepositoryMock.Setup(r => r.GetStaffById(id)).ReturnsAsync((ApplicationUser)null);
        
        // act
        var result = await client.PatchAsJsonAsync($"/api/Staff/{id}", request);
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        _staffRepositoryMock.Verify(r => r.SaveChanges(), Times.Never);
    }
    
    [Fact]
    public async Task CreateStaff_WhenRequestIsValid_Returns201Created()
    {
        // arrange
        var client = _factory.CreateClient();
        var expectedId = Guid.NewGuid();
        var request = new CreateStaffCommand
        {
            Name = "Staff Name",
            Surname = "Staff Surname",
            PasswordHash = "Password123!",
            PhoneNumber = "+1234567890123",
            Salary = 2000,
        };
        var staff = new ApplicationUser
        {
            Id = expectedId,
            Name = "Staff Name",
            Surname = "Staff Surname",
            PasswordHash = "Password123!",
            PhoneNumber = "+1234567890123",
            Salary = 2000,
            UserType = "Staff"
        };

        _staffRepositoryMock.Setup(r => r.CreateStaff(staff)).ReturnsAsync(expectedId);
        
        // act
        var result = await client.PostAsync("/api/Staff", JsonContent.Create(request));
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Headers.Location.Should().NotBeNull();
        _staffRepositoryMock.Verify(r => r.CreateStaff(It.IsAny<ApplicationUser>()), Times.Once);
    }
    
    [Fact]
    public async Task CreateStaff_WithInvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new CreateStaffCommand
        {
            Name = "",
            Surname = "",
            PasswordHash = "",
            PhoneNumber = "",
            Salary = -1000
        };

        // Act
        var result = await client.PostAsJsonAsync("/api/Staff", request);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _staffRepositoryMock.Verify(r => r.CreateStaff(It.IsAny<ApplicationUser>()), Times.Never);
    }
}
