using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Entities.Api;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HairdresserBookingApi.IntegrationTests;

public class WorkerControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public WorkerControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    private void SeedWorker(Worker worker)
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();

        using var scope = scopeFactory?.CreateScope();
        var dbContext = scope?.ServiceProvider.GetService<BookingDbContext>();

        dbContext?.Workers.Add(worker);
        dbContext?.SaveChanges();
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var response = await _client.GetAsync("api/worker");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_ForExistingWorker_ReturnsOk()
    {
        var model = new Worker()
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            PhoneNumber = "123123123"
        };

        SeedWorker(model);

        var response = await _client.GetAsync($"api/worker/{model.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_ForNonExistingWorker_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"api/worker/{-1}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    

}