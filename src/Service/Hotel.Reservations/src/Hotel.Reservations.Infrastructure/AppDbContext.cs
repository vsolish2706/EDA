using Hotel.Reservations.Domain.Entities;
using Hotel.Reservations.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Reservations.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReservationConfiguration).Assembly);
    }
}