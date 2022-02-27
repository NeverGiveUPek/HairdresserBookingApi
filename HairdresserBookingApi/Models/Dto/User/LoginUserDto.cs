namespace HairdresserBookingApi.Models.Dto.User;

public class LoginUserDto
{
    //validation by LoginUserDtoValidator

    public string Email { get; set; }

    public string Password { get; set; }
}