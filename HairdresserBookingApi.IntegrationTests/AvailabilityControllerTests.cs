using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using HairdresserBookingApi.IntegrationTests.Helpers;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto.Availability;
using HairdresserBookingApi.Models.Entities.Api;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HairdresserBookingApi.IntegrationTests;

public class AvailabilityControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;


    public AvailabilityControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    private void ClearWorkers()
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();

        using var scope = scopeFactory?.CreateScope();
        var dbContext = scope?.ServiceProvider.GetService<BookingDbContext>();

        dbContext?.Workers.RemoveRange(dbContext?.Workers);
        dbContext?.SaveChanges();
    }

    private void ClearAvailabilities()
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();

        using var scope = scopeFactory?.CreateScope();
        var dbContext = scope?.ServiceProvider.GetService<BookingDbContext>();

        dbContext?.Availabilities.RemoveRange(dbContext?.Availabilities);
        dbContext?.SaveChanges();
    }

    private void SeedWorker(Worker worker)
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();

        using var scope = scopeFactory?.CreateScope();
        var dbContext = scope?.ServiceProvider.GetService<BookingDbContext>();

        dbContext?.Workers.Add(worker);
        dbContext?.SaveChanges();
    }

    private void SeedAvailability(Availability availability)
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();

        using var scope = scopeFactory?.CreateScope();
        var dbContext = scope?.ServiceProvider.GetService<BookingDbContext>();

        dbContext?.Availabilities.Add(availability);
        dbContext?.SaveChanges();
    }


    [Fact]
    public async Task GetCurrentAvailabilities_ForExistingWorker_ReturnsOk()
    {
        ClearWorkers();

        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        SeedWorker(worker);

        var response = await _client.GetAsync($"api/worker/{worker.Id}/availability/current");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetCurrentAvailabilities_ForNonExistingWorker_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"api/worker/{-1}/availability/current");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllAvailabilities_ForExistingWorker_ReturnsOk()
    {
        ClearWorkers();

        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        SeedWorker(worker);

        var response = await _client.GetAsync($"api/worker/{worker.Id}/availability/all");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllAvailabilities_ForNonExistingWorker_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"api/worker/{-1}/availability/all");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task AddAvailability_ForExistingWorkerAndValidModel_ReturnsCreated()
    {
        ClearWorkers();

        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        SeedWorker(worker);

        var addAvailability = new AddAvailabilityDto()
        {
            Start = new DateTime(2030, 1, 1, 10, 0, 0),
            End = new DateTime(2030, 1, 1, 18, 0, 0)
        };

        var httpContent = addAvailability.ToJsonHttpContent();

        var response = await _client.PostAsync($"api/worker/{worker.Id}/availability", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task AddAvailability_ForExistingWorkerAndValidModelAndOverlappingDate_ReturnsCreated()
    {
        ClearWorkers();

        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        SeedWorker(worker);

        ClearAvailabilities();

        var availabilityToOverlap = new Availability()
        {
            Start = new DateTime(2030, 1, 1, 10, 0, 0),
            End = new DateTime(2030, 1, 1, 20, 0, 0),
            WorkerId = worker.Id
        };

        SeedAvailability(availabilityToOverlap);


        var addAvailability = new AddAvailabilityDto()
        {
            Start = new DateTime(2030, 1, 1, 10, 0, 0),
            End = new DateTime(2030, 1, 1, 18, 0, 0)
        };

        var httpContent = addAvailability.ToJsonHttpContent();

        var response = await _client.PostAsync($"api/worker/{worker.Id}/availability", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task AddAvailability_ForNonExistingWorkerAndValidModel_ReturnsNotFound()
    { 

        var addAvailability = new AddAvailabilityDto()
        {
            Start = new DateTime(2030, 1, 1, 10, 0, 0),
            End = new DateTime(2030, 1, 1, 18, 0, 0)
        };

        var httpContent = addAvailability.ToJsonHttpContent();

        var response = await _client.PostAsync($"api/worker/{-1}/availability", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddAvailability_InvalidModel_ReturnsBadRequest()
    {

        var addAvailability = new AddAvailabilityDto()
        {
            Start = new DateTime(2015, 1, 1, 10, 0, 1),
            End = new DateTime(2010, 1, 1, 18, 0, 2)
        };

        var httpContent = addAvailability.ToJsonHttpContent();

        var response = await _client.PostAsync($"api/worker/{-1}/availability", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task RemoveAvailability_ForExistingModelAndExistingWorker_ReturnsNoContent()
    {
        ClearWorkers();

        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        SeedWorker(worker);

        ClearAvailabilities();

        var availability = new Availability()
        {
            Start = new DateTime(2030, 1, 1, 10, 0, 0),
            End = new DateTime(2030, 1, 1, 20, 0, 0),
            WorkerId = worker.Id
        };

        SeedAvailability(availability);

        var response = await _client.DeleteAsync($"api/worker/{worker.Id}/availability/{availability.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task RemoveAvailability_ForNonExistingModelAndExistingWorker_ReturnsNotFound()
    {
        ClearWorkers();

        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        SeedWorker(worker);


        var response = await _client.DeleteAsync($"api/worker/{worker.Id}/availability/{-1}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task RemoveAvailability_ForNonExistingWorker_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync($"api/worker/{-1}/availability/{-1}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task UpdateAvailability_ForExistingWorkerAndExistingAvailabilityAndValidModel_ReturnsOk()
    {
        ClearWorkers();

        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        SeedWorker(worker);

        ClearAvailabilities();

        var availability = new Availability()
        {
            Start = new DateTime(2030, 1, 1, 10, 0, 0),
            End = new DateTime(2030, 1, 1, 20, 0, 0),
            WorkerId = worker.Id
        };

        SeedAvailability(availability);

        var updateAvailability = new UpdateAvailabilityDto()
        {
            Start = new DateTime(2030, 1, 1, 12, 0, 0),
            End = new DateTime(2030, 1, 1, 16, 0, 0),
        };


        var httpContent = updateAvailability.ToJsonHttpContent();


        var response = await _client.PutAsync($"api/worker/{worker.Id}/availability/{availability.Id}", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateAvailability_ForExistingWorkerAndExistingAvailabilityAndInvalidDateInModel_ReturnsBadRequest()
    {
        ClearWorkers();

        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        SeedWorker(worker);

        ClearAvailabilities();

        var availability = new Availability()
        {
            Start = new DateTime(2030, 1, 1, 10, 0, 0),
            End = new DateTime(2030, 1, 1, 20, 0, 0),
            WorkerId = worker.Id
        };

        SeedAvailability(availability);

        //day must be the same as updating entity

        var updateAvailability = new UpdateAvailabilityDto()
        {
            Start = new DateTime(2030, 1, 2, 12, 0, 0),
            End = new DateTime(2030, 1, 2, 16, 0, 0),
        };


        var httpContent = updateAvailability.ToJsonHttpContent();


        var response = await _client.PutAsync($"api/worker/{worker.Id}/availability/{availability.Id}", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task UpdateAvailability_ForExistingWorkerAndExistingAvailabilityAndInvalidModel_ReturnsBadRequest()
    {
        ClearWorkers();

        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        SeedWorker(worker);

        ClearAvailabilities();

        var availability = new Availability()
        {
            Start = new DateTime(2030, 1, 1, 10, 0, 0),
            End = new DateTime(2030, 1, 1, 20, 0, 0),
            WorkerId = worker.Id
        };

        SeedAvailability(availability);

        

        var updateAvailability = new UpdateAvailabilityDto()
        {
            Start = new DateTime(2015, 1, 1, 12, 0, 1),
            End = new DateTime(2011, 1, 1, 16, 1, 0),
        };


        var httpContent = updateAvailability.ToJsonHttpContent();


        var response = await _client.PutAsync($"api/worker/{worker.Id}/availability/{availability.Id}", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task UpdateAvailability_ForExistingWorkerAndNonExistingAvailability_ReturnsNotFound()
    {
        ClearWorkers();

        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        SeedWorker(worker);
        


        var updateAvailability = new UpdateAvailabilityDto()
        {
            Start = new DateTime(2030, 1, 2, 12, 0, 0),
            End = new DateTime(2030, 1, 2, 16, 0, 0),
        };


        var httpContent = updateAvailability.ToJsonHttpContent();


        var response = await _client.PutAsync($"api/worker/{worker.Id}/availability/{-1}", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateAvailability_NonExistingWorker_ReturnsNotFound()
    {
        
        var updateAvailability = new UpdateAvailabilityDto()
        {
            Start = new DateTime(2030, 1, 2, 12, 0, 0),
            End = new DateTime(2030, 1, 2, 16, 0, 0),
        };


        var httpContent = updateAvailability.ToJsonHttpContent();


        var response = await _client.PutAsync($"api/worker/{-1}/availability/{-1}", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddAvailabilityInPeriod_ForExistingWorkerAndValidModel_ReturnsCreated()
    {
        ClearWorkers();

        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        SeedWorker(worker);

        var model = new TimeRangeAvailabilityDto()
        {
            StartDate = new DateTime(2030, 1, 1, 12, 0, 0),
            EndDate = new DateTime(2030, 1, 7, 20, 0, 0)
        };

        var httpContent = model.ToJsonHttpContent();

        var response = await _client.PostAsync($"api/worker/{worker.Id}/availability/timeRange", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

    }

    [Fact]
    public async Task AddAvailabilityInPeriod_ForNonExistingWorkerAndValidModel_ReturnsNotFound()
    {
        var model = new TimeRangeAvailabilityDto()
        {
            StartDate = new DateTime(2030, 1, 1, 12, 0, 0),
            EndDate = new DateTime(2030, 1, 7, 20, 0, 0)
        };

        var httpContent = model.ToJsonHttpContent();

        var response = await _client.PostAsync($"api/worker/{-1}/availability/timeRange", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

    }

    [Fact]
    public async Task AddAvailabilityInPeriod_InvalidModel_ReturnsNotFound()
    {
        var model = new TimeRangeAvailabilityDto()
        {
            StartDate = new DateTime(2015, 2, 1, 12, 0, 0),
            EndDate = new DateTime(2010, 1, 7, 20, 0, 1)
        };

        var httpContent = model.ToJsonHttpContent();

        var response = await _client.PostAsync($"api/worker/{-1}/availability/timeRange", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    }

}