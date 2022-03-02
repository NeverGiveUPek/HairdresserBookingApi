using FluentValidation;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto.Availability;

namespace HairdresserBookingApi.Models.Validation.Validators;

public class AddAvailabilityDtoValidator : AbstractValidator<AddAvailabilityDto>
{
    public AddAvailabilityDtoValidator()
    {
        RuleFor(a => a.Start)
            .NotEmpty()
            .Custom((value, context) =>
            {
                if (!IsDateInFuture(value)) context.AddFailure("Date should be in future");
                if (!HasMinimumTimeSpanAsCertainMinutes(value, 5)) context.AddFailure("Minimum time span is 5 minutes");
            });

        RuleFor(a => a.End)
            .NotEmpty()
            .GreaterThan(a => a.Start)
            .Custom((value, context) =>
            {
                if (!IsDateInFuture(value)) context.AddFailure("Date should be in future");
                if (!HasMinimumTimeSpanAsCertainMinutes(value, 5)) context.AddFailure("Minimum time span is 5 minutes");
            });




    }


   

    private static bool IsDateInFuture(DateTime dateTime)
    {
        return dateTime.Date >= DateTime.Now.Date;
    }

    private static bool HasMinimumTimeSpanAsCertainMinutes(DateTime dateTime, int minuteSpan)
    {
        if (dateTime.Millisecond != 0) return false;
        if (dateTime.Second != 0) return false;
        if (dateTime.Minute % minuteSpan != 0) return false;
        return true;
    }
}