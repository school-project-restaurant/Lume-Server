using Lume.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lume.Infrastructure.Persistence;

internal class RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) 
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<Staff> Staffs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>()
            .HasDiscriminator<string>("UserType")
            .HasValue<Customer>("Customer")
            .HasValue<Staff>("Staff");

        modelBuilder.Entity<Table>().HasKey(t => t.Number);
        modelBuilder.Entity<Reservation>().HasKey(r => r.Id);
    }
}