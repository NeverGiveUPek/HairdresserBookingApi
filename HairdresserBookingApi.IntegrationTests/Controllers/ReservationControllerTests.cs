using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using HairdresserBookingApi.IntegrationTests.Helpers;
using HairdresserBookingApi.Models.Dto.Helper;
using HairdresserBookingApi.Models.Dto.Reservation;
using HairdresserBookingApi.Models.Entities.Api;
using Xunit;

namespace HairdresserBookingApi.IntegrationTests.Controllers;

public class ReservationControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public ReservationControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
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

    [Fact]
    public async Task GetAllPossibleTimesInDay_ForValidModelAndProperWorkerActivity_ReturnsOk()
    {
        var workerActivity = MakeWorkerActivity();

        var request = new ReservationRequestDto
        {
            Date = DateTime.Now.Date.AddDays(1),
            WorkerActivityId = workerActivity.Id
        };

        var response = await _client.GetAsync("api/reservation/day?" + request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllPossibleTimesInDay_ForValidModelAndNotProperWorkerActivity_ReturnsNotFound()
    {
        var request = new ReservationRequestDto
        {
            Date = DateTime.Now.Date.AddDays(1),
            WorkerActivityId = -1
        };

        var response = await _client.GetAsync("api/reservation/day?" + request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllPossibleTimesInDay_ForInvalidModel_ReturnsBadRequest()
    {
        var request = new ReservationRequestDto
        {
            Date = DateTime.Now.AddDays(-1),
            WorkerActivityId = -1
        };

        var response = await _client.GetAsync("api/reservation/day?" + request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task GetAllUserReservations_ReturnsOk()
    {
        var response = await _client.GetAsync("api/reservation/all");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetFutureUserReservations_ReturnsOk()
    {
        var response = await _client.GetAsync("api/reservation");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteReservation_ForExistingReservation_ReturnsNoContent()
    {
        var workerActivity = MakeWorkerActivity();

        var reservation = new Reservation
        {
            Date = DateTime.Now.Date.AddDays(1),
            WorkerActivityId = workerActivity.Id,
            UserId = 1
        };

        EntitySeeder.SeedReservation(reservation, _factory);

        var response = await _client.DeleteAsync($"api/reservation/{reservation.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteReservation_ForNonExistingReservation_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync($"api/reservation/{-1}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task EditReservation_ForExistingReservationAndValidModel_ReturnsOk()
    {
        var workerActivity = MakeWorkerActivity();

        var reservation = new Reservation
        {
            Date = DateTime.Now.Date.AddDays(1).AddHours(12),
            WorkerActivityId = workerActivity.Id,
            UserId = 1
        };

        EntitySeeder.SeedReservation(reservation, _factory);


        var availability = new Availability
        {
            WorkerId = workerActivity.WorkerId,
            Start = DateTime.Now.Date.AddDays(1).AddHours(10),
            End = DateTime.Now.Date.AddDays(1).AddHours(20)
        };

        EntitySeeder.SeedAvailability(availability, _factory);


        var editReservationDto = new EditReservationDateDto
        {
            Date = DateTime.Now.Date.AddDays(1).AddHours(14)
        };

        var httpContent = editReservationDto.ToJsonHttpContent();

        var response = await _client.PutAsync($"api/reservation/{reservation.Id}", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task EditReservation_ForExistingReservationAndNotAccessibleTime_ReturnsBadRequest()
    {
        var workerActivity = MakeWorkerActivity();

        var reservation = new Reservation
        {
            Date = DateTime.Now.Date.AddDays(1).AddHours(12),
            WorkerActivityId = workerActivity.Id,
            UserId = 1
        };

        EntitySeeder.SeedReservation(reservation, _factory);


        var editReservationDto = new EditReservationDateDto
        {
            Date = DateTime.Now.Date.AddDays(1).AddHours(14)
        };

        var httpContent = editReservationDto.ToJsonHttpContent();

        var response = await _client.PutAsync($"api/reservation/{reservation.Id}", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task EditReservation_NotValidModel_ReturnsBadRequest()
    {
        var reservation = new Reservation
        {
            Date = DateTime.Now.Date.AddDays(1).AddHours(12),
            WorkerActivityId = -1,
            UserId = 1
        };

        EntitySeeder.SeedReservation(reservation, _factory);

        var editReservationDto = new EditReservationDateDto
        {
            Date = DateTime.Now.Date.AddDays(-1).AddMinutes(1)
        };

        var httpContent = editReservationDto.ToJsonHttpContent();

        var response = await _client.PutAsync($"api/reservation/{reservation.Id}", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task EditReservation_ForNonExistingReservationAndValidModel_ReturnsBadRequest()
    {
        var editReservationDto = new EditReservationDateDto
        {
            Date = DateTime.Now.Date.AddDays(1).AddHours(14)
        };

        var httpContent = editReservationDto.ToJsonHttpContent();

        var response = await _client.PutAsync($"api/reservation/{-1}", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task EditReservation_ForWrongUser_ReturnsForbidden()
    {
        var reservation = new Reservation
        {
            Date = DateTime.Now.Date.AddDays(1).AddHours(12),
            WorkerActivityId = -1,
            UserId = 2 //correct id is 1
        };

        EntitySeeder.SeedReservation(reservation, _factory);

        var editReservationDto = new EditReservationDateDto
        {
            Date = DateTime.Now.Date.AddDays(1).AddHours(14)
        };

        var httpContent = editReservationDto.ToJsonHttpContent();

        var response = await _client.PutAsync($"api/reservation/{reservation.Id}", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PickReservation_ForValidModelAndAccessibleTime_ReturnsOk()
    {
        var workerActivity = MakeWorkerActivity();

        var timeRange = new TimeRange(DateTime.Now.Date.AddDays(1), DateTime.Now.Date.AddDays(7));

        var requirementDto = new ReservationRequirementDto
        {
            TimeRange = timeRange,
            WorkerActivityId = workerActivity.Id
        };


        var availability = new Availability
        {
            WorkerId = workerActivity.WorkerId,
            Start = DateTime.Now.Date.AddDays(2).AddHours(10),
            End = DateTime.Now.Date.AddDays(2).AddHours(20)
        };

        EntitySeeder.SeedAvailability(availability, _factory);


        var response = await _client.GetAsync("api/reservation/pick?" + requirementDto);

        //WorkerActivityId=3&TimeRange.StartDate=2022-03-25T00:00:00.000Z&TimeRange.EndDate=2022-03-28T00:00:00.000Z&PickStrategy=1
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PickReservation_ForValidModelAndNotAccessibleTime_ReturnsBadRequest()
    {
        var workerActivity = MakeWorkerActivity();

        var timeRange = new TimeRange(DateTime.Now.Date.AddDays(1), DateTime.Now.Date.AddDays(7));

        var requirementDto = new ReservationRequirementDto
        {
            TimeRange = timeRange,
            WorkerActivityId = workerActivity.Id
        };


        var response = await _client.GetAsync("api/reservation/pick?" + requirementDto);

        //WorkerActivityId=3&TimeRange.StartDate=2022-03-25T00:00:00.000Z&TimeRange.EndDate=2022-03-28T00:00:00.000Z&PickStrategy=1
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PickReservation_ForInvalidModel_ReturnsBadRequest()
    {
        var timeRange = new TimeRange(DateTime.Now.Date.AddDays(-1), DateTime.Now.Date.AddDays(-7));

        var requirementDto = new ReservationRequirementDto
        {
            TimeRange = timeRange,
            WorkerActivityId = -1
        };


        var response = await _client.GetAsync("api/reservation/pick?" + requirementDto);

        //WorkerActivityId=3&TimeRange.StartDate=2022-03-25T00:00:00.000Z&TimeRange.EndDate=2022-03-28T00:00:00.000Z&PickStrategy=1
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task MakeReservation_ForValidModel_ReturnsCreated()
    {

        var workerActivity = MakeWorkerActivity();

        var availability = new Availability
        {
            WorkerId = workerActivity.WorkerId,
            Start = DateTime.Now.Date.AddDays(1).AddHours(10),
            End = DateTime.Now.Date.AddDays(1).AddHours(20)
        };

        EntitySeeder.SeedAvailability(availability, _factory);


        var reservationRequest = new ReservationRequestDto
        {
            Date = DateTime.Now.Date.AddDays(1).AddHours(15),
            WorkerActivityId = workerActivity.Id
        };

        var httpContent = reservationRequest.ToJsonHttpContent();

        EntityRemover.RemoveUserReservations(1, _factory);


        var response = await _client.PostAsync("api/reservation", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task MakeReservation_ForNotAccessibleTime_ReturnsBadRequest()
    {
        var workerActivity = MakeWorkerActivity();


        var reservationRequest = new ReservationRequestDto
        {
            Date = DateTime.Now.Date.AddDays(1).AddHours(15),
            WorkerActivityId = workerActivity.Id
        };

        var httpContent = reservationRequest.ToJsonHttpContent();

        var response = await _client.PostAsync("api/reservation", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task MakeReservation_ForInvalidModel_ReturnsBadRequest()
    {
        var reservationRequest = new ReservationRequestDto
        {
            Date = DateTime.Now.Date.AddDays(-2)
        };

        var httpContent = reservationRequest.ToJsonHttpContent();

        var response = await _client.PostAsync("api/reservation", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task MakeReservation_ForNotActiveWorkerActivity_ReturnsBadRequest()
    {
        var worker = TestWorker;

        EntitySeeder.SeedWorker(worker, _factory);

        var activity = TestActivity;

        EntitySeeder.SeedActivity(activity, _factory);

        var workerActivity = new WorkerActivity
        {
            Price = 100,
            RequiredMinutes = 50,
            ActivityId = activity.Id,
            WorkerId = worker.Id,
            IsActive = false
        };

        EntitySeeder.SeedWorkerActivity(workerActivity, _factory);


        var availability = new Availability
        {
            WorkerId = workerActivity.WorkerId,
            Start = DateTime.Now.Date.AddDays(1).AddHours(10),
            End = DateTime.Now.Date.AddDays(1).AddHours(20)
        };

        EntitySeeder.SeedAvailability(availability, _factory);


        var reservationRequest = new ReservationRequestDto
        {
            Date = DateTime.Now.Date.AddDays(1).AddHours(15),
            WorkerActivityId = workerActivity.Id
        };

        var httpContent = reservationRequest.ToJsonHttpContent();

        var response = await _client.PostAsync("api/reservation", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task MakeReservation_ForToManyActiveReservations_ReturnsForbidden()
    {
        
        var workerActivity = MakeWorkerActivity();

        var availability = new Availability
        {
            WorkerId = workerActivity.WorkerId,
            Start = DateTime.Now.Date.AddDays(1).AddHours(10),
            End = DateTime.Now.Date.AddDays(1).AddHours(20)
        };

        EntitySeeder.SeedAvailability(availability, _factory);


        MakeReservations(5, DateTime.Now.AddDays(1).AddHours(10), _factory);


        var reservationRequest = new ReservationRequestDto
        {
            Date = DateTime.Now.Date.AddDays(1).AddHours(15),
            WorkerActivityId = workerActivity.Id
        };

        var httpContent = reservationRequest.ToJsonHttpContent();

        var response = await _client.PostAsync("api/reservation", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }


    private WorkerActivity MakeWorkerActivity()
    {
        var worker = TestWorker;

        EntitySeeder.SeedWorker(worker, _factory);

        var activity = TestActivity;

        EntitySeeder.SeedActivity(activity, _factory);

        var entity = new WorkerActivity
        {
            Price = 100,
            RequiredMinutes = 30,
            ActivityId = activity.Id,
            WorkerId = worker.Id
        };

        EntitySeeder.SeedWorkerActivity(entity, _factory);

        return entity;
    }

    private static void MakeReservations(int amount, DateTime dateTime, CustomWebApplicationFactory<Program> factory)
    {
        for (var i = 0; i < amount; i++)
        {
            var reservation = new Reservation
            {
                Date = dateTime.AddHours(30 * i),
                UserId = 1,
                WorkerActivityId = 1
            };

            EntitySeeder.SeedReservation(reservation, factory);
        }
    }
}