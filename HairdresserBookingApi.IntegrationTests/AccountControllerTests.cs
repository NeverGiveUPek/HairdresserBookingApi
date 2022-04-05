using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using HairdresserBookingApi.IntegrationTests.Helpers;
using HairdresserBookingApi.Models.Dto.User;
using HairdresserBookingApi.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace HairdresserBookingApi.IntegrationTests;

public class AccountControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly Mock<IAccountService> _accountServiceMock = new Mock<IAccountService>();

    public AccountControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_accountServiceMock.Object);
            });
        }).CreateClient();
    }


    [Fact]
    public async Task RegisterUser_ForValidModel_ReturnsOk()
    {
        var model = new CreateUserDto()
        {
            Email = "accountTest@test.com",
            Password = "TestPassword123",
            ConfirmPassword = "TestPassword123"
        };


        var httpContent = model.ToJsonHttpContent();

        var response = await _client.PostAsync("api/account/register", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RegisterUser_ForInvalidModel_ReturnsBadRequest()
    {
        var model = new CreateUserDto()
        {
            Email = "accountTest@test.com",
            Password = "test",
            ConfirmPassword = "test222"
        };


        var httpContent = model.ToJsonHttpContent();

        var response = await _client.PostAsync("api/account/register", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task LoginUser_ForRegisteredUser_ReturnsOk()
    {
        _accountServiceMock
            .Setup(e => e.GenerateUserJwt(It.IsAny<LoginUserDto>()))
            .Returns("jwtToken");

        var model = new LoginUserDto()
        {
            Email = "test@test",
            Password = "testPassword"
        };

        var httpContent = model.ToJsonHttpContent();

        var response = await _client.PostAsync("api/account/login", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task LoginUser_ForWrongUser_ReturnsBadRequest()
    {
        _accountServiceMock
            .Setup(e => e.GenerateUserJwt(It.IsAny<LoginUserDto>()))
            .Returns("jwtToken");

        var model = new LoginUserDto()
        {
            Email = "test"
        };

        var httpContent = model.ToJsonHttpContent();

        var response = await _client.PostAsync("api/account/login", httpContent);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task RemoveAccount_ForRegisteredUser_ReturnsNoContent()
    {
        var response = await _client.DeleteAsync($"api/account");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

}