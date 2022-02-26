using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using HairdresserBookingApi.IntegrationTests.Helpers;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto.Activity;
using HairdresserBookingApi.Models.Entities.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;


namespace HairdresserBookingApi.IntegrationTests;

public class ActivityControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;


    public ActivityControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
            {
                var dbContextOptions = services.SingleOrDefault(service =>
                    service.ServiceType == typeof(DbContextOptions<BookingDbContext>));
                services.Remove(dbContextOptions);

                services.AddDbContext<BookingDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));

            }));
        _client = _factory.CreateClient();

    }
    

    private void SeedActivity(Activity activity)
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<BookingDbContext>();

        dbContext.Activities.Add(activity);
        dbContext.SaveChanges();
    }


    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/activity");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllAvailable_ReturnsOK()
    {
        var response = await _client.GetAsync("api/activity/available");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_ForExistingEntity_ReturnsOk()
    {
        var model = new Activity()
        {
            Name = "Test",
            Description = "Description test",
            IsForMan = true
        };

        SeedActivity(model);

        var response = await _client.GetAsync($"api/activity/{model.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }


    [Fact]
    public async Task GetById_ForNonExistingEntity_ReturnsNotFound(){
    
        var response = await _client.GetAsync($"api/activity/{-1}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]  
    public async Task Create_ForValidData_ReturnsCreated()
    {
        var model = new CreateActivityDto()
        {
            Name = "Test model",
            Description = "Description test ",
            IsForMan = true
        };

        var httpContent = model.ToJsonHttpContent();

        var response = await _client.PostAsync("api/activity", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    [Fact]
    public async Task Create_ForInvalidData_ReturnsBadRequest()
    {
        var model = new CreateActivityDto()
        {
            Description = "Description test",
            IsForMan = true
        };

        var httpContent = model.ToJsonHttpContent();


        var response = await _client.PostAsync("api/activity", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_ForDoubleEntity_ReturnsBadRequest()
    {
        var firstModel = new Activity()
        {
            Name = "Double create test",
            IsForMan = true,
            Description = "First"
        };

        var secondModel = new CreateActivityDto()
        {
            Name = "Double create test",
            IsForMan = true,
            Description = "Second"
        };

        SeedActivity(firstModel);

        var httpContent = secondModel.ToJsonHttpContent();
        
        var response = await _client.PostAsync("api/activity", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Delete_ForValidId_ReturnsNoContent()
    {
        var model = new Activity()
        {
            Name = "test",
            IsForMan = true,
            Description = "First"
        };

        SeedActivity(model);

        var response = await _client.DeleteAsync($"api/activity/{model.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_ForInValidId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync($"api/activity/{-1}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_ForValidModelAndExistingId_ReturnsOk()
    {
        var modelToUpdate = new Activity()
        {
            Name = "test to update success",
            IsForMan = true,
            Description = "First"
        };
        
        SeedActivity(modelToUpdate);


        var updateDto = new UpdateActivityDto()
        {
            Name = "Change test",
            Description = "Change description test ",
            IsForMan = false
        };

        var httpContent = updateDto.ToJsonHttpContent();

        var response = await _client.PutAsync($"api/activity/{modelToUpdate.Id}", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Update_ForValidModelAndNonExistingId_ReturnsNotFound()
    {
        
        var updateDto = new UpdateActivityDto()
        {
            Name = "Change test",
            Description = "Change description test ",
            IsForMan = false
        };

        var httpContent = updateDto.ToJsonHttpContent();

        var response = await _client.PutAsync($"api/activity/{-1}", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_ForInValidModelAndExistingId_ReturnsBadRequest()
    {
        var modelToUpdate = new Activity()
        {
            Name = "test to update fail",
            IsForMan = true,
            Description = "First"
        };

        SeedActivity(modelToUpdate);


        //lack of required description
        var updateDto = new UpdateActivityDto()
        {
            Name = "Change test",
            IsForMan = false
        };


        var httpContent = updateDto.ToJsonHttpContent();

        var response = await _client.PutAsync($"api/activity/{modelToUpdate.Id}", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


}