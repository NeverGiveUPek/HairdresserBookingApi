using System.ComponentModel.DataAnnotations;

namespace HairdresserBookingApi.Models.Entities.Api;

public class WorkerService
{
    [Required]
    [Key]
    public int Id { get; set; }
    [Required]
    public double Price { get; set; }
    [Required]
    public int RequiredMinutes { get; set; }
    
    [Required]
    public int WorkerId { get; set; }
    public virtual Worker Worker { get; set; }
    
    [Required]
    public int ServiceId { get; set; }
    public virtual Service Service { get; set; }
}