using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using HairdresserBookingApi.Models.Dto.Availability;
using HairdresserBookingApi.Models.Dto.User;
using HairdresserBookingApi.Models.Validation.Validators.Availability;
using Xunit;

namespace HairdresserBookingApi.IntegrationTests.Validators.Availability;

public class AddAvailabilityDtoValidatorTests
{
    [Fact]
    public void Validate_ForCorrectModel_ReturnsSuccess()
    {
        var validator = new AddAvailabilityDtoValidator();

        var model = new AddAvailabilityDto()
        {
            Start = DateTime.Now.Date.AddHours(10),
            End = DateTime.Now.Date.AddHours(18),
        };

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(GetIncorrectData))]
    public void Validate_ForIncorrectModel_ReturnsFailure(AddAvailabilityDto dto)
    {
        var validator = new AddAvailabilityDtoValidator();

        var result = validator.TestValidate(dto);

        result.ShouldHaveAnyValidationError();
    }

    public static IEnumerable<object[]> GetIncorrectData()
    {
        var list = new List<AddAvailabilityDto>()
        {
            new AddAvailabilityDto()
            {
                Start = DateTime.Now.Date.AddHours(-10),
                End = DateTime.Now.Date.AddHours(18)
            },
            new AddAvailabilityDto()
            {
                Start = DateTime.Now.Date.AddHours(10),
                End = DateTime.Now.Date.AddHours(5)
            },
            new AddAvailabilityDto()
            {
                Start = DateTime.Now.Date.AddHours(10).AddMinutes(1),
                End = DateTime.Now.Date.AddHours(5)
            },
            new AddAvailabilityDto()
            {
                Start = DateTime.Now.Date.AddHours(10),
                End = DateTime.Now.Date.AddHours(5).AddMinutes(1)
            },
            new AddAvailabilityDto()
            {
                Start = DateTime.Now.Date.AddHours(10),
                End = DateTime.Now.Date.AddHours(10).AddDays(1)
            },
            new AddAvailabilityDto()
            {
                Start = DateTime.Now.Date.AddHours(10)
            },
            new AddAvailabilityDto()
            {
                End = DateTime.Now.Date.AddHours(10)
            },
            new AddAvailabilityDto()
            {
            }
        };

        return list.Select(l => new object[] {l});
    }
}