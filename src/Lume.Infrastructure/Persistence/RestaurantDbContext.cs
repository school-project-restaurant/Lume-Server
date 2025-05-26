using Lume.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lume.Infrastructure.Persistence;

internal class RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<Dish> Dishes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>();

        modelBuilder.Entity<Table>().HasKey(t => t.Number);
        modelBuilder.Entity<Reservation>().HasKey(r => r.Id);
        modelBuilder.Entity<Dish>().HasKey(r => r.Id);
    }
}
