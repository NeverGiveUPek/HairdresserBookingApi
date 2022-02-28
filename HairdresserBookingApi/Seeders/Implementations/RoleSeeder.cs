using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Entities.Users;

namespace HairdresserBookingApi.Seeders.Implementations;

public class RoleSeeder : ISeeder
{
    public void Seed(BookingDbContext dbContext)
    {
        if (!dbContext.Roles.Any())
        {
            dbContext.Roles.AddRange(GetRoles());
            dbContext.SaveChanges();
        }
    }


    private IEnumerable<Role> GetRoles()
    {
        var roles = new List<Role>()
        {
            new Role()
            {
                Name = "User"
            },
            new Role()
            {
                Name = "Manager"
            },
            new Role()
            {
                Name = "Admin"
            }
        };

        return roles;
    }
}