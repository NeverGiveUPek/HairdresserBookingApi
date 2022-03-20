using FluentValidation;
using HairdresserBookingApi.Helpers;
using HairdresserBookingApi.Models.Dto.Reservation;

namespace HairdresserBookingApi.Models.Validation.Validators.Reservation;

public class EditReservationDateDtoValidator : AbstractValidator<EditReservationDateDto>
{
    public EditReservationDateDtoValidator()
    {
        RuleFor(e => e.Date)
            .NotEmpty()
            .Custom((value, context) =>
            {
                if (!DateTimeHelper.IsDatePresent(value)) context.AddFailure("Date should be in future");
                if (!DateTimeHelper.HasMinimumTimeSpanAsCertainMinutes(value, 5)) context.AddFailure("Minimum time span is 5 minutes");
            });


    }

}