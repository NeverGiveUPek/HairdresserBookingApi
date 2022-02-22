using HairdresserBookingApi.Models.Db;

namespace HairdresserBookingApi.Seeders;

public class SeederFacade
{
    private readonly BookingDbContext _dbContext;

    private readonly List<ISeeder> _seeders = new();




    public SeederFacade(BookingDbContext dbContext)
    {
        _dbContext = dbContext;

        _seeders.Add(new ActivitySeeder());
        _seeders.Add(new WorkersSeeder());
    }

    public void Seed()
    {
        if (_dbContext.Database.CanConnect())
        {
            _seeders.ForEach(s => s.Seed(_dbContext));
        }

        _dbContext.SaveChanges();
    }
}