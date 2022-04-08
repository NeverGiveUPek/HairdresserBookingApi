using System.Data;
using FluentValidation;
using HairdresserBookingApi.Helpers;
using HairdresserBookingApi.Models.Dto.Reservation;

namespace HairdresserBookingApi.Models.Validation.Validators.Reservation;

public class ReservationRequirementDtoValidator : AbstractValidator<ReservationRequirementDto>
{
    public ReservationRequirementDtoValidator()
    {
        RuleFor(x => x.TimeRange.StartDate)
            .NotEmpty()
            .Custom((value, context) =>
            {
                if (!DateTimeHelper.IsDateTimeInFuture(value)) context.AddFailure("Date should be in future");
            });


        RuleFor(x => x.TimeRange.EndDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.TimeRange.StartDate)
            .Custom((value, context) =>
            {
                if (!DateTimeHelper.IsDateTimeInFuture(value)) context.AddFailure("Date should be in future");
            });

        RuleFor(x => x.WorkerActivityId)
            .NotEmpty();
    }
}