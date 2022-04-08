using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.TestHelper;
using HairdresserBookingApi.Models.Dto.Availability;
using HairdresserBookingApi.Models.Validation.Validators.Availability;
using Xunit;

namespace HairdresserBookingApi.IntegrationTests.Validators.Availability;

public class TimeRangeAvailabilityDtoValidatorTests
{
    [Fact]
    public void Validate_ForCorrectModel_ReturnsSuccess()
    {
        var validator = new TimeRangeAvailabilityDtoValidator();

        var model = new TimeRangeAvailabilityDto()
        {
            StartDate = DateTime.Now.Date.AddHours(10),
            EndDate = DateTime.Now.Date.AddHours(18),
        };

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(GetIncorrectData))]
    public void Validate_ForIncorrectModel_ReturnsFailure(TimeRangeAvailabilityDto dto)
    {
        var validator = new TimeRangeAvailabilityDtoValidator();

        var result = validator.TestValidate(dto);

        result.ShouldHaveAnyValidationError();
    }

    public static IEnumerable<object[]> GetIncorrectData()
    {
        var list = new List<TimeRangeAvailabilityDto>()
        {
            new TimeRangeAvailabilityDto()
            {
                StartDate = DateTime.Now.Date.AddHours(-10),
                EndDate = DateTime.Now.Date.AddHours(18)
            },
            new TimeRangeAvailabilityDto()
            {
                StartDate = DateTime.Now.Date.AddHours(10),
                EndDate = DateTime.Now.Date.AddHours(5)
            },
            new TimeRangeAvailabilityDto()
            {
                StartDate = DateTime.Now.Date.AddHours(10).AddMinutes(1),
                EndDate = DateTime.Now.Date.AddHours(5)
            },
            new TimeRangeAvailabilityDto()
            {
                StartDate = DateTime.Now.Date.AddHours(10),
                EndDate = DateTime.Now.Date.AddHours(5).AddMinutes(1)
            },
            new TimeRangeAvailabilityDto()
            {
                StartDate = DateTime.Now.Date.AddHours(10)
            },
            new TimeRangeAvailabilityDto()
            {
                EndDate = DateTime.Now.Date.AddHours(10)
            },
            new TimeRangeAvailabilityDto()
            {
            }
        };

        return list.Select(l => new object[] {l});
    }
}