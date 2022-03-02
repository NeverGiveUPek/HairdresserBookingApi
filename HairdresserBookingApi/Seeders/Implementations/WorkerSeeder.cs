using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Entities.Api;

namespace HairdresserBookingApi.Seeders.Implementations;

public class WorkerSeeder : ISeeder
{
    public void Seed(BookingDbContext dbContext)
    {
        if (dbContext.Workers.Any()) return;

        var workers = GetWorkers();

        dbContext.Workers.AddRange(workers);
        dbContext.SaveChanges();
    }

    private IEnumerable<Worker> GetWorkers()
    {
        var workers = new List<Worker>();

        var todayMidnight = DateTime.Today;
        var todayStart = todayMidnight.AddHours(10);
        var todayEnd = todayMidnight.AddHours(18);

        workers.Add(new Worker()
        {
            FirstName = "Agnieszka",
            LastName = "Kowalska",
            Email = "akowalska@gmail.com",
            PhoneNumber = "+48 123 123 123",
            Availabilities = new List<Availability>()
            {
                new Availability()
                {
                    Start = todayStart.AddDays(1),
                    End = todayEnd.AddDays(1),
                },
                new Availability()
                {
                    Start = todayStart.AddDays(2),
                    End = todayEnd.AddDays(2),
                }
            }
        });

        workers.Add(new Worker()
        {
            FirstName = "Adam",
            LastName = "Miły",
            Email = "adam_miły@gmail.com",
            PhoneNumber = "+48 321 321 321",
            Availabilities = new List<Availability>()
            {
                new Availability()
                {
                    Start = todayStart,
                    End = todayEnd
                }
            }
        });

        workers.Add(new Worker()
        {
            FirstName = "Natalia",
            LastName = "Nowak",
            Email = "natalia_nowak@gmail.com",
            PhoneNumber = "+48 222 111 112",
            Availabilities = new List<Availability>()
            {
                new Availability()
                {
                    Start = todayStart,
                    End = todayEnd
                }
            } 
        });


        return workers;
    }
}