using System.Collections.Generic;
using System.Linq;
using FluentValidation.TestHelper;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto.User;
using HairdresserBookingApi.Models.Validation.Validators.User;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HairdresserBookingApi.IntegrationTests.Validators.User;

public class CreateUserDtoValidatorTests
{
    private readonly BookingDbContext _dbContext;

    public CreateUserDtoValidatorTests()
    {
        var builder = new DbContextOptionsBuilder<BookingDbContext>();
        builder.UseInMemoryDatabase("TestDatabase");
        _dbContext = new BookingDbContext(builder.Options);
        Seed();
    }

    [Fact]
    public void Validate_ForCorrectModel_ReturnsSuccess()
    {
        var model = new CreateUserDto()
        {
            Password = "Test1234",
            ConfirmPassword = "Test1234",
            Email = "test@test.com"
        };


        var validator = new CreateUserDtoValidator(_dbContext);

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(GetMissingPartData))]
    [MemberData(nameof(GetInvalidData))]
    public void Validate_ForIncorrectModel_ReturnsFailure(CreateUserDto dto)
    {
        var validator = new CreateUserDtoValidator(_dbContext);

        var result = validator.TestValidate(dto);

        result.ShouldHaveAnyValidationError();
    }


    public static IEnumerable<object[]> GetMissingPartData()
    {
        var list = new List<CreateUserDto>()
        {
            new CreateUserDto(),
            new CreateUserDto()
            {
                Email = "Test@test.com",
                Password = "test1234"
            },
            new CreateUserDto()
            {
                Password = "Test1234",
                ConfirmPassword = "Test1234"
            },
            new CreateUserDto()
            {
                Email = "test@test.com",
                ConfirmPassword = "Test1234"
            },
        };

        return list.Select(l => new object[] {l});
    }

    public static IEnumerable<object[]> GetInvalidData()
    {
        var list = new List<CreateUserDto>()
        {
            new CreateUserDto()
            {
                Email = "testEmail@test.com",
                Password = "Test1234",
                ConfirmPassword = "Test1234"
            },
            new CreateUserDto()
            {
                Email = "test@test.com",
                Password = "Test",
                ConfirmPassword = "Test"
            },
            new CreateUserDto()
            {
                Email = "test@test.com",
                Password = "Test12",
                ConfirmPassword = "Test12"
            },
            new CreateUserDto()
            {
                Email = "test@test.com",
                Password = "test1234",
                ConfirmPassword = "test1234"
            },
            new CreateUserDto()
            {
                Email = "test@test.com",
                Password = "TEST1234",
                ConfirmPassword = "TEST1234"
            },
            new CreateUserDto()
            {
                Email = "test@test.com",
                Password = "Test1234",
                ConfirmPassword = "Mest1234"
            },
        };

        return list.Select(l => new object[] {l});
    }

    private void Seed()
    {
        var testUser = new Models.Entities.Users.User()
        {
            Email = "testEmail@test.com",
            PasswordHash = "passwordHash"
        };
        _dbContext.Users.Add(testUser);
        _dbContext.SaveChanges();
    }
}