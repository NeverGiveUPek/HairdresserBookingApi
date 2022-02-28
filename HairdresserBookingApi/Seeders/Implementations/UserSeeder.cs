using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace HairdresserBookingApi.Seeders.Implementations;

public class UserSeeder : ISeeder
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserSeeder(IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public void Seed(BookingDbContext dbContext)
    {
        if (!dbContext.Users.Any())
        {
            dbContext.Users.AddRange(GetUsers(dbContext));
            dbContext.SaveChanges();
        }
    }


    private IEnumerable<User> GetUsers(BookingDbContext dbContext)
    {
        var users = new List<User>();

        var roles = dbContext.Roles.ToList();

        
        var firstUser = new User()
        {
            FirstName = "Jan",
            LastName = "Kowalski",
            DateOfBirth = DateTime.Today.AddHours(-1),
            Email = "janKowalski@gmail.com",
            RoleId = roles.First().Id
        };
        
        firstUser.PasswordHash = _passwordHasher.HashPassword(firstUser, "JanKowalski123");

        users.Add(firstUser);

        var secondUser = new User()
        {
            FirstName = "Adam",
            LastName = "Bystry",
            DateOfBirth = DateTime.Today,
            Email = "adamBystry@gmail.com",
            RoleId = roles.Skip(1).First().Id
        };
        secondUser.PasswordHash = _passwordHasher.HashPassword(secondUser, "AdamBystry123");

        users.Add(secondUser);


        var thirdUser = new User()
        {
            FirstName = "Anna",
            LastName = "Nowak",
            DateOfBirth = DateTime.Today.AddDays(1),
            Email = "annaNowak@gmail.com",
            RoleId = roles.Skip(2).First().Id
        };
        thirdUser.PasswordHash = _passwordHasher.HashPassword(thirdUser, "AnnaNowak123");

        users.Add(thirdUser);


        return users;
    }



}