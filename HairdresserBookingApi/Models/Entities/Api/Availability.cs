using System.ComponentModel.DataAnnotations;

namespace HairdresserBookingApi.Models.Entities.Api;

public class Availability
{
    [Required] public int Id { get; set; }


    [Required] public DateTime Start { get; set; }
    [Required] public DateTime End { get; set; }

    [Required] public int WorkerId { get; set; }
    public virtual Worker Worker { get; set; }
}