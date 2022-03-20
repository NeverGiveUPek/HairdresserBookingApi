using HairdresserBookingApi.Models.Dto.User;

namespace HairdresserBookingApi.Services.Interfaces;

public interface IAccountService
{
    void CreateUser(CreateUserDto dto);

    string GenerateUserJwt(LoginUserDto dto);

    void ChangeRole(string role, int id);

    void RemoveAccount();
}