using System.ComponentModel.DataAnnotations;
using HairdresserBookingApi.Models.Entities;

namespace HairdresserBookingApi.Models.Dto.User;

public class CreateUserDto
{
    //validation by CreateUserDtoValidator

    public string Email { get; set; }
    
    public string Password { get; set; }

    public string ConfirmPassword { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }

}