using System.Linq;
using HairdresserBookingApi.IntegrationTests.Helpers.Authorization;
using HairdresserBookingApi.Models.Db;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HairdresserBookingApi.IntegrationTests;

public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextOptions = services.SingleOrDefault(service =>
                service.ServiceType == typeof(DbContextOptions<BookingDbContext>));
            if (dbContextOptions != null) services.Remove(dbContextOptions);

            services.AddSingleton<IPolicyEvaluator, TestsPolicyEvaluator>();
            services.AddMvc(x => x.Filters.Add(new TestUserFilter()));


            services.AddDbContext<BookingDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
        });
    }
}