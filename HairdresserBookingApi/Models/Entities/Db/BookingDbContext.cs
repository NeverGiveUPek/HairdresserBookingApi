using HairdresserBookingApi.Models.Entities.Api;
using Microsoft.EntityFrameworkCore;

namespace HairdresserBookingApi.Models.Entities.Db;

public class BookingDbContext : DbContext
{

    public DbSet<Service> Services { get; set; }
    public DbSet<Worker> Workers { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkerService>()
            .HasKey(t => new {t.WorkerId, t.ServiceId});

        modelBuilder.Entity<WorkerService>()
            .HasOne(ws => ws.Worker)
            .WithMany(w => w.WorkerServices)
            .HasForeignKey(ws => ws.WorkerId);
        
        modelBuilder.Entity<WorkerService>()
            .HasOne(ws => ws.Service)
            .WithMany(s => s.WorkerServices)
            .HasForeignKey(ws => ws.ServiceId);

    }
}