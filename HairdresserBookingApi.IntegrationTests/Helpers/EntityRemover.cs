using System.Linq;
using HairdresserBookingApi.Models.Db;
using Microsoft.Extensions.DependencyInjection;

namespace HairdresserBookingApi.IntegrationTests.Helpers;

public static class EntityRemover
{
    public static void RemoveUserReservations(int id, CustomWebApplicationFactory<Program> factory)
    {
        var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();

        using var scope = scopeFactory?.CreateScope();
        var dbContext = scope?.ServiceProvider.GetService<BookingDbContext>();

        dbContext?.Reservations.RemoveRange(dbContext.Reservations.Where(r => r.UserId == id));
        dbContext?.SaveChanges();
    }
}