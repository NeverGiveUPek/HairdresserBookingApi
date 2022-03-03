using FluentValidation;
using HairdresserBookingApi.Helpers;
using HairdresserBookingApi.Models.Dto.Activity;
using HairdresserBookingApi.Models.Dto.Availability;

namespace HairdresserBookingApi.Models.Validation.Validators;

public class UpdateAvailabilityDtoValidator : AbstractValidator<UpdateAvailabilityDto>
{
    public UpdateAvailabilityDtoValidator()
    {
        RuleFor(u => u.Start)
            .NotEmpty()
            .Custom((value, context) =>
            {
                if (!DateTimeHelper.HasMinimumTimeSpanAsCertainMinutes(value, 5)) context.AddFailure("Minimum time span is 5 minutes");
            });

        RuleFor(u => u.End)
            .NotEmpty()
            .GreaterThan(a => a.Start)
            .Custom((value, context) =>
            {
                if (!DateTimeHelper.HasMinimumTimeSpanAsCertainMinutes(value, 5)) context.AddFailure("Minimum time span is 5 minutes");
            });

        RuleFor(u => u.End.Day)
            .Equal(u => u.Start.Day)
            .WithMessage("Availability Start and End must be in same day");

    }
}