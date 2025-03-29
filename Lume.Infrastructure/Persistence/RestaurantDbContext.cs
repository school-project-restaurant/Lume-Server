using Lume.Domain.Entities;
        using Microsoft.AspNetCore.Identity;
        using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
        using Microsoft.EntityFrameworkCore;
        
        namespace Lume.Infrastructure.Persistence;
        
        internal class RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : DbContext(options)
        {
            public DbSet<Customer> Customers { get; set; }
            public DbSet<Reservation> Reservations { get; set; }
            public DbSet<Table> Tables { get; set; }
            public DbSet<Staff> Staffs { get; set; }
        
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
        
                modelBuilder.Entity<Table>().HasKey(t => t.Number);
                modelBuilder.Entity<Customer>().HasKey(c => c.PhoneNumber);
                modelBuilder.Entity<Reservation>().HasKey(r => r.Id);
                modelBuilder.Entity<Staff>().HasKey(s => s.Id);
            }
        }