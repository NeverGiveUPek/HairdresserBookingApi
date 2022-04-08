using FluentValidation;
using HairdresserBookingApi.Helpers;
using HairdresserBookingApi.Models.Dto.Availability;
using HairdresserBookingApi.Models.Dto.Helper;

namespace HairdresserBookingApi.Models.Validation.Validators.Availability;

public class TimeRangeAvailabilityDtoValidator : AbstractValidator<TimeRangeAvailabilityDto>
{
    public TimeRangeAvailabilityDtoValidator()
    {

        RuleFor(t => t.StartDate)
            .NotEmpty()
            .Custom((value, context) =>
            {
                if (!DateTimeHelper.IsDatePresent(value)) context.AddFailure("Date should be in future");
                if (!DateTimeHelper.HasMinimumTimeSpanAsCertainMinutes(value, 5))
                    context.AddFailure("Minimum time span is 5 minutes");
            });

        RuleFor(t => t.EndDate)
            .NotEmpty()
            .GreaterThan(a => a.StartDate)
            .Custom((value, context) =>
            {
                if (!DateTimeHelper.IsDatePresent(value)) context.AddFailure("Date should be in future");
                if (!DateTimeHelper.HasMinimumTimeSpanAsCertainMinutes(value, 5))
                    context.AddFailure("Minimum time span is 5 minutes");
            });


    }
}