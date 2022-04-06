using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using HairdresserBookingApi.IntegrationTests.Helpers;
using HairdresserBookingApi.Models.Dto.WorkerActivity;
using HairdresserBookingApi.Models.Entities.Api;
using Xunit;

namespace HairdresserBookingApi.IntegrationTests;

public class WorkerActivityControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public WorkerActivityControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }


    [Fact]
    public async Task GetAllActivitiesOfWorker_ForExistingWorker_ReturnsOk()
    {
        var worker = new Worker()
        {
            FirstName = "test",
            LastName = "test",
            Email = "test@test.com",
            PhoneNumber = "111222123"
        };

        EntitySeeder.SeedWorker(worker,_factory);

        var response = await _client.GetAsync($"api/workerActivity/worker/{worker.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllActivitiesOfWorker_ForNonExistingWorker_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"api/workerActivity/worker/{-1}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task GetAllWorkersOfActivity_ForExistingActivity_ReturnsOk()
    {
        var activity = TestActivity;

        EntitySeeder.SeedActivity(activity, _factory);

        var response = await _client.GetAsync($"api/workerActivity/activity/{activity.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }


    [Fact]
    public async Task GetAllWorkersOfActivity_ForNonExistingActivity_ReturnsNotFound()
    {
        
        var response = await _client.GetAsync($"api/workerActivity/activity/{-1}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task GetWorkerActivity_ForExistingWorkerActivity_ReturnsOk()
    {
        var worker = TestWorker;

        EntitySeeder.SeedWorker(worker, _factory);

        var activity = TestActivity;
        
        EntitySeeder.SeedActivity(activity, _factory);

        var workerActivity = new WorkerActivity()
        {
            Price = 100,
            RequiredMinutes = 30,
            WorkerId = worker.Id,
            ActivityId = activity.Id
        };
        
        EntitySeeder.SeedWorkerActivity(workerActivity, _factory);

        var response = await _client.GetAsync($"api/workerActivity/{workerActivity.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

    }


    [Fact]
    public async Task GetWorkerActivity_ForNonExistingWorkerActivity_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"api/workerActivity/{-1}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

    }


    [Fact]
    public async Task CreateWorkerActivity_ForValidModel_ReturnsCreated()
    {
        var worker = TestWorker;

        EntitySeeder.SeedWorker(worker, _factory);

        var activity = TestActivity;

        EntitySeeder.SeedActivity(activity, _factory);

        var model = new CreateWorkerActivityDto()
        {
            Price = 100,
            RequiredMinutes = 50,
            ActivityId = activity.Id,
            WorkerId = worker.Id
        };

        var httpContent = model.ToJsonHttpContent();

        var response = await _client.PostAsync($"api/workerActivity", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

    }


    [Fact]
    public async Task CreateWorkerActivity_ForInvalidModel_ReturnsBadRequest()
    {

        var model = new CreateWorkerActivityDto();
       

        var httpContent = model.ToJsonHttpContent();

        var response = await _client.PostAsync($"api/workerActivity", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    }

    [Fact]
    public async Task CreateWorkerActivity_ForNonExistingWorker_ReturnsNotFound()
    {
        var activity = TestActivity;

        EntitySeeder.SeedActivity(activity, _factory);

        var model = new CreateWorkerActivityDto()
        {
            Price = 100,
            RequiredMinutes = 50,
            ActivityId = activity.Id,
            WorkerId = -1
        };

        var httpContent = model.ToJsonHttpContent();

        var response = await _client.PostAsync($"api/workerActivity", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateWorkerActivity_ForNonExistingActivity_ReturnsNotFound()
    {
        var worker = TestWorker;

        EntitySeeder.SeedWorker(worker, _factory);

        var model = new CreateWorkerActivityDto()
        {
            Price = 100,
            RequiredMinutes = 50,
            ActivityId = -1,
            WorkerId = worker.Id
        };

        var httpContent = model.ToJsonHttpContent();

        var response = await _client.PostAsync($"api/workerActivity", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateWorkerActivity_SameEntity_ReturnsBadRequest()
    {
        var worker = TestWorker;

        EntitySeeder.SeedWorker(worker, _factory);

        var activity = TestActivity;

        EntitySeeder.SeedActivity(activity, _factory);

        var entity = new WorkerActivity()
        {
            Price = 100,
            RequiredMinutes = 50,
            ActivityId = activity.Id,
            WorkerId = worker.Id
        };

        EntitySeeder.SeedWorkerActivity(entity, _factory);


        var model = new CreateWorkerActivityDto()
        {
            Price = 100,
            RequiredMinutes = 50,
            ActivityId = activity.Id,
            WorkerId = worker.Id
        };

        var httpContent = model.ToJsonHttpContent();

        var response = await _client.PostAsync($"api/workerActivity", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task Delete_ForExistingWorkerActivityWithoutFutureReservation_ReturnsNoContent()
    {
        var worker = TestWorker;

        EntitySeeder.SeedWorker(worker, _factory);

        var activity = TestActivity;

        EntitySeeder.SeedActivity(activity, _factory);

        var entity = new WorkerActivity()
        {
            Price = 100,
            RequiredMinutes = 50,
            ActivityId = activity.Id,
            WorkerId = worker.Id
        };

        EntitySeeder.SeedWorkerActivity(entity, _factory);

        var response = await _client.DeleteAsync($"api/workerActivity/{entity.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ForExistingWorkerActivityWithFutureReservation_ReturnsBadRequest()
    {
        var worker = TestWorker;

        EntitySeeder.SeedWorker(worker, _factory);

        var activity = TestActivity;

        EntitySeeder.SeedActivity(activity, _factory);

        var entity = new WorkerActivity()
        {
            Price = 100,
            RequiredMinutes = 50,
            ActivityId = activity.Id,
            WorkerId = worker.Id
        };

        EntitySeeder.SeedWorkerActivity(entity, _factory);

        var reservation = new Reservation()
        {
            Date = DateTime.Now,
            WorkerActivityId = entity.Id
        };

        EntitySeeder.SeedReservation(reservation, _factory);


        var response = await _client.DeleteAsync($"api/workerActivity/{entity.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task Delete_ForNonExistingWorkerActivity_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync($"api/workerActivity/{-1}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Deactivate_ForExistingWorkerActivity_ReturnsNoContent()
    {
        var worker = TestWorker;

        EntitySeeder.SeedWorker(worker, _factory);

        var activity = TestActivity;

        EntitySeeder.SeedActivity(activity, _factory);

        var entity = new WorkerActivity()
        {
            Price = 100,
            RequiredMinutes = 50,
            ActivityId = activity.Id,
            WorkerId = worker.Id
        };

        EntitySeeder.SeedWorkerActivity(entity, _factory);

        var response = await _client.PutAsync($"api/workerActivity/{entity.Id}/deactivate", null);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Deactivate_ForNonExistingWorkerActivity_ReturnsNoContent()
    {
        
        var response = await _client.PutAsync($"api/workerActivity/{-1}/deactivate", null);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Activate_ForExistingWorkerActivity_ReturnsNoContent()
    {
        var worker = TestWorker;

        EntitySeeder.SeedWorker(worker, _factory);

        var activity = TestActivity;

        EntitySeeder.SeedActivity(activity, _factory);

        var entity = new WorkerActivity()
        {
            Price = 100,
            RequiredMinutes = 50,
            ActivityId = activity.Id,
            WorkerId = worker.Id
        };

        EntitySeeder.SeedWorkerActivity(entity, _factory);

        var response = await _client.PutAsync($"api/workerActivity/{entity.Id}/activate", null);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Activate_ForNonExistingWorkerActivity_ReturnsNoContent()
    {

        var response = await _client.PutAsync($"api/workerActivity/{-1}/deactivate", null);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }




    private static Worker TestWorker => new()
    {
        FirstName = "worker",
        LastName = "worker",
        Email = "workerEmail@email",
        PhoneNumber = "765765765"
    };

    private static Activity TestActivity => new()
    {
        Name = "test",
        IsForMan = true,
        Description = "First"
    };
}