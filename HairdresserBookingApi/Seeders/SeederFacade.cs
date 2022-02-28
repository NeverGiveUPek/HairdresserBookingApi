using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Entities.Users;
using HairdresserBookingApi.Seeders.Implementations;
using Microsoft.AspNetCore.Identity;

namespace HairdresserBookingApi.Seeders;

public class SeederFacade
{
    private readonly BookingDbContext _dbContext;

    private readonly List<ISeeder> _seeders = new();




    public SeederFacade(BookingDbContext dbContext, IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;

        _seeders.Add(new ActivitySeeder());
        _seeders.Add(new WorkerSeeder());
        _seeders.Add(new RoleSeeder());
        _seeders.Add(new UserSeeder(passwordHasher));
        _seeders.Add(new WorkerActivitySeeder());
    }

    public void Seed()
    {
        if (_dbContext.Database.CanConnect())
        {
            _seeders.ForEach(s => s.Seed(_dbContext));
        }
    }
}