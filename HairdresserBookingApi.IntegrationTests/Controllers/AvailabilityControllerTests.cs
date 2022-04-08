using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using HairdresserBookingApi.IntegrationTests.Helpers;
using HairdresserBookingApi.Models.Dto.Availability;
using HairdresserBookingApi.Models.Entities.Api;
using Xunit;

namespace HairdresserBookingApi.IntegrationTests.Controllers;

public class AvailabilityControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;


    public AvailabilityControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }


    [Fact]
    public async Task GetCurrentAvailabilities_ForExistingWorker_ReturnsOk()
    {
        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        EntitySeeder.SeedWorker(worker, _factory);

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
        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        EntitySeeder.SeedWorker(worker, _factory);

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
        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        EntitySeeder.SeedWorker(worker, _factory);

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
        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        EntitySeeder.SeedWorker(worker, _factory);


        var availabilityToOverlap = new Availability()
        {
            Start = new DateTime(2030, 1, 1, 10, 0, 0),
            End = new DateTime(2030, 1, 1, 20, 0, 0),
            WorkerId = worker.Id
        };

        EntitySeeder.SeedAvailability(availabilityToOverlap, _factory);


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
        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        EntitySeeder.SeedWorker(worker, _factory);


        var availability = new Availability()
        {
            Start = new DateTime(2030, 1, 1, 10, 0, 0),
            End = new DateTime(2030, 1, 1, 20, 0, 0),
            WorkerId = worker.Id
        };

        EntitySeeder.SeedAvailability(availability, _factory);

        var response = await _client.DeleteAsync($"api/worker/{worker.Id}/availability/{availability.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task RemoveAvailability_ForNonExistingModelAndExistingWorker_ReturnsNotFound()
    {
        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        EntitySeeder.SeedWorker(worker, _factory);


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
        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        EntitySeeder.SeedWorker(worker, _factory);


        var availability = new Availability()
        {
            Start = new DateTime(2030, 1, 1, 10, 0, 0),
            End = new DateTime(2030, 1, 1, 20, 0, 0),
            WorkerId = worker.Id
        };

        EntitySeeder.SeedAvailability(availability, _factory);

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
    public async Task
        UpdateAvailability_ForExistingWorkerAndExistingAvailabilityAndInvalidDateInModel_ReturnsBadRequest()
    {
        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        EntitySeeder.SeedWorker(worker, _factory);


        var availability = new Availability()
        {
            Start = new DateTime(2030, 1, 1, 10, 0, 0),
            End = new DateTime(2030, 1, 1, 20, 0, 0),
            WorkerId = worker.Id
        };

        EntitySeeder.SeedAvailability(availability, _factory);

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
        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        EntitySeeder.SeedWorker(worker, _factory);


        var availability = new Availability()
        {
            Start = new DateTime(2030, 1, 1, 10, 0, 0),
            End = new DateTime(2030, 1, 1, 20, 0, 0),
            WorkerId = worker.Id
        };

        EntitySeeder.SeedAvailability(availability, _factory);


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
        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        EntitySeeder.SeedWorker(worker, _factory);


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
        var worker = new Worker()
        {
            FirstName = "AvailabilityTest",
            LastName = "AvailabilityTest",
            Email = "Availability@test.com",
            PhoneNumber = "456456456"
        };

        EntitySeeder.SeedWorker(worker, _factory);

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