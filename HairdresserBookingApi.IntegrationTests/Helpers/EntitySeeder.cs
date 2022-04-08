using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Entities.Api;
using Microsoft.Extensions.DependencyInjection;

namespace HairdresserBookingApi.IntegrationTests.Helpers;

public static class EntitySeeder
{
    public static void SeedActivity(Activity activity, CustomWebApplicationFactory<Program> factory)
    {
        var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();

        using var scope = scopeFactory?.CreateScope();
        var dbContext = scope?.ServiceProvider.GetService<BookingDbContext>();

        dbContext?.Activities.Add(activity);
        dbContext?.SaveChanges();
    }

    public static void SeedWorker(Worker worker, CustomWebApplicationFactory<Program> factory)
    {
        var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();

        using var scope = scopeFactory?.CreateScope();
        var dbContext = scope?.ServiceProvider.GetService<BookingDbContext>();

        dbContext?.Workers.Add(worker);
        dbContext?.SaveChanges();
    }

    public static void SeedAvailability(Availability availability, CustomWebApplicationFactory<Program> factory)
    {
        var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();

        using var scope = scopeFactory?.CreateScope();
        var dbContext = scope?.ServiceProvider.GetService<BookingDbContext>();

        dbContext?.Availabilities.Add(availability);
        dbContext?.SaveChanges();
    }

    public static void SeedWorkerActivity(WorkerActivity workerActivity, CustomWebApplicationFactory<Program> factory)
    {
        var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();

        using var scope = scopeFactory?.CreateScope();
        var dbContext = scope?.ServiceProvider.GetService<BookingDbContext>();

        dbContext?.WorkerActivities.Add(workerActivity);
        dbContext?.SaveChanges();
    }

    public static void SeedReservation(Reservation reservation, CustomWebApplicationFactory<Program> factory)
    {
        var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();

        using var scope = scopeFactory?.CreateScope();
        var dbContext = scope?.ServiceProvider.GetService<BookingDbContext>();

        dbContext?.Reservations.Add(reservation);
        dbContext?.SaveChanges();
    }
}