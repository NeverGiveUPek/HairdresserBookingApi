using HairdresserBookingApi.Models.Entities.Api;
using HairdresserBookingApi.Models.Entities.Db;

namespace HairdresserBookingApi.Seeders;

public class WorkersSeeder : ISeeder
{
    public void Seed(BookingDbContext dbContext)
    {
        if (dbContext.Services.Any()) return;

        var workers = GetWorkers();

        dbContext.Workers.AddRange(workers);
    }

    private IEnumerable<Worker> GetWorkers()
    {
        var workers = new List<Worker>();

        workers.Add(new Worker()
        {
            FirstName = "Agnieszka",
            LastName = "Kowalska",
            Email = "akowalska@gmail.com",
            PhoneNumber = "+48 123 123 123"
        });

        workers.Add(new Worker()
        {
            FirstName = "Adam",
            LastName = "Miły",
            Email = "adam_miły@gmail.com",
            PhoneNumber = "+48 321 321 321"
        });


        return workers;
    }
}