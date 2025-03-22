using Lume.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lume.Infrastructure.Persistence;

internal class RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : DbContext(options)
{
    internal DbSet<Customer> Customers { get; set; }

}