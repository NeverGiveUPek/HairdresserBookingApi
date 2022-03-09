using FluentValidation;
using HairdresserBookingApi.Models.Db;
using HairdresserBookingApi.Models.Dto.User;

namespace HairdresserBookingApi.Models.Validation.Validators.User;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator(BookingDbContext dbContext)
    {
        RuleFor(u => u.FirstName)
            .MaximumLength(50);

        RuleFor(u => u.LastName)
            .MaximumLength(50);

        RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"[A-Z]+").WithMessage("Password must contain uppercase letter")
            .Matches(@"[a-z]+").WithMessage("Password must contain lowercase letter")
            .Matches(@"[0-9]+").WithMessage("Password must contain number");

        RuleFor(u => u.ConfirmPassword)
            .NotEmpty()
            .Equal(u => u.Password).WithMessage("Confirm password must be equal to password");

        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress()
            .Custom((value, context) =>
            {
                if (dbContext.Users.Any(x => x.Email == value))
                {
                    context.AddFailure("Email", "Email must be unique");
                }
            });









    }


}