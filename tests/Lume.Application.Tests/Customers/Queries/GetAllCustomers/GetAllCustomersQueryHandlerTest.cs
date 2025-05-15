using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Lume.Application.Customers.Dtos;
using Lume.Application.Customers.Queries.GetAllCustomers;
using Lume.Domain.Constants;
using Lume.Domain.Entities;
using Lume.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Lume.Application.Tests.Customers.Queries.GetAllCustomers;

[TestSubject(typeof(GetAllCustomersQueryHandler))]
public class GetAllCustomersQueryHandlerTest
{
    private readonly Mock<ILogger<GetAllCustomersQueryHandler>> _loggerMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();

    [Fact]
    public async Task Handle_WhenRequestIsValid_ReturnPagedCustomerDtos()
    {
        // arrange
        var query = new GetAllCustomersQuery
        {
            PageSize = 10,
            PageIndex = 1,
            SortBy = "name",
            SortDirection = SortDirection.Ascending
        };

        var filterOptions = new CustomerFilterOptions();
        var sortOptions = new CustomerSortOptions();
        
        var customers = new List<ApplicationUser>
        {
            new() { Id = Guid.NewGuid(), Name = "John", Surname = "Doe" },
            new() { Id = Guid.NewGuid(), Name = "Jane", Surname = "Smith" }
        };
        
        var customerDtos = new List<CustomerDto>
        {
            new() { Name = "John", Surname = "Doe" },
            new() { Name = "Jane", Surname = "Smith" }
        };
        
        int totalCount = 2;
        
        _mapperMock.Setup(m => m.Map<CustomerFilterOptions>(query))
            .Returns(filterOptions);
            
        _mapperMock.Setup(m => m.Map<CustomerSortOptions>(query))
            .Returns(sortOptions);
            
        _customerRepositoryMock.Setup(r => r.GetMatchingCustomers(
                It.IsAny<CustomerFilterOptions>(), 
                It.IsAny<CustomerSortOptions>()))
            .ReturnsAsync((customers, totalCount));
        
        _mapperMock.Setup(m => m.Map<IEnumerable<CustomerDto>>(customers))
            .Returns(customerDtos);

        var handler = new GetAllCustomersQueryHandler(
            _loggerMock.Object, 
            _mapperMock.Object, 
            _customerRepositoryMock.Object);
        
        // act
        var result = await handler.Handle(query, CancellationToken.None);
        
        // assert
        result.Should().NotBeNull();
        result.Items.Should().BeEquivalentTo(customerDtos);
        result.TotalItemsCount.Should().Be(totalCount);
        result.TotalPages.Should().Be(1);
        
        _customerRepositoryMock.Verify(r => r.GetMatchingCustomers(
            It.IsAny<CustomerFilterOptions>(), 
            It.IsAny<CustomerSortOptions>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_WhenNoCustomersExist_ReturnEmptyPagedResult()
    {
        // arrange
        var query = new GetAllCustomersQuery
        {
            PageSize = 10,
            PageIndex = 1
        };

        var filterOptions = new CustomerFilterOptions();
        var sortOptions = new CustomerSortOptions();
        
        var emptyCustomersList = new List<ApplicationUser>();
        var emptyDtosList = new List<CustomerDto>();
        int totalCount = 0;

        _mapperMock.Setup(m => m.Map<CustomerFilterOptions>(query))
            .Returns(filterOptions);
            
        _mapperMock.Setup(m => m.Map<CustomerSortOptions>(query))
            .Returns(sortOptions);
            
        _customerRepositoryMock.Setup(r => r.GetMatchingCustomers(
                It.IsAny<CustomerFilterOptions>(), 
                It.IsAny<CustomerSortOptions>()))
            .ReturnsAsync((emptyCustomersList, totalCount));
        
        _mapperMock.Setup(m => m.Map<IEnumerable<CustomerDto>>(emptyCustomersList))
            .Returns(emptyDtosList);

        var handler = new GetAllCustomersQueryHandler(
            _loggerMock.Object,
            _mapperMock.Object, 
            _customerRepositoryMock.Object);
    
        // act
        var result = await handler.Handle(query, CancellationToken.None);

        // assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalItemsCount.Should().Be(0);
        result.TotalPages.Should().Be(0);
        
        _customerRepositoryMock.Verify(r => r.GetMatchingCustomers(
            It.IsAny<CustomerFilterOptions>(), 
            It.IsAny<CustomerSortOptions>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_WithSearchFilters_PassesFiltersToRepository()
    {
        // arrange
        var query = new GetAllCustomersQuery
        {
            SearchName = "John",
            SearchEmail = "john@example.com",
            PageSize = 10,
            PageIndex = 1
        };

        CustomerFilterOptions capturedFilterOptions = null!;
        CustomerSortOptions capturedSortOptions = null!;
        
        _mapperMock.Setup(m => m.Map<CustomerFilterOptions>(query))
            .Returns(new CustomerFilterOptions { 
                SearchName = "John", 
                SearchEmail = "john@example.com",
                PageSize = 10,
                PageIndex = 1
            });
            
        _mapperMock.Setup(m => m.Map<CustomerSortOptions>(query))
            .Returns(new CustomerSortOptions());
            
        _customerRepositoryMock.Setup(r => r.GetMatchingCustomers(
                It.IsAny<CustomerFilterOptions>(), 
                It.IsAny<CustomerSortOptions>()))
            .Callback<CustomerFilterOptions, CustomerSortOptions>((filters, sort) => {
                capturedFilterOptions = filters;
                capturedSortOptions = sort;
            })
            .ReturnsAsync((new List<ApplicationUser>(), 0));
        
        _mapperMock.Setup(m => m.Map<IEnumerable<CustomerDto>>(It.IsAny<IEnumerable<ApplicationUser>>()))
            .Returns(new List<CustomerDto>());

        var handler = new GetAllCustomersQueryHandler(
            _loggerMock.Object,
            _mapperMock.Object, 
            _customerRepositoryMock.Object);
    
        // act
        await handler.Handle(query, CancellationToken.None);

        // assert
        capturedFilterOptions.Should().NotBeNull();
        capturedFilterOptions.SearchName.Should().Be("John");
        capturedFilterOptions.SearchEmail.Should().Be("john@example.com");
        
        _customerRepositoryMock.Verify(r => r.GetMatchingCustomers(
            It.IsAny<CustomerFilterOptions>(), 
            It.IsAny<CustomerSortOptions>()), Times.Once);
    }
}
