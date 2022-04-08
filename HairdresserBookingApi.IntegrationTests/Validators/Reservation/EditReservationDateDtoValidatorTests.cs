using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.TestHelper;
using HairdresserBookingApi.Models.Dto.Reservation;
using HairdresserBookingApi.Models.Validation.Validators.Reservation;
using Xunit;

namespace HairdresserBookingApi.IntegrationTests.Validators.Reservation;

public class EditReservationDateDtoValidatorTests
{
    [Fact]
    public void Validate_ForCorrectModel_ReturnsSuccess()
    {
        var validator = new EditReservationDateDtoValidator();

        var model = new EditReservationDateDto()
        {
            Date = DateTime.Now.Date.AddDays(1).AddHours(10),
        };

        var result = validator.TestValidate(model);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(GetIncorrectData))]
    public void Validate_ForIncorrectModel_ReturnsFailure(EditReservationDateDto dto)
    {
        var validator = new EditReservationDateDtoValidator();

        var result = validator.TestValidate(dto);

        result.ShouldHaveAnyValidationError();
    }

    public static IEnumerable<object[]> GetIncorrectData()
    {
        var list = new List<EditReservationDateDto>()
        {
            new EditReservationDateDto()
            {
                Date = DateTime.Now.Date.AddDays(-1).AddHours(10),
            },
            new EditReservationDateDto()
            {
                Date = DateTime.Now.Date.AddDays(1).AddHours(10).AddMinutes(1),
            },
            new EditReservationDateDto()
            {
            }
        };

        return list.Select(l => new object[] {l});
    }
}