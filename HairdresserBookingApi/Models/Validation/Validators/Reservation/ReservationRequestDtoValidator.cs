using FluentValidation;
using HairdresserBookingApi.Helpers;
using HairdresserBookingApi.Models.Dto.Reservation;

namespace HairdresserBookingApi.Models.Validation.Validators.Reservation;

public class ReservationRequestDtoValidator : AbstractValidator<ReservationRequestDto>
{
    public ReservationRequestDtoValidator()
    {
        RuleFor(r => r.WorkerActivityId)
            .NotEmpty();

        RuleFor(r => r.Date)
            .NotEmpty()
            .Custom((value, context) =>
            {
                //if (!DateTimeHelper.IsDateInFuture(value)) context.AddFailure("Date should be in future");
                if (!DateTimeHelper.HasMinimumTimeSpanAsCertainMinutes(value, 5)) context.AddFailure("Minimum time span is 5 minutes");
            });

    }

}