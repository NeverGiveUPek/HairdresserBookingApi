using System.ComponentModel.DataAnnotations;

namespace HairdresserBookingApi.Models.Entities.Users;

public class User
{
    public int Id { get; set; }

    [EmailAddress] [Required] public string Email { get; set; }

    [MaxLength(50)] public string? FirstName { get; set; }

    [MaxLength(50)] public string? LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [Required] public string PasswordHash { get; set; }

    [Required] public int RoleId { get; set; }

    public virtual Role Role { get; set; }
}