using HairdresserBookingApi.Models.Entities;
using HairdresserBookingApi.Models.Entities.Api;
using Microsoft.EntityFrameworkCore;

namespace HairdresserBookingApi.Models.Db;

public class BookingDbContext : DbContext
{

    public DbSet<Service> Services { get; set; }
    public DbSet<Worker> Workers { get; set; }
    public DbSet<WorkerService> WorkerServices { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<WorkerAvailability> WorkerAvailabilities { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkerService>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<WorkerService>()
            .HasOne(ws => ws.Worker)
            .WithMany(w => w.WorkerServices)
            .HasForeignKey(ws => ws.WorkerId);
        
        modelBuilder.Entity<WorkerService>()
            .HasOne(ws => ws.Service)
            .WithMany(s => s.WorkerServices)
            .HasForeignKey(ws => ws.ServiceId);
    }

    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
    {

    }
}