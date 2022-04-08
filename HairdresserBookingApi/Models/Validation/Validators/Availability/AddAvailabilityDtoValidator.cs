using FluentValidation;
using HairdresserBookingApi.Helpers;
using HairdresserBookingApi.Models.Dto.Availability;

namespace HairdresserBookingApi.Models.Validation.Validators.Availability;

public class AddAvailabilityDtoValidator : AbstractValidator<AddAvailabilityDto>
{
    public AddAvailabilityDtoValidator()
    {
        RuleFor(a => a.Start)
            .NotEmpty()
            .Custom((value, context) =>
            {
                if (!DateTimeHelper.IsDatePresent(value)) context.AddFailure("Date should be in future");
                if (!DateTimeHelper.HasMinimumTimeSpanAsCertainMinutes(value, 5))
                    context.AddFailure("Minimum time span is 5 minutes");
            });

        RuleFor(a => a.End)
            .NotEmpty()
            .GreaterThan(a => a.Start)
            .Custom((value, context) =>
            {
                if (!DateTimeHelper.IsDatePresent(value)) context.AddFailure("Date should be in future");
                if (!DateTimeHelper.HasMinimumTimeSpanAsCertainMinutes(value, 5))
                    context.AddFailure("Minimum time span is 5 minutes");
            });

        RuleFor(u => u.End.Date)
            .Equal(u => u.Start.Date)
            .WithMessage("Availability Start and End must be in same day");
    }
}