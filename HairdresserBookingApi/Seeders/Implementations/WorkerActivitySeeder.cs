using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Entities.Api;

namespace HairdresserBookingApi.Seeders.Implementations;

public class WorkerActivitySeeder : ISeeder
{
    public void Seed(BookingDbContext dbContext)
    {
        if (!dbContext.WorkerActivities.Any())
        {
            dbContext.WorkerActivities.AddRange(GetWorkerActivities(dbContext));
            dbContext.SaveChanges();
        }
    }


    private IEnumerable<WorkerActivity> GetWorkerActivities(BookingDbContext dbContext)
    {
        var workers = dbContext.Workers.ToList();
        var activities = dbContext.Activities.ToList();

        var workerActivities = new List<WorkerActivity>();

        var firstWorkerActivity = new WorkerActivity()
        {
            Price = 40,
            RequiredMinutes = 30,
            ActivityId = activities.First().Id,
            WorkerId = workers.First().Id
        };
        workerActivities.Add(firstWorkerActivity);


        var secondWorkerActivity = new WorkerActivity()
        {
            Price = 50,
            RequiredMinutes = 45,
            ActivityId = activities.First().Id,
            WorkerId = workers.Skip(1).First().Id
        };
        workerActivities.Add(secondWorkerActivity);


        var thirdWorkerActivity = new WorkerActivity()
        {
            Price = 120,
            RequiredMinutes = 120,
            ActivityId = activities.Skip(1).First().Id,
            WorkerId = workers.Skip(1).First().Id
        };
        workerActivities.Add(thirdWorkerActivity);


        var fourthWorkerActivity = new WorkerActivity()
        {
            Price = 100,
            RequiredMinutes = 90,
            ActivityId = activities.Skip(1).First().Id,
            WorkerId = workers.Skip(2).First().Id
        };
        workerActivities.Add(fourthWorkerActivity);

        var fifthWorkerActivity = new WorkerActivity()
        {
            Price = 200,
            RequiredMinutes = 180,
            ActivityId = activities.Skip(2).First().Id,
            WorkerId = workers.Skip(2).First().Id
        };
        workerActivities.Add(fourthWorkerActivity);


        return workerActivities;
    }
}