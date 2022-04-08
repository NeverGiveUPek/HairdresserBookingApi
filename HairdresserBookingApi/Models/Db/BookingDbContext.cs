using HairdresserBookingApi.Models.Entities;
using HairdresserBookingApi.Models.Entities.Api;
using HairdresserBookingApi.Models.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace HairdresserBookingApi.Models.Db;

public class BookingDbContext : DbContext
{
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Worker> Workers { get; set; }
    public DbSet<WorkerActivity> WorkerActivities { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Availability> Availabilities { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkerActivity>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<WorkerActivity>()
            .HasOne(ws => ws.Worker)
            .WithMany(w => w.WorkerActivity)
            .HasForeignKey(ws => ws.WorkerId);

        modelBuilder.Entity<WorkerActivity>()
            .HasOne(ws => ws.Activity)
            .WithMany(s => s.WorkerActivity)
            .HasForeignKey(ws => ws.ActivityId);
    }

    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
    {
    }
}