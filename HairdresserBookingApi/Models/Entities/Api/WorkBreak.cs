using System.ComponentModel.DataAnnotations;

namespace HairdresserBookingApi.Models.Entities.Api;

public class WorkBreak
{
    [Required]
    public int Id { get; set; }

    [Required]
    public DateTime From { get; set; }
    [Required]
    public DateTime To { get; set; }

    public int WorkerAvailabilityId { get; set; }
    public virtual WorkerAvailability WorkerAvailability { get; set; }
}