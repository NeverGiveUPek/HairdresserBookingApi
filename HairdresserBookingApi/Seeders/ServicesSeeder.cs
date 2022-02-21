using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Entities.Api;

namespace HairdresserBookingApi.Seeders;

public class ServicesSeeder : ISeeder
{
    public void Seed(BookingDbContext dbContext)
    {
        if (dbContext.Services.Any()) return;

        var services = GetServices();
        
        dbContext.Services.AddRange(services);
    }

    private IEnumerable<Service> GetServices()
    {
        var services = new List<Service>();

        services.Add(new Service()
        {
            Name = "Strzyżenie męskie",
            Description = "Szybkie męskie strzyżenie",
            IsForMan = true
        });

        services.Add(new Service()
        {
            Name = "Strzyżenie damskie",
            Description = "Profesjonalne strzyżenie i pielęgnacja włosów",
            IsForMan = false
        });

        services.Add(new Service()
        {
            Name = "Farbowanie włosów długich damskich",
            Description = "Farbowanie długich włosów",
            IsForMan = false
        });



        return services;
    }
}