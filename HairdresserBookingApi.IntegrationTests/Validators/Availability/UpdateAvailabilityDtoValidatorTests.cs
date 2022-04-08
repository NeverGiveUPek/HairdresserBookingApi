using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.TestHelper;
using HairdresserBookingApi.Models.Dto.Availability;
using HairdresserBookingApi.Models.Validation.Validators.Availability;
using Xunit;

namespace HairdresserBookingApi.IntegrationTests.Validators.Availability;

public class UpdateAvailabilityDtoValidatorTests
{
    [Fact]
    public void Validate_ForCorrectModel_ReturnsSuccess()
    {
        var validator = new UpdateAvailabilityDtoValidator();

        var model = new UpdateAvailabilityDto()
        {
            Start = DateTime.Now.Date.AddHours(10),
            End = DateTime.Now.Date.AddHours(18),
        };

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(GetIncorrectData))]
    public void Validate_ForIncorrectModel_ReturnsFailure(UpdateAvailabilityDto dto)
    {
        var validator = new UpdateAvailabilityDtoValidator();

        var result = validator.TestValidate(dto);

        result.ShouldHaveAnyValidationError();
    }

    public static IEnumerable<object[]> GetIncorrectData()
    {
        var list = new List<UpdateAvailabilityDto>()
        {
            
            new UpdateAvailabilityDto()
            {
                Start = DateTime.Now.Date.AddHours(10),
                End = DateTime.Now.Date.AddHours(5)
            },
            new UpdateAvailabilityDto()
            {
                Start = DateTime.Now.Date.AddHours(10).AddMinutes(1),
                End = DateTime.Now.Date.AddHours(5)
            },
            new UpdateAvailabilityDto()
            {
                Start = DateTime.Now.Date.AddHours(10),
                End = DateTime.Now.Date.AddHours(5).AddMinutes(1)
            },
            new UpdateAvailabilityDto()
            {
                Start = DateTime.Now.Date.AddHours(10),
                End = DateTime.Now.Date.AddHours(10).AddDays(1)
            },
            new UpdateAvailabilityDto()
            {
                Start = DateTime.Now.Date.AddHours(10)
            },
            new UpdateAvailabilityDto()
            {
                End = DateTime.Now.Date.AddHours(10)
            },
            new UpdateAvailabilityDto()
            {

            }
        };

        return list.Select(l => new object[] { l });
    }
}