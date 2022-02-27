using HairdresserBookingApi.Models.Dto.User;

namespace HairdresserBookingApi.Services.Interfaces;

public interface IAccountService
{
    void CreateUser(CreateUserDto dto);

    string GenerateUserJwt(LoginUserDto dto);
}