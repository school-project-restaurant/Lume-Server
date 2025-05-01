using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Lume.Application.Reservations.Commands.CreateReservation;
using Lume.Application.Reservations.Commands.UpdateReservation;
using Lume.Application.Reservations.Dtos;
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

[TestSubject(typeof(ReservationsController))]
public class ReservationsControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IReservationRepository> _reservationRepositoryMock = new();
    private readonly Mock<ISeederOrchestrator> _seederOrchestratorMock = new();

    public ReservationsControllerTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IReservationRepository),
                    _ => _reservationRepositoryMock.Object));
                services.Replace(ServiceDescriptor.Singleton(typeof(ISeederOrchestrator),
                    _ => _seederOrchestratorMock.Object));
            });
        });
    }

    [Fact]
    public async Task GetAllReservations_WhenRequestIsValid_Returns200Ok()
    {
        // arrange
        var client = _factory.CreateClient();
        var reservations = new List<Reservation>
        {
            new()
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                Date = DateTime.Now.AddDays(1),
                TableNumber = 5,
                GuestCount = 4,
                Status = ReservationStatus.Confirmed,
                Notes = "Window seat requested"
            }
        };
        
        _reservationRepositoryMock.Setup(r => r.GetAllReservations())
            .ReturnsAsync(reservations);
        
        // act
        var result = await client.GetAsync("/api/Reservations");
        var reservationDtos = await result.Content.ReadFromJsonAsync<List<ReservationDto>>();
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        reservationDtos.Should().NotBeNull();
        reservationDtos.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task GetReservationById_WhenReservationExists_Returns200Ok()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var reservation = new Reservation
        {
            Id = id,
            CustomerId = Guid.NewGuid(),
            Date = DateTime.Now.AddDays(2),
            TableNumber = 3,
            GuestCount = 2,
            Status = ReservationStatus.Pending,
            Notes = "Anniversary dinner"
        };
        
        _reservationRepositoryMock.Setup(r => r.GetReservationById(id))
            .ReturnsAsync(reservation);
        
        // act
        var result = await client.GetAsync($"/api/Reservations/{id}");
        var reservationDto = await result.Content.ReadFromJsonAsync<ReservationDto>();
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        reservationDto.Should().NotBeNull();
        reservationDto.Id.Should().Be(id);
        reservationDto.TableNumber.Should().Be(3);
        reservationDto.GuestCount.Should().Be(2);
        reservationDto.Status.Should().Be(ReservationStatus.Pending);
    }
    
    [Fact]
    public async Task GetReservationById_WhenReservationDoesNotExist_Returns404NotFound()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        
        _reservationRepositoryMock.Setup(r => r.GetReservationById(id))
            .ReturnsAsync((Reservation)null);
        
        // act
        var result = await client.GetAsync($"/api/Reservations/{id}");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateReservation_WhenRequestIsValid_Returns201Created()
    {
        // arrange
        var client = _factory.CreateClient();
        var expectedId = Guid.NewGuid();
        var request = new CreateReservationCommand
        {
            CustomerId = Guid.NewGuid(),
            Date = DateTime.Now.AddDays(3),
            TableNumber = 7,
            GuestCount = 6,
            Status = ReservationStatus.Pending,
            Notes = "Birthday celebration"
        };
        
        _reservationRepositoryMock.Setup(r => r.CreateReservation(It.IsAny<Reservation>()))
            .ReturnsAsync(expectedId);
        
        // act
        var result = await client.PostAsJsonAsync("/api/Reservations", request);
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        result.Headers.Location.Should().NotBeNull();
        _reservationRepositoryMock.Verify(r => r.CreateReservation(It.IsAny<Reservation>()), Times.Once);
    }
    
    [Fact]
    public async Task CreateReservation_WithInvalidModel_ReturnsBadRequest()
    {
        // arrange
        var client = _factory.CreateClient();
        var request = new CreateReservationCommand
        {
            CustomerId = Guid.Empty, // Invalid CustomerId
            Date = DateTime.Now.AddDays(-1), // Past date
            TableNumber = -1, // Negative table number
            GuestCount = 0, // Zero guests
            Status = "InvalidStatus" // Invalid status
        };
        
        // act
        var result = await client.PostAsJsonAsync("/api/Reservations", request);
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _reservationRepositoryMock.Verify(r => r.CreateReservation(It.IsAny<Reservation>()), Times.Never);
    }
    
    [Fact]
    public async Task DeleteReservation_WhenReservationExists_Returns204NoContent()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var reservation = new Reservation
        {
            Id = id,
            CustomerId = Guid.NewGuid(),
            Date = DateTime.Now.AddDays(1),
            TableNumber = 5,
            GuestCount = 4,
            Status = ReservationStatus.Confirmed
        };
        
        _reservationRepositoryMock.Setup(r => r.GetReservationById(id))
            .ReturnsAsync(reservation);
        _reservationRepositoryMock.Setup(r => r.DeleteReservation(reservation));
        
        // act
        var result = await client.DeleteAsync($"/api/Reservations/{id}");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        _reservationRepositoryMock.Verify(r => r.DeleteReservation(reservation), Times.Once);
    }
    
    [Fact]
    public async Task DeleteReservation_WhenReservationDoesNotExist_Returns404NotFound()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        
        _reservationRepositoryMock.Setup(r => r.GetReservationById(id))
            .ReturnsAsync((Reservation)null);
        
        // act
        var result = await client.DeleteAsync($"/api/Reservations/{id}");
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        _reservationRepositoryMock.Verify(r => r.DeleteReservation(It.IsAny<Reservation>()), Times.Never);
    }
    
    [Fact]
    public async Task UpdateReservation_WhenReservationExists_Returns204NoContent()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var request = new UpdateReservationCommand
        {
            Id = id,
            Date = DateTime.Parse("15/05/2006"),
            TableNumber = 10,
            GuestCount = 8,
            Status = ReservationStatus.Confirmed,
            Notes = "Updated notes"
        };
        
        var reservation = new Reservation
        {
            Id = id,
            CustomerId = Guid.NewGuid(),
            Date = DateTime.Now.AddDays(1),
            TableNumber = 5,
            GuestCount = 4,
            Status = ReservationStatus.Pending
        };
        
        _reservationRepositoryMock.Setup(r => r.GetReservationById(id))
            .ReturnsAsync(reservation);
        _reservationRepositoryMock.Setup(r => r.SaveChanges());
        
        // act
        var result = await client.PatchAsJsonAsync($"/api/Reservations/{id}", request);
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        _reservationRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
    }
    
    [Fact]
    public async Task UpdateReservation_WhenReservationDoesNotExist_Returns404NotFound()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var request = new UpdateReservationCommand
        {
            Id = id,
            Date = DateTime.Parse("15/05/2006"),
            TableNumber = 12,
            GuestCount = 4,
            Status = ReservationStatus.Rejected,
            Notes = "Cancelled"
        };
        
        _reservationRepositoryMock.Setup(r => r.GetReservationById(id))
            .ReturnsAsync((Reservation)null);
        
        // act
        var result = await client.PatchAsJsonAsync($"/api/Reservations/{id}", request);
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        _reservationRepositoryMock.Verify(r => r.SaveChanges(), Times.Never);
    }
    
    [Fact]
    public async Task UpdateReservation_WithInvalidModel_ReturnsBadRequest()
    {
        // arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();
        var request = new UpdateReservationCommand
        {
            Id = id,
            TableNumber = -5, // Negative table number
            GuestCount = -2, // Negative guest count
            Status = "InvalidStatus" // Invalid status
        };
        
        // act
        var result = await client.PatchAsJsonAsync($"/api/Reservations/{id}", request);
        
        // assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _reservationRepositoryMock.Verify(r => r.SaveChanges(), Times.Never);
    }
}
