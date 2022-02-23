using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;


namespace HairdresserBookingApi.IntegrationTests;

public class ActivityControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;


    public ActivityControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }


    [Fact]
    public async Task GetAll_ReturnsOkResult()
    {
        var response = await _client.GetAsync("/api/activity");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }




}