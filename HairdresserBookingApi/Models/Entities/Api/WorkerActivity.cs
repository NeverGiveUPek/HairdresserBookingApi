using System.ComponentModel.DataAnnotations;

namespace HairdresserBookingApi.Models.Entities.Api;

public class WorkerActivity
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
    public int ActivityId { get; set; }
    public virtual Activity Activity { get; set; }
}