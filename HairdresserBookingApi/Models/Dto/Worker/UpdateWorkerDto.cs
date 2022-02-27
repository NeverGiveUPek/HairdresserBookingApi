using System.ComponentModel.DataAnnotations;
using HairdresserBookingApi.Models.Validation.Attributes;

namespace HairdresserBookingApi.Models.Dto.Worker;

public class UpdateWorkerDto
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }


    [Required]
    [MaxLength(50)]
    [EmailAddress]
    [WorkerEmailUnique]
    public string Email { get; set; }

    [Required]
    [MaxLength(30)]
    [Phone]
    [WorkerPhoneNumberUnique]
    public string PhoneNumber { get; set; }
}