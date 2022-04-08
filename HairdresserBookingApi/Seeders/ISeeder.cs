using HairdresserBookingApi.Models.Db;

namespace HairdresserBookingApi.Seeders;

public interface ISeeder
{
    void Seed(BookingDbContext dbContext);
}