using System.ComponentModel.DataAnnotations;

namespace HairdresserBookingApi.Models.Entities.Api;

public class Worker
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    [Required]
    [MaxLength(50)]
    public string Email { get; set; }

    [Required]
    [MaxLength(30)]
    [Phone]
    public string PhoneNumber { get; set; }

    public virtual ICollection<WorkerService> WorkerServices { get; set; }





}