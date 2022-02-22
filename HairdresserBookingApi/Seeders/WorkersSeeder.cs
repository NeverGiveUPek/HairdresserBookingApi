using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Entities.Api;

namespace HairdresserBookingApi.Seeders;

public class WorkersSeeder : ISeeder
{
    public void Seed(BookingDbContext dbContext)
    {
        if (dbContext.Workers.Any()) return;

        var workers = GetWorkers();

        dbContext.Workers.AddRange(workers);
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
            WorkerAvailabilities = new List<WorkerAvailability>()
            {
                new WorkerAvailability()
                {
                    Start = todayStart,
                    End = todayEnd,
                    Breaks = new List<WorkBreak>()
                    {
                        new WorkBreak()
                        {
                            From = todayStart.AddHours(1),
                            To = todayStart.AddHours(2)
                        }
                    }
                }
            }
        });

        workers.Add(new Worker()
        {
            FirstName = "Adam",
            LastName = "Miły",
            Email = "adam_miły@gmail.com",
            PhoneNumber = "+48 321 321 321",
            WorkerAvailabilities = new List<WorkerAvailability>()
            {
                new WorkerAvailability()
                {
                    Start = todayStart,
                    End = todayEnd
                }
            }
        });


        return workers;
    }
}