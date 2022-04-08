using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.TestHelper;
using HairdresserBookingApi.Models.Dto.Availability;
using HairdresserBookingApi.Models.Dto.Helper;
using HairdresserBookingApi.Models.Dto.Reservation;
using HairdresserBookingApi.Models.Validation.Validators.Availability;
using HairdresserBookingApi.Models.Validation.Validators.Reservation;
using Xunit;

namespace HairdresserBookingApi.IntegrationTests.Validators.Reservation;

public class ReservationRequirementDtoValidatorTests
{
    [Fact]
    public void Validate_ForCorrectModel_ReturnsSuccess()
    {
        var validator = new ReservationRequirementDtoValidator();

        var model = new ReservationRequirementDto()
        {
            TimeRange = new TimeRange(DateTime.Now.Date.AddDays(1).AddHours(10),
                DateTime.Now.Date.AddDays(1).AddHours(18)),
            WorkerActivityId = 1
        };

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(GetIncorrectData))]
    public void Validate_ForIncorrectModel_ReturnsFailure(ReservationRequirementDto dto)
    {
        var validator = new ReservationRequirementDtoValidator();

        var result = validator.TestValidate(dto);

        result.ShouldHaveAnyValidationError();
    }

    public static IEnumerable<object[]> GetIncorrectData()
    {
        var list = new List<ReservationRequirementDto>()
        {
            new ReservationRequirementDto()
            {
                TimeRange = new TimeRange(DateTime.Now.Date.AddDays(-1).AddHours(10),
                    DateTime.Now.Date.AddDays(1).AddHours(18)),
                WorkerActivityId = 1
            },
            new ReservationRequirementDto()
            {
                TimeRange = new TimeRange(DateTime.Now.Date.AddDays(1).AddHours(10),
                    DateTime.Now.Date.AddDays(1).AddHours(5)),
                WorkerActivityId = 1
            },
            new ReservationRequirementDto()
            {
                TimeRange = new TimeRange(DateTime.Now.Date.AddDays(1).AddHours(10),
                    DateTime.Now.Date.AddDays(1).AddHours(18))
            },
            new ReservationRequirementDto()
            {
                TimeRange = new TimeRange
                {
                    StartDate = DateTime.Now.Date.AddDays(1).AddHours(5)
                },
                WorkerActivityId = 1
            },
            new ReservationRequirementDto()
            {
                TimeRange = new TimeRange
                {
                    EndDate = DateTime.Now.Date.AddDays(1).AddHours(5)
                },
                WorkerActivityId = 1
            },
            new ReservationRequirementDto()
            {
                TimeRange = new TimeRange
                {
                },
                WorkerActivityId = 1
            }
        };

        return list.Select(l => new object[] {l});
    }
}