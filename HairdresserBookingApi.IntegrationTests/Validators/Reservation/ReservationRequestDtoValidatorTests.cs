using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.TestHelper;
using HairdresserBookingApi.Models.Dto.Availability;
using HairdresserBookingApi.Models.Dto.Reservation;
using HairdresserBookingApi.Models.Validation.Validators.Availability;
using HairdresserBookingApi.Models.Validation.Validators.Reservation;
using Xunit;

namespace HairdresserBookingApi.IntegrationTests.Validators.Reservation;

public class ReservationRequestDtoValidatorTests
{
    [Fact]
    public void Validate_ForCorrectModel_ReturnsSuccess()
    {
        var validator = new ReservationRequestDtoValidator();

        var model = new ReservationRequestDto()
        {
            Date = DateTime.Now.Date.AddDays(1).AddHours(10),
            WorkerActivityId = 1
        };

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(GetIncorrectData))]
    public void Validate_ForIncorrectModel_ReturnsFailure(ReservationRequestDto dto)
    {
        var validator = new ReservationRequestDtoValidator();

        var result = validator.TestValidate(dto);

        result.ShouldHaveAnyValidationError();
    }

    public static IEnumerable<object[]> GetIncorrectData()
    {
        var list = new List<ReservationRequestDto>()
        {
            new ReservationRequestDto()
            {
                Date = DateTime.Now.Date.AddDays(-1).AddHours(10),
                WorkerActivityId = 1
            },
            new ReservationRequestDto()
            {
                Date = DateTime.Now.Date.AddDays(1).AddHours(10).AddMinutes(1),
                WorkerActivityId = 1
            },
            new ReservationRequestDto()
            {
                Date = DateTime.Now.Date.AddDays(1).AddHours(10),
            },
            new ReservationRequestDto()
            {
                WorkerActivityId = 1
            },
            new ReservationRequestDto()
            {
                
            },
        };

        return list.Select(l => new object[] { l });
    }
}