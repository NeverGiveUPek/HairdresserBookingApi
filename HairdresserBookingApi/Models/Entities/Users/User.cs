using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace HairdresserBookingApi.Models.Entities;

public class User
{ 

    public int Id { get; set; }
    
    [EmailAddress]
    [Required]
    public string Email { get; set; }

    [MaxLength(50)]
    public string? FirstName { get; set; }

    [MaxLength(50)]
    public string? LastName { get; set; }

    [MaxLength(30)]
    [Phone]
    public string? PhoneNumber { get; set; }
    
    public DateTime? DateOfBirth { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public int RoleId { get; set; }

    public virtual Role Role { get; set; }

}