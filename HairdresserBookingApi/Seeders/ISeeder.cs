using HairdresserBookingApi.Models.Entities.Db;

namespace HairdresserBookingApi.Seeders;

public interface ISeeder
{
    void Seed(BookingDbContext dbContext);

}