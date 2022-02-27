using FluentValidation;
using HairdresserBookingApi.Models.Dto.User;

namespace HairdresserBookingApi.Models.Validation.Validators;

public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDtoValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(u => u.Password)
            .NotEmpty();
    }
}