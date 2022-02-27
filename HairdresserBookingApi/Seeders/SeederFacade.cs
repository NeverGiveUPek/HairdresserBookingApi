using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace HairdresserBookingApi.Seeders;

public class SeederFacade
{
    private readonly BookingDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;


    private readonly List<ISeeder> _seeders = new();




    public SeederFacade(BookingDbContext dbContext, IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;

        _seeders.Add(new ActivitySeeder());
        _seeders.Add(new WorkerSeeder());
        _seeders.Add(new RoleSeeder());
        _seeders.Add(new UserSeeder(passwordHasher));
    }

    public void Seed()
    {
        if (_dbContext.Database.CanConnect())
        {
            _seeders.ForEach(s => s.Seed(_dbContext));
        }
    }
}