using System.ComponentModel.DataAnnotations;

namespace HairdresserBookingApi.Models.Entities.Api;

public class Reservation
{
    [Required]
    public int Id { get; set; }
    [Required]
    public DateTime Date { get; set; }

    [Required]
    public int UserId { get; set; }
    public virtual User User { get; set; }

    [Required]
    public int WorkerServiceId { get; set; }
    public virtual WorkerService WorkerService { get; set; }

}