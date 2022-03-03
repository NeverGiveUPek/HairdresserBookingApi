using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Entities.Api;

namespace HairdresserBookingApi.Seeders.Implementations;

public class ReservationSeeder : ISeeder
{
    public void Seed(BookingDbContext dbContext)
    {

        if (!dbContext.Reservations.Any())
        {
            var reservations = GetReservation(dbContext);

            dbContext.Reservations.AddRange(reservations);

            dbContext.SaveChanges();
        }

    }


    private ICollection<Reservation> GetReservation(BookingDbContext dbContext)
    {
        var reservations = new List<Reservation>();

        var users = dbContext.Users.ToList();

        var workerActivities = dbContext.WorkerActivities.ToList();

        var todayMidnight = DateTime.Today;

        var firstReservation = new Reservation()
        {
            Date = todayMidnight.AddDays(1).AddHours(13), //13:00.00
            UserId = users.First().Id,
            WorkerActivityId = workerActivities.First().Id
        };

        reservations.Add(firstReservation);

        var secondReservation = new Reservation()
        {
            Date = todayMidnight.AddDays(1).AddHours(15), //15:00.00
            UserId = users.Skip(1).First().Id,
            WorkerActivityId = workerActivities.First().Id
        };

        reservations.Add(secondReservation);

        var thirdReservation = new Reservation()
        {
            Date = todayMidnight.AddHours(15), //15:00.00
            UserId = users.Skip(2).First().Id,
            WorkerActivityId = workerActivities.Skip(1).First().Id
        };

        reservations.Add(thirdReservation);

        return reservations;
    }
}