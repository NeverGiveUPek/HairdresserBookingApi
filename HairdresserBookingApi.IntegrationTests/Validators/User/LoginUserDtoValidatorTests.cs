using System.Collections.Generic;
using System.Linq;
using FluentValidation.TestHelper;
using HairdresserBookingApi.Models.Dto.User;
using HairdresserBookingApi.Models.Validation.Validators.User;
using Xunit;

namespace HairdresserBookingApi.IntegrationTests.Validators.User;

public class LoginUserDtoValidatorTests
{
    public static IEnumerable<object[]> GetSampleInvalidData()
    {
        var list = new List<LoginUserDto>()
        {
            new LoginUserDto(),
            new LoginUserDto()
            {
                Email = "test",
                Password = "test"
            },
            new LoginUserDto()
            {
                Email = "test@test"
            },
            new LoginUserDto()
            {
                Password = "test"
            }
        };

        return list.Select(l => new object[] {l});
    }


    [Fact]
    public void Validate_ForCorrectModel_ReturnsSuccess()
    {
        var validator = new LoginUserDtoValidator();

        var model = new LoginUserDto()
        {
            Email = "test@test.com",
            Password = "password"
        };

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(GetSampleInvalidData))]
    public void Validate_ForIncorrectModel_ReturnsFailure(LoginUserDto dto)
    {
        var validator = new LoginUserDtoValidator();

        var result = validator.TestValidate(dto);

        result.ShouldHaveAnyValidationError();
    }
}